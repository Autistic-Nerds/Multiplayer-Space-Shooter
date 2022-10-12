using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CosmosEngine.Netcode
{
	public class NetcodeTransport
	{
		public const int SIO_UDP_CONNRESET = -1744830452;
		private event Action<NetcodeMessage, IPEndPoint> onReceiveMessageEvent = delegate { };
		private event Action onDisconnectedEvent = delegate { };

		private bool connected;
		private bool server;

		private UdpClient socket;
		private IPEndPoint endPoint;
		private Task receiveMessageTask;

		private readonly object m_queueLock = new object();
		private bool useSiumlatedLatency;
		private List<Msg> messageQueue;
		private float packageLoss;
		private float latency;
		private Task latencySimulatorTask;

		public Action OnDisconnected { get => onDisconnectedEvent; set => onDisconnectedEvent = value; }

		private class Msg
		{
			public byte[] Data { get; set; }
			public float TimeToSend { get; set; }
			public bool Alive { get; set; }
			public IPEndPoint EndPoint { get; set; }
		}

		public void SimulateLatency(float latency, float packageLoss)
		{
			if(useSiumlatedLatency)
			{
				useSiumlatedLatency = false;
				return;
			}

			messageQueue = new List<Msg>();
			useSiumlatedLatency = true;
			this.latency = latency;
			this.packageLoss = packageLoss;

			latencySimulatorTask = new Task(LatencySendData);
			latencySimulatorTask.Start();
		}

		public void SetupServer(int port)
		{
			Console.WriteLine($"Server listening on port: {port}");
			socket = new UdpClient(port);
			endPoint = new IPEndPoint(IPAddress.Any, port);
			server = true;
			Connect();
		}

		public void SetupClient(IPAddress adress, int port)
		{
			Console.WriteLine($"Connecting as client to {adress}:{port}");
			socket = new UdpClient();
			endPoint = new IPEndPoint(adress, port);
			socket.Connect(endPoint);
			Connect();
		}

		private void Connect()
		{
			socket.Client.IOControl(
			   (IOControlCode)SIO_UDP_CONNRESET,
			   new byte[] { 0, 0, 0, 0 },
			   null
		   );
			connected = true;
			receiveMessageTask = new Task(ListenForMessage);
			receiveMessageTask.Start();
		}

		public void Disconnect()
		{
			OnDisconnected.Invoke();
			connected = false;
			socket.Close();
			RemoveAllListeners();
			Debug.Log($"NetcodeHandler was disconnected");
		}

		#region SEND

		public void SendToServer(NetcodeMessage netcodeMessage)
		{
			SendData(NetcodeSerializer.Serialize(netcodeMessage));
		}

		public void SendToClient(NetcodeMessage netcodeMessage, IPEndPoint groupEP)
		{
			SendDataToEndPoint(NetcodeSerializer.Serialize(netcodeMessage), groupEP);
		}

		private void SendData(byte[] data)
		{
			if(socket == null)
			{
				Debug.Log($"Trying to send message, but the handler has not been established.", LogFormat.Error);
				return;
			}
			else if(!connected)
			{
				Debug.Log($"Can't send data to server when not connected.", LogFormat.Warning);
				return;
			}
			Console.WriteLine($"SEND [{data.Length}]: {Encoding.UTF8.GetString(data)}");
			//Debug.Log($"Sending Message: {Encoding.UTF8.GetString(data)}");
			if (useSiumlatedLatency)
			{
				SendLatencyData(data, null);
			}
			else
				socket.Send(data, data.Length);
		}

		private void SendDataToEndPoint(byte[] data, IPEndPoint groupEP)
		{
			if (socket == null)
			{
				Debug.Log($"Trying to send message, but the handler has not been established.", LogFormat.Error);
				return;
			}
			else if (!connected)
			{
				Debug.Log($"Can't send data to endpoint when not connected.", LogFormat.Warning);
				return;
			}
			//Debug.Log($"Sending Message: {Encoding.UTF8.GetString(data)} to {groupEP}");
			Console.WriteLine($"SEND [{data.Length}]: {Encoding.UTF8.GetString(data)} TO {groupEP}");
			if (useSiumlatedLatency)
			{
				SendLatencyData(data, endPoint);
			}
			else
				socket.Send(data, data.Length, groupEP);
		}

		private void SendLatencyData(byte[] data, IPEndPoint? endPoint)
		{
			if (Random.Value < packageLoss)
			{
				Console.WriteLine($"Package was lost");
				return;
			}

			Msg msg = new Msg()
			{
				Data = data,
				TimeToSend = Time.ElapsedTime + (latency / 1000f),
				EndPoint = endPoint,
				Alive = true,
			};
			lock (m_queueLock)
			{
				messageQueue.Add(msg);
			}
		}

		private async void LatencySendData()
		{
			while (useSiumlatedLatency && connected)
			{
				do
				{
					await Task.Delay(TimeSpan.Zero);
				} while (messageQueue.Count == 0);

				float time = Time.ElapsedTime;
				lock (m_queueLock)
				{
					for (int i = 0; i < messageQueue.Count; i++)
					{
						Msg msg = messageQueue[i];
						if (msg.TimeToSend < time)
						{
							if (msg.EndPoint != null)
							{
								//Sending as server
								socket.Send(msg.Data, msg.Data.Length, msg.EndPoint);
							}
							else
							{
								//Sending as client
								socket.Send(msg.Data, msg.Data.Length);
							}
							msg.Alive = false;
						}
					}
					messageQueue.RemoveAll(item => !item.Alive);
				}
			}
		}

		#endregion

		#region RECEIVE

		private void ListenForMessage()
		{
			try
			{
				while (connected)
				{
					byte[] data = socket.Receive(ref endPoint);
					string dataDecoded = Encoding.UTF8.GetString(data);
					Console.WriteLine($"RECIEVED [{data.Length}]: {dataDecoded} FROM {endPoint}");
					//Debug.Log($"RECIEVED: {dataDecoded} [{Encoding.UTF8.GetByteCount(dataDecoded)}] -- {endPoint}");

					NetcodeMessage message = new NetcodeMessage()
					{
						Data = NetcodeSerializer.Deserialize(data),
					};
					onReceiveMessageEvent.Invoke(message, endPoint);
				}
			}
			catch (SocketException e)
			{
				Debug.Log(e, LogFormat.Error);
			}
			finally
			{
				Disconnect();
			}
		}

		public void AddListener(Action<NetcodeMessage, IPEndPoint> callback)
		{
			onReceiveMessageEvent += callback;
		}

		public void RemvoeListener(Action<NetcodeMessage, IPEndPoint> callback)
		{
			onReceiveMessageEvent -= callback;
		}

		public void RemoveAllListeners() => onReceiveMessageEvent = delegate { };

		#endregion
	}
}