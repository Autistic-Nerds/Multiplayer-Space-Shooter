using System.Collections.Generic;
using System.Net;
using System.Timers;
using System.Text;
using CosmosEngine;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using CosmosEngine.Modules;
using CosmosEngine.Collection;
using SharpDX.Direct3D9;
using System;

namespace CosmosEngine.Netcode
{
	public class NetcodeServer : Singleton<NetcodeServer>
	{
		private string ip = "127.0.0.1";
		private int port = 7000;
		private float serverTickRate = 20;
		protected readonly List<NetcodeClient> connectedClients = new List<NetcodeClient>();
		protected readonly Bag<NetcodeIdentity> netcodeObjects = new Bag<NetcodeIdentity>();

		private NetcodeTransport transport;
		private bool isServerConnection;
		private double serverTickTime;
		private double serverTickDifference;

		private readonly object serializationLock = new object();
		private readonly List<SerializeNetcodeData> serializationObjects = new List<SerializeNetcodeData>();

		private readonly object m_rpcLock = new object();
		private List<NetcodeRPC> remoteProcedureCalls = new List<NetcodeRPC>();

		private List<byte[]> recievedMessages = new List<byte[]>();

		public bool IsServerConnection => isServerConnection;
		public NetcodeTransport NetcodeTransport => transport ??= new NetcodeTransport();

		public NetcodeServer()
		{
		}

		private void OnNetcodeIdentityInstantiated(NetcodeIdentity item)
		{
			int indentity = netcodeObjects.Count;
			netcodeObjects[indentity] = item;
			item.NetId = (uint)indentity;
		}

		protected override void Update()
		{
			if (!NetcodeHandler.IsConnected)
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

		protected void StartServer()
		{
			if (!Application.IsRunning)
				return;

			Debug.Log($"Start Server");
			transport = new NetcodeTransport();
			transport.SetupServer(port);

			StartObjectSerialization();
			isServerConnection = true;
			OnConnected();
			NetcodeHandler.IsServer = true;
			NetcodeHandler.IsClient = true;
			OnStartServer();
			ObjectDelegater.CreateNewDelegation<NetcodeIdentity>(OnNetcodeIdentityInstantiated);
		}

		protected void StartClient()
		{
			if (!Application.IsRunning)
				return;

			Debug.Log($"Start Client");
			if (IPAddress.TryParse(ip, out IPAddress address))
			{
				transport = new NetcodeTransport();
				transport.SetupClient(address, port);
				transport.SendToServer(new NetcodeMessage()
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
			NetcodeHandler.IsClient = true;
			OnStartClient();
		}

		public void SimulateLatency(float latency, float packageLoss) => NetcodeTransport.SimulateLatency(latency, packageLoss);

		protected virtual void OnConnected()
		{
			transport.AddListener(ReceiveNetcodeMessage);
			NetcodeHandler.IsConnected = true;
		}

		protected virtual void OnStartServer()
		{

		}

		protected virtual void OnStartClient()
		{

		}

		protected virtual void OnClientConnected(NetcodeClient client)
		{

		}

		private void StartObjectSerialization()
		{
			//Application.targetFrameRate = 45;
		}

		protected override void LateUpdate()
		{
			if (transport == null)
				return;

			if (Time.ElapsedTime >= (serverTickTime))
			{
				DeserializeNetIdentityObjects();
				SerializeNetIdentityObjects();

				double delta = 1d / (double)serverTickRate;
				serverTickDifference = ((double)Time.ElapsedTime - serverTickTime - delta);
				serverTickTime = Time.ElapsedTime + delta;
			}

			lock(m_rpcLock)
			{
				if (remoteProcedureCalls.Count > 0)
				{
					List<NetcodeIdentity> netObjects = new List<NetcodeIdentity>(FindObjectsOfType<NetcodeIdentity>());
					foreach (NetcodeRPC rpc in remoteProcedureCalls)
					{
						NetcodeIdentity identity = netObjects.Find(item => item.NetId == rpc.NetId);
						if (identity != null)
						{
							identity.ExecuteRpc(rpc.Call);
						}
					}
					remoteProcedureCalls.Clear();
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

		#region Send

		public void SendToConnectedClients(NetcodeMessage message)
		{
			Debug.Log($"Sending global message to all clients: {connectedClients.Count}");
			//Debug.Log($"SEND: {Encoding.UTF8.GetString(NetcodeSerializer.Serialize(message))}");
			foreach(NetcodeClient client in connectedClients)
			{
				NetcodeTransport.SendToClient(message, client);
			}
		}

		#endregion

		#region Object Serialize and Deserialize

		private void SerializeNetIdentityObjects()
		{
			NetcodeIdentity[] netObjects = FindObjectsOfType<NetcodeIdentity>();
			foreach (NetcodeIdentity netIdentity in netObjects)
			{
				//I should only serialize if I have authority or I am the server.
				if (!netIdentity.HasAuthority)
					continue;

				Debug.Log($"I SERIALIZE FOR OBJECT: {netIdentity.NetId}");

				SerializeNetcodeData data = netIdentity.SerializeFromObject();
				if (data.Data.Count == 0)
					continue;
				NetcodeMessage message = new NetcodeMessage()
				{
					Data = data,
				};

				if (!IsServerConnection)
				{
					transport.SendToServer(message);
				}
				else
				{
					SendToConnectedClients(message);
				}
			}
		}

		private void DeserializeNetIdentityObjects()
		{
			List<NetcodeIdentity> netObjects = new List<NetcodeIdentity>();
			netObjects.AddRange(FindObjectsOfType<NetcodeIdentity>());
			lock(serializationLock)
			{
				foreach (SerializeNetcodeData data in serializationObjects)
				{
					NetcodeIdentity netIdentity = netObjects.Find(item => item.NetId == data.NetId);
					if (netIdentity == null)
					{
						continue;
					}
					if (netIdentity.HasAuthority)
						continue;

					Debug.Log($"I DESERIALIZE TO OBJECT: {netIdentity.NetId}");
					netIdentity.DeserializeToObject(data);
				}
			}
		}

		private void ReceiveSerializedNetcode(SerializeNetcodeData data)
		{
			lock(serializationLock)
			{
				if(IsServerConnection)
				{
					SendToConnectedClients(new NetcodeMessage()
					{
						Data = data,
					});
				}
				Console.WriteLine($"Received Serialized Netcode: {data.NetId}");
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
						NetcodeClient client = new NetcodeClient(endPoint);
						OnClientConnected(client);
						connectedClients.Add(client);

						transport.SendToClient(new NetcodeMessage()
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
					NetcodeAcknowledge ack = new NetcodeAcknowledge()
					{
						RPI = rpc.RPI,
					};
					if (isServerConnection)
					{
						transport.SendToClient(new NetcodeMessage()
						{
							Data = ack,
						}, endPoint);
					}
					else
					{
						transport.SendToServer(new NetcodeMessage()
						{
							Data = ack,
						});
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
						transport.SendToClient(msg, endPoint);
					}
					else
					{
					}
					break;
			}
		}

		private void OnApplicationQuit()
		{
			if (transport != null)
				transport.Disconnect();
		}
	}
}