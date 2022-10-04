using CosmosEngine;

namespace Cosmos.Entity
{
	public class Transform : EntityComponent
	{
		public Vector2 position;
		public float rotation;
		public Vector2 scale;

		public Transform()
		{
			position = Vector2.Zero;
			rotation = 0;
			scale = Vector2.One;
		}
	}
}