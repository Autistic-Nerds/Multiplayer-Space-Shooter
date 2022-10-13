using Newtonsoft.Json;
using System;

namespace CosmosEngine.Netcode
{
	[System.Serializable]
	public class RemoteProcedureCall : IEquatable<RemoteProcedureCall>, IComparable<RemoteProcedureCall>
	{
		//When an RPC is invoked
		//Check if an identical (same ID and method) RPC message is within the queue.
		//If an identical RPC exist, replace it with the newest RPC.
		//Add the RPC to a call queue

		//Send the RPC to the reciever.
		//Every RTT resend the package.

		//When receiver gets the message.
		//Send an acknowledge message to sender, confirming that the RPC was recieved.
		//Remove the RPC from the call queue.

		public string Method { get; set; }
		public uint Index { get; set; }
		public uint RPI { get; set; }
		public string[] Args { get; set; }
		[JsonIgnore]
		public NetcodeClient Target { get; set; }

		public int CompareTo(RemoteProcedureCall other)
		{
			return RPI.CompareTo(other.RPI);
		}

		public bool Equals(RemoteProcedureCall other)
		{
			if(Index.Equals(other.Index))
			{
				if (Method.Equals(other.Method))
				{
					//return Args.Equals(other.Args, StringComparison.CurrentCultureIgnoreCase);
				}
			}
			return false;
		}
	}
}