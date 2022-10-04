using CosmosEngine.Netcode.Serialization;
using System;
using System.Collections.Generic;

namespace CosmosEngine.Netcode
{
	public enum NetcodeMessageType
	{
		Empty,
		Connect,
		Disconnect,
		Data,
		RPC,
	}


	[Serializable]
	public class NetcodeMessage
	{
		public NetcodeData Data { get; set; } 
	}

	[Serializable]
	public abstract class NetcodeData
	{
		public abstract NetcodeMessageType Type { get; }
	}

	[Serializable]
	public class ClientConnectData : NetcodeData
	{
		public override NetcodeMessageType Type => NetcodeMessageType.Connect;
	}

	[Serializable]
	public class ClientDisconnectData : NetcodeData
	{
		public override NetcodeMessageType Type => NetcodeMessageType.Disconnect;
	}
	
	[Serializable]
	public class SerializeNetcodeData : NetcodeData
	{
		public uint NetId { get; set; }
		public List<SerializedObjectData> Data { get; set; } = new List<SerializedObjectData>();
		public override NetcodeMessageType Type => NetcodeMessageType.Data;
	}

	[Serializable]
	public class NetcodeRPC : NetcodeData
	{
		public uint NetId { get; set; }
		public uint BehaviourId { get; set; }
		public string Method { get; set; }
		public List<string> Parameters { get; set; }
		public override NetcodeMessageType Type => NetcodeMessageType.RPC;
	}
}