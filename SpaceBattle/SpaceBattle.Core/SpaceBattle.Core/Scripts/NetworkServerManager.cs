using CosmosEngine;
using CosmosEngine.Netcode;

namespace SpaceBattle
{
	internal class NetworkServerManager : NetcodeServer
	{
		private NetworkSpawner spawner;

		protected override void OnConnected()
		{
			base.OnConnected();
			GameObject go = new GameObject("NetcodeSpawner");
			go.AddComponent<NetcodeIdentity>().NetId = 1;
			spawner = go.AddComponent<NetworkSpawner>();
		}

		protected override void OnStartServer()
		{
			spawner.CreateServerPlayer();
		}

		protected override void OnClientConnected(NetcodeClient client)
		{
			base.OnClientConnected(client);
			spawner.CreateNewPlayer(client, connectedClients.ToArray());
		}

		protected override void Update()
		{
			base.Update();
			//Debug.LogTable($"Netcode Objects [{netcodeObjects.Count}]", netcodeObjects);
		}
	}
}