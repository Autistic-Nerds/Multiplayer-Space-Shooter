using CosmosEngine;
using CosmosEngine.Netcode;
using System.Collections.Generic;

namespace SpaceBattle
{
	internal class NetworkSpawner : NetcodeBehaviour
	{
		private readonly List<NetcodeIdentity> netcodeObjects = new List<NetcodeIdentity>();
 		private uint netId = 255;
		public void CreateNewPlayer(NetcodeClient newClient, NetcodeClient[] existingClients)
		{
			//We only call this method from the server.
			if (!NetcodeHandler.IsServer)
				return;

			//Generate random position and unique netId
			//NetID is actually not unique, this is a very crude way to do it. Works for now.
			Vector2 position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
			uint assignedId = netId++;

			Debug.Log("Assigned ID: " + assignedId);
			Debug.Log($"Create new player for {(newClient == null ? "null" : newClient)}");
			if (newClient != null)
			{

				//Spawn a player ship on the new client for each player ship already on the server.
				foreach (NetcodeIdentity netObject in netcodeObjects)
				{
					Rpc(nameof(SpawnNewPlayer), newClient, netObject.NetId, false, netObject.Transform.Position);
				}

				//Spawn the new player's own ship.
				Rpc(nameof(SpawnNewPlayer), newClient, assignedId, true, position);

				//Last we send message to all already connected clients to spawn a new ship.
				foreach (NetcodeClient client in existingClients)
				{
					Rpc(nameof(SpawnNewPlayer), client, assignedId, false, position);
				}

				//Spawn a version of the new player ship on the server.
				SpawnNewPlayer(assignedId, false, position);
			}
		}

		protected override void Update()
		{
			base.Update();
			Debug.LogTable(netcodeObjects);
		}

		public void CreateServerPlayer()
		{
			SpawnNewPlayer(netId++, true, Vector2.Zero);
		}

		private void SpawnNewPlayer(uint netId, bool isMine, Vector2 position)
		{
			Debug.Log($"Spawn Command: {netId} - {isMine} {position}", LogFormat.Message, LogOption.None);
			GameObject go = new GameObject("Player");
			NetcodeIdentity net = go.AddComponent<NetcodeIdentity>();
			go.AddComponent<NetcodeTransform>();
			go.AddComponent<NetworkMover>();

			go.Transform.Position = position;
			net.HasAuthority = isMine;
			net.NetId = netId;

			go.AddComponent<SpriteRenderer>().Sprite = ArtContent.InterceptorPlayer;
			netcodeObjects.Add(net);
		}
	}
}