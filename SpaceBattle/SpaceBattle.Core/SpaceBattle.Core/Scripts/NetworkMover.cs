using CosmosEngine;
using CosmosEngine.Netcode;

namespace SpaceBattle
{
	internal class NetworkMover : NetcodeBehaviour
	{
		protected override void Update()
		{
			if(FindObjectOfType<NetcodeServer>().IsServerConnection)
			{
				Move();
			}
		}

		private void Move()
		{
			Vector2 move = new Vector2(InputManager.GetAxis("horizontal"), InputManager.GetAxis("vertical"));
			Debug.Log($"MOVE: {move}");
			Transform.Translate(move * Time.DeltaTime);
		}
	}
}