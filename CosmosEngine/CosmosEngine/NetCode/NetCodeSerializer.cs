using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CosmosEngine.Netcode
{
	public static class NetcodeSerializer
	{
		#region Messages

		public static byte[] Serialize(NetcodeMessage netcodeMessage)
		{
			string serializedMessage = JsonConvert.SerializeObject(netcodeMessage);
			byte[] jsonData = Encoding.UTF8.GetBytes(serializedMessage);
			return jsonData;
		}

		public static NetcodeData Deserialize(byte[] data)
		{
			string json = Encoding.UTF8.GetString(data);
			JObject? netcodeMessage = JObject.Parse(json);

			if (netcodeMessage != null)
			{
				JToken netcodeData = netcodeMessage["Data"];
				if (netcodeData == null)
				{
					Debug.Log($"Recieved a null message", LogFormat.Warning);
				}
				else
				{
					JToken? netcodeType = netcodeData["Type"];
					if (netcodeType?.Type is JTokenType.Integer)
					{
						NetcodeMessageType type = (NetcodeMessageType)netcodeType.Value<int>();
						NetcodeData p = type switch
						{
							NetcodeMessageType.Empty => default(NetcodeData),
							NetcodeMessageType.Connect => netcodeData.ToObject<ClientConnectData>(),
							NetcodeMessageType.Disconnect => netcodeData.ToObject<ClientDisconnectData>(),
							NetcodeMessageType.Data => netcodeData.ToObject<SerializeNetcodeData>(),
							NetcodeMessageType.RPC => netcodeData.ToObject<NetcodeRPC>(),
							NetcodeMessageType.RTT => netcodeData.ToObject<RoundtripTime>(),
							NetcodeMessageType.ACK => netcodeData.ToObject<NetcodeAcknowledge>(),
							_ => default(NetcodeData),
						};
						return p;
					}
				}
			}
			return default(NetcodeData);
		}

		#endregion
	}
}
