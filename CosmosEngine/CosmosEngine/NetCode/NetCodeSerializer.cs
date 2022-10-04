using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CosmosEngine.NetCode
{
	public static class NetCodeSerializer
	{
		public static byte[] Serialize(NetCodeMessage netCodeMessage)
		{
			string serializedMessage = JsonConvert.SerializeObject(netCodeMessage);
			byte[] jsonData = Encoding.UTF8.GetBytes(serializedMessage);
			return jsonData;
		}

		public static NetCodeData Deserialize(byte[] data)
		{
			string json = Encoding.UTF8.GetString(data);
			JObject? netCodeMessage = JObject.Parse(json);

			if (netCodeMessage != null)
			{
				JToken netCodeData = netCodeMessage["Data"];
				if (netCodeData == null)
				{
					Debug.Log($"Recieved a null message", LogFormat.Warning);
				}
				else
				{
					JToken? netCodeType = netCodeData["Type"];
					if (netCodeType?.Type is JTokenType.Integer)
					{
						NetCodeMessageType type = (NetCodeMessageType)netCodeType.Value<int>();
						NetCodeData p = type switch
						{
							NetCodeMessageType.Empty => default(NetCodeData),
							NetCodeMessageType.Connect => netCodeData.ToObject<ClientConnectData>(),
							NetCodeMessageType.Disconnect => netCodeData.ToObject<ClientDisconnectData>(),
							NetCodeMessageType.Data => netCodeData.ToObject<SerializeNetCodeData>(),
							_ => default(NetCodeData),
						};
						return p;
					}
				}
			}
			return default(NetCodeData);
		}
	}
}
