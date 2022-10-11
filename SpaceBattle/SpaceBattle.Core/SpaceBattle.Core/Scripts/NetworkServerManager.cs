using CosmosEngine.Netcode;

namespace SpaceBattle
{
	internal class NetworkServerManager : NetcodeServer
	{
		protected override void OnClientConnected(NetcodeClient client)
		{
			base.OnClientConnected(client);
		}
	}
}