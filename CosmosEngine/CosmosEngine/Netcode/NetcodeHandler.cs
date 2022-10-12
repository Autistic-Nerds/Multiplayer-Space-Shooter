namespace CosmosEngine.Netcode
{
	public static class NetcodeHandler
	{
		private static bool isConnected;
		private static bool isClient;
		private static bool isServer;

		/// <summary>
		/// Returns true if this application is conneccted to a network session.
		/// </summary>
		public static bool IsConnected { get => isConnected; internal set => isConnected = value; }
		/// <summary>
		/// Returns true if this application is a client of a network session.
		/// </summary>
		public static bool IsClient { get => isClient && IsConnected; internal set => isClient = value; }
		/// <summary>
		/// Returns true if this application is the server/host of a network session.
		/// </summary>
		public static bool IsServer { get => isServer && IsConnected; internal set => isServer = value; }
	}
}