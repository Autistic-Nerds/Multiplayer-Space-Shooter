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
		RTT,
		ACK
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
	public class NetcodeRPC : NetcodeData, IEquatable<NetcodeRPC>
	{
		/// <summary>
		/// Reliable package index.
		/// </summary>
		public uint RPI { get; set; }
		public uint NetId { get; set; }
		public RemoteProcedureCall Call { get; set; }
		public override NetcodeMessageType Type => NetcodeMessageType.RPC;

		public bool Equals(NetcodeRPC other)
		{
			if(NetId == other.NetId)
			{
				return true;
			}
			return false;
		}
	}

	[Serializable]
	public class RoundtripTime : NetcodeData
	{
		public override NetcodeMessageType Type => NetcodeMessageType.RTT;
	}

	[Serializable]
	public class NetcodeAcknowledge : NetcodeData
	{
		public uint RPI { get; set; }
		public override NetcodeMessageType Type => NetcodeMessageType.ACK;
	}
}