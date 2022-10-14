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
			if (HasAuthority)
			{
				Vector2 move = new Vector2(InputManager.GetAxis("horizontal"), InputManager.GetAxis("vertical"));
				Transform.Translate(move * 4.7f * Time.DeltaTime, Space.Self);

				Vector2 mouse = Camera.Main.ScreenToWorld(InputManager.MousePosition);
				Transform.RotateTowards(mouse, 200f);
			}
		}
	}
}