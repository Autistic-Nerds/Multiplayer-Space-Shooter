using System.Collections.Generic;
using System.Net;
using System.Timers;
using System.Text;
using CosmosEngine;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;

namespace CosmosEngine.Netcode
{
	public class NetcodeServer : Singleton<NetcodeServer>
	{
		private string ip = "127.0.0.1";
		private int port = 7000;
		private float serverTickRate = 5;
		private readonly List<IPEndPoint> connectedClients = new List<IPEndPoint>();
		private readonly List<NetcodeIdentity> netcodeBehaviours = new List<NetcodeIdentity>();

		private NetcodeTransport handler;
		private bool isServerConnection;
		private double serverTickTime;
		private double serverTickDifference;

		private readonly object serializationLock = new object();
		private readonly List<SerializeNetcodeData> serializationObjects = new List<SerializeNetcodeData>();

		private readonly object m_rpcLock = new object();
		private List<NetcodeRPC> remoteProcedureCalls = new List<NetcodeRPC>();

		private List<byte[]> recievedMessages = new List<byte[]>();

		public bool IsServerConnection => isServerConnection;
		public NetcodeTransport NetcodeHandler => handler;

		protected override void Update()
		{
			if (handler == null)
			{
				if (InputManager.GetButtonDown("c"))
				{
					StartClient();
				}
				else if (InputManager.GetButtonDown("z"))
				{
					StartServer();
				}
			}
		}

		private void StartServer()
		{
			if (!Application.IsRunning)
				return;
			Debug.Log($"Start Server");
			handler = new NetcodeTransport();
			handler.SetupServer(port);

			StartObjectSerialization();
			isServerConnection = true;
			OnConnected();
		}

		private void StartClient()
		{
			if (!Application.IsRunning)
				return;
			Debug.Log($"Start Client");

			if (IPAddress.TryParse(ip, out IPAddress address))
			{
				handler = new NetcodeTransport();
				handler.SetupClient(address, port);
				handler.SendToServer(new NetcodeMessage()
				{
					Data = new ClientConnectData(),
				});
			}
			else
			{
				Debug.Log($"Invalid IP adress: {ip}", LogFormat.Error);
				return;
			}
			OnConnected();
		}

		public void OnConnected()
		{
			handler.AddListener(ReceiveNetcodeMessage);
			handler.SimulateLatency(60, 0.2f);
		}

		private void StartObjectSerialization()
		{
			//Application.targetFrameRate = 45;
		}

		protected override void LateUpdate()
		{
			if (handler == null)
				return;

			if (Time.ElapsedTime >= (serverTickTime))
			{
				if (isServerConnection)
				{
					SerializeNetIdentityObjects();
					SimulateRecieve();
				}
				else
				{
					DeserializeNetIdentityObjects();
				}
				double delta = 1d / (double)serverTickRate;
				serverTickDifference = ((double)Time.ElapsedTime - serverTickTime - delta);
				serverTickTime = Time.ElapsedTime + delta;
			}

			lock(m_rpcLock)
			{
				foreach(NetcodeRPC rpc in remoteProcedureCalls)
				{
					foreach (RemoteProcedureCall call in rpc.Call)
					{
						Debug.Log($"RPC for netID: {rpc.NetId} - Method: {call.Method} with args: {call.Args}");
					}
				}
				remoteProcedureCalls.Clear();
			}
		}

		private void SerializeNetIdentityObjects()
		{
			NetcodeIdentity[] netObjects = FindObjectsOfType<NetcodeIdentity>();
			foreach (NetcodeIdentity netIdentity in netObjects)
			{
				SerializeNetcodeData data = netIdentity.SerializeFromObject();
				if (data.Data.Count == 0)
					continue;
				NetcodeMessage message = new NetcodeMessage()
				{
					Data = data,
				};

				foreach (IPEndPoint client in connectedClients)
				{
					handler.SendToClient(message, client);
				}
			}
		}

		private void SimulateRecieve()
		{
			if (connectedClients.Count > 0)
				return;

			foreach (byte[] data in recievedMessages)
			{
				//Debug.Log($"MESSAGE: {Encoding.UTF8.GetString(data)}");
				NetcodeMessage message = new NetcodeMessage()
				{
					Data = NetcodeSerializer.Deserialize(data),
				};
				if(message.Data.Type == NetcodeMessageType.Data)
				{
					SerializeNetcodeData netcodeData = (SerializeNetcodeData)message.Data;
					serializationObjects.Add(netcodeData);
				}
			}
			recievedMessages.Clear();

			List<NetcodeIdentity> netObjects = new List<NetcodeIdentity>();
			netObjects.AddRange(FindObjectsOfType<NetcodeIdentity>());
			lock (serializationLock)
			{
				//Debug.Log($"Object to serialize: {netObjects.Count} | Data to deserialize: {serializationObjects.Count}");
				foreach (SerializeNetcodeData netData in serializationObjects)
				{
					NetcodeIdentity netIdentity = netObjects.Find(item => item.NetId == netData.NetId);
					netIdentity.DeserializeToObject(netData);
				}
				serializationObjects.Clear();
			}
		}

		#region Object Serialize and Deserialize

		private void DeserializeNetIdentityObjects()
		{
			List<NetcodeIdentity> netObjects = new List<NetcodeIdentity>();
			netObjects.AddRange(FindObjectsOfType<NetcodeIdentity>());
			lock(serializationLock)
			{
				foreach (SerializeNetcodeData data in serializationObjects)
				{
					NetcodeIdentity netIdentity = netObjects.Find(item => item.NetId == data.NetId);
					netIdentity.DeserializeToObject(data);
				}
			}
		}

		private void ReceiveSerializedNetcode(SerializeNetcodeData data)
		{
			lock(serializationLock)
			{
				int index = serializationObjects.FindIndex(item => item.NetId == data.NetId);
				if(index >= 0)
				{
					serializationObjects[index] = data;
				}
				else
				{
					serializationObjects.Add(data);
				}
			}
		}

		#endregion

		private void ReceiveNetcodeMessage(NetcodeMessage message, IPEndPoint endPoint)
		{
			if (message == null)
				return;

			switch (message.Data.Type)
			{
				case NetcodeMessageType.Data:
					ReceiveSerializedNetcode((SerializeNetcodeData)message.Data);
					break;
				case NetcodeMessageType.Connect:
					if (isServerConnection)
					{
						Debug.Log($"Received Client Connect Message from - {endPoint}");
						connectedClients.Add(endPoint);
						handler.SendToClient(new NetcodeMessage()
						{
							Data = new ClientConnectData(),
						}, endPoint);
					}
					else
					{
						Debug.Log($"Received Accepted Connect Message");
					}
					break;
				case NetcodeMessageType.RPC:
					NetcodeRPC rpc = (NetcodeRPC)message.Data;
					if(isServerConnection)
					{
						handler.SendToClient(new NetcodeMessage()
						{
							Data = new NetcodeAcknowledge()
							{
								Key = rpc.ReliableKey,
							}
						}, endPoint);
					}
					lock (m_rpcLock)
					{
						remoteProcedureCalls.Add(rpc);
					}
					break;
				case NetcodeMessageType.RTT:
					if(isServerConnection)
					{
						NetcodeMessage msg = new NetcodeMessage()
						{
							Data = new RoundtripTime(),
						};
						handler.SendToClient(msg, endPoint);
					}
					else
					{
					}
					break;
			}
		}

		private void OnApplicationQuit()
		{
			if (handler != null)
				handler.Disconnect();
		}
	}
}