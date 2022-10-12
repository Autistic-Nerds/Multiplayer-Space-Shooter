using CosmosEngine;
using CosmosEngine.Netcode;

namespace SpaceBattle
{
	internal class NetworkMover : NetcodeBehaviour
	{
		protected override void Update()
		{
			Move();
		}

		private void Move()
		{
			Vector2 move = new Vector2(InputManager.GetAxis("horizontal"), InputManager.GetAxis("vertical"));
			Transform.Translate(move * Time.DeltaTime);
		}
	}
}