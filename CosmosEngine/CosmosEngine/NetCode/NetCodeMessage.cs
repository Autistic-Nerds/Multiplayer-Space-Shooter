using CosmosEngine.NetCode.Serialization;
using System;
using System.Collections.Generic;

namespace CosmosEngine.NetCode
{
	public enum NetCodeMessageType
	{
		Empty,
		Connect,
		Disconnect,
		Data,
	}


	[Serializable]
	public class NetCodeMessage
	{
		public NetCodeData Data { get; set; } 
	}

	[Serializable]
	public abstract class NetCodeData
	{
		public abstract NetCodeMessageType Type { get; }
	}

	[Serializable]
	public class ClientConnectData : NetCodeData
	{
		public override NetCodeMessageType Type => NetCodeMessageType.Connect;
	}

	[Serializable]
	public class ClientDisconnectData : NetCodeData
	{
		public override NetCodeMessageType Type => NetCodeMessageType.Disconnect;
	}
	
	[Serializable]
	public class SerializeNetCodeData : NetCodeData
	{
		public uint NetId { get; set; }
		public List<SerializedObjectData> Data { get; set; } = new List<SerializedObjectData>();
		public override NetCodeMessageType Type => NetCodeMessageType.Data;
	}
}