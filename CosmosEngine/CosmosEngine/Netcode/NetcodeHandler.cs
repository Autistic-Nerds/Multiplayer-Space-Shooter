namespace CosmosEngine.Netcode
{
	internal static class NetcodeHandler
	{
		public static bool IsConnected { get; internal set; }
		public static bool IsClient { get; internal set; }
		public static bool IsServer { get; internal set; }
	}
}