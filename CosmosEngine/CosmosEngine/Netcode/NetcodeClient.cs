using System;
using System.Net;

namespace CosmosEngine.Netcode
{
	public class NetcodeClient : IEquatable<NetcodeClient>, IEquatable<IPEndPoint>
	{
		private IPEndPoint endPoint;
		internal IPEndPoint EndPoint => endPoint;

		public NetcodeClient(IPEndPoint endPoint)
		{
			this.endPoint = endPoint;
		}

		public bool Equals(IPEndPoint other) => EndPoint.Equals(other);
		public bool Equals(NetcodeClient other) => EndPoint.Equals(other.EndPoint);
		public override string ToString() => EndPoint.ToString();

		public static implicit operator IPEndPoint(NetcodeClient client) => client.EndPoint;
	}
}