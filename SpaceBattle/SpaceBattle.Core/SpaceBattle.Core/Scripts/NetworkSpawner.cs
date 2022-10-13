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
			if (!NetcodeHandler.IsServer)
				return;

			Vector2 position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

			uint assignedId = netId++;
			Debug.Log("Assigned ID: " + assignedId);
			Debug.Log($"Create new player for {(newClient == null ? "null" : newClient)}");
			if (newClient != null)
			{
				SpawnNewPlayer(assignedId, false, position);
				foreach (NetcodeIdentity netObject in netcodeObjects)
				{
					Rpc(nameof(SpawnNewPlayer), newClient, netObject.NetId, false, netObject.Transform.Position);
				}

				Rpc(nameof(SpawnNewPlayer), newClient, assignedId, true, position);

				foreach (NetcodeClient client in existingClients)
				{
					Rpc(nameof(SpawnNewPlayer), client, assignedId, false, position);
				}
			}
		}

		private void SpawnNewPlayer(uint netId, bool isMine, Vector2 position)
		{
			Debug.Log($"Spawn Command: {netId} - {isMine} {position}");
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