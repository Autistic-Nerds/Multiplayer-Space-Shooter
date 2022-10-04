using CosmosEngine;
using CosmosEngine.NetCode;

namespace SpaceBattle
{
	internal class NetworkMover : NetCodeBehaviour
	{
		protected override void Update()
		{
			if(FindObjectOfType<NetCodeServer>().IsServerConnection)
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