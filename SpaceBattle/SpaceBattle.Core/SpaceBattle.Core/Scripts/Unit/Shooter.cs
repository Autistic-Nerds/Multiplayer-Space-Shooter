using CosmosEngine;
using CosmosEngine.InputModule;
using CosmosEngine.Netcode;
using System.Collections.Generic;

namespace SpaceBattle
{
	internal class Shooter : NetcodeBehaviour
	{
		private float cd;
		private List<Transform> projectiles = new List<Transform>();

		protected override void Update()
		{
			foreach(Transform p in projectiles)
			{
				p.Transform.Translate(Vector2.Up * 3f * Time.DeltaTime, Space.Self);
			}

			if (!IsConnected)
				return;

			if(InputManager.GetKey(Keys.Space))
			{
				if(cd < Time.ElapsedTime)
				{
					Rpc(nameof(Shoot), null);
					cd = Time.ElapsedTime + 0.2f;
				}
			}
		}

		[ClientRPC]
		private void Shoot()
		{
			Rpc(nameof(SpawnProjectile), null);
			SpawnProjectile();
		}

		[ServerRPC]
		private void SpawnProjectile()
		{
			GameObject obj = new GameObject("Projectile");
			obj.AddComponent<SpriteRenderer>().Sprite = DefaultGeometry.Circle;
			obj.Transform.LocalScale = new Vector2(0.2f, 0.2f);
			obj.Transform.Position = Transform.Position;
			obj.Transform.Rotation = Transform.Rotation;
			projectiles.Add(obj.Transform);
		}
	}
}