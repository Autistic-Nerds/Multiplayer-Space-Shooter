using System.Collections.Generic;
using System.Net;
using System.Timers;
using System.Text;
using CosmosEngine;

namespace CosmosEngine.NetCode
{
	public class NetCodeServer : GameBehaviour
	{
		private string ip = "127.0.0.1";
		private int port = 7000;
		private float serverTickRate = 30;
		private readonly List<IPEndPoint> connectedClients = new List<IPEndPoint>();
		private readonly List<NetCodeIdentity> netCodeBehaviours = new List<NetCodeIdentity>();

		private Timer serverTickTimer;
		private NetCodeHandler handler;
		private bool isServerConnection;
		private double serverTickTime;
		private double serverTickDifference;

		private readonly object serializationLock = new object();
		private readonly List<SerializeNetCodeData> serializationObjects = new List<SerializeNetCodeData>();

		public bool IsServerConnection => isServerConnection;

		protected override void Update()
		{
			if(handler == null)
			{
				if(InputManager.GetButtonDown("c"))
				{
					StartClient();
				}
				else if(InputManager.GetButtonDown("z"))
				{
					StartServer();
				}
			}
		}

		private List<byte[]> recievedMessages = new List<byte[]>();

		private void StartServer()
		{
			if (!Application.IsRunning)
				return;
			Debug.Log($"Start Server");
			handler = new NetCodeHandler();
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
				handler = new NetCodeHandler();
				handler.SetupClient(address, port);
				handler.SendToServer(new NetCodeMessage()
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
			handler.AddListener(ReceiveNetCodeMessage);
		}

		private void StartObjectSerialization()
		{
			//Application.targetFrameRate = 45;
		}

		protected override void LateUpdate()
		{
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
		}
		private void SerializeNetIdentityObjects()
		{
			NetCodeIdentity[] netObjects = FindObjectsOfType<NetCodeIdentity>();
			foreach (NetCodeIdentity netIdentity in netObjects)
			{
				NetCodeMessage message = new NetCodeMessage()
				{
					Data = netIdentity.SerializeFromObject(),
				};

				byte[] vs = NetCodeSerializer.Serialize(message);
				//Debug.Log($"SEND: {System.Text.Encoding.UTF8.GetString(vs)}");
				recievedMessages.Add(vs);
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
				Debug.Log($"MESSAGE: {Encoding.UTF8.GetString(data)}");
				NetCodeMessage message = new NetCodeMessage()
				{
					Data = NetCodeSerializer.Deserialize(data),
				};
				if(message.Data.Type == NetCodeMessageType.Data)
				{
					SerializeNetCodeData netCodeData = (SerializeNetCodeData)message.Data;
					serializationObjects.Add(netCodeData);
				}
			}
			recievedMessages.Clear();

			List<NetCodeIdentity> netObjects = new List<NetCodeIdentity>();
			netObjects.AddRange(FindObjectsOfType<NetCodeIdentity>());
			lock (serializationLock)
			{
				//Debug.Log($"Object to serialize: {netObjects.Count} | Data to deserialize: {serializationObjects.Count}");
				foreach (SerializeNetCodeData netData in serializationObjects)
				{
					NetCodeIdentity netIdentity = netObjects.Find(item => item.NetId == netData.NetId);
					netIdentity.DeserializeToObject(netData);
				}
				serializationObjects.Clear();
			}
		}

		private void DeserializeNetIdentityObjects()
		{
			List<NetCodeIdentity> netObjects = new List<NetCodeIdentity>();
			netObjects.AddRange(FindObjectsOfType<NetCodeIdentity>());
			lock(serializationLock)
			{
				foreach (SerializeNetCodeData data in serializationObjects)
				{
					NetCodeIdentity netIdentity = netObjects.Find(item => item.NetId == data.NetId);
					netIdentity.DeserializeToObject(data);
				}
			}
		}

		private void ReceiveSerializedNetCode(SerializeNetCodeData data)
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

		private void ReceiveNetCodeMessage(NetCodeMessage message, IPEndPoint endPoint)
		{
			if (message == null)
				return;

			switch (message.Data.Type)
			{
				case NetCodeMessageType.Data:
					ReceiveSerializedNetCode((SerializeNetCodeData)message.Data);
					break;
				case NetCodeMessageType.Connect:
					if (isServerConnection)
					{
						Debug.Log($"Received Client Connect Message from - {endPoint.ToString()}");
						connectedClients.Add(endPoint);
						handler.SendToClient(new NetCodeMessage()
						{
							Data = new ClientConnectData(),
						}, endPoint);
					}
					else
					{
						Debug.Log($"Received Accepted Connect Message");
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