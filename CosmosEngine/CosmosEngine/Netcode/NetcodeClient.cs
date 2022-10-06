using System;
using System.Net;

namespace CosmosEngine.Netcode
{
	internal class NetcodeClient : IEquatable<NetcodeClient>, IEquatable<EndPoint>
	{
		private EndPoint endPoint;
		internal EndPoint EndPoint => endPoint;

		public NetcodeClient(EndPoint endPoint)
		{
			this.endPoint = endPoint;
		}

		public bool Equals(EndPoint other) => EndPoint.Equals(other);
		public bool Equals(NetcodeClient other) => EndPoint.Equals(other.EndPoint);

		public static implicit operator EndPoint(NetcodeClient client) => client.EndPoint;
	}
}