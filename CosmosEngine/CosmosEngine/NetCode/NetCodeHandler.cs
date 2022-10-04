using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CosmosEngine.NetCode
{
	public class NetCodeHandler
	{
		public const int SIO_UDP_CONNRESET = -1744830452;
		private event Action<NetCodeMessage, IPEndPoint> onReceiveMessageEvent = delegate { };

		private bool connected;

		private UdpClient socket;
		private IPEndPoint endPoint;
		private Task receiveMessageTask;

		public void SetupServer(int port)
		{
			socket = new UdpClient(port);
			endPoint = new IPEndPoint(IPAddress.Any, port);
			Connect();
		}

		public void SetupClient(IPAddress adress, int port)
		{
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
			connected = false;
			socket.Close();
			RemoveAllListeners();
			Debug.Log($"NetCodeHandler was disconnected");
		}

		#region SEND

		public void SendToServer(NetCodeMessage netCodeMessage)
		{
			SendData(NetCodeSerializer.Serialize(netCodeMessage));
		}

		public void SendToClient(NetCodeMessage netCodeMessage, IPEndPoint groupEP)
		{
			SendDataToEndPoint(NetCodeSerializer.Serialize(netCodeMessage), groupEP);
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
			//Debug.Log($"Sending Message: {Encoding.UTF8.GetString(data)}");
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
			socket.Send(data, data.Length, groupEP);
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
					//Debug.Log($"RECIEVED: {dataDecoded} [{Encoding.UTF8.GetByteCount(dataDecoded)}] -- {endPoint}");

					NetCodeMessage message = new NetCodeMessage()
					{
						Data = NetCodeSerializer.Deserialize(data),
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

		public void AddListener(Action<NetCodeMessage, IPEndPoint> callback)
		{
			onReceiveMessageEvent += callback;
		}

		public void RemvoeListener(Action<NetCodeMessage, IPEndPoint> callback)
		{
			onReceiveMessageEvent -= callback;
		}

		public void RemoveAllListeners() => onReceiveMessageEvent = delegate { };

		#endregion
	}
}