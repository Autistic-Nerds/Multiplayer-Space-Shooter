using CosmosEngine;
using CosmosEngine.Netcode;
using System.Collections;

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
				if(InputManager.GetMouseButtonUp(1))
				{
					StartCoroutine(nameof(TeleportToLocation));
					return;
				}
				Vector2 move = new Vector2(InputManager.GetAxis("horizontal"), InputManager.GetAxis("vertical"));
				Transform.Translate(move * 4.7f * Time.DeltaTime, Space.Self);

				Vector2 mouse = Camera.Main.ScreenToWorld(InputManager.MousePosition);
				Transform.RotateTowards(mouse, 200f);
			}

			Debug.QuickLog($"Time: {Time.ElapsedTime}");
		}

		private IEnumerator TeleportToLocation()
		{
			Vector2 mouse = Camera.Main.ScreenToWorld(InputManager.MousePosition);
			yield return Wait.Until(() => Time.ElapsedTime > 10.0f);
			Transform.Position = mouse;
		}
	}
}