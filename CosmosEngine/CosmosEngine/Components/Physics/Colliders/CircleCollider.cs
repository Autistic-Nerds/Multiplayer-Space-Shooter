namespace CosmosEngine
{
	public class CircleCollider : Collider
	{
		private float radius;
		private float scaledRadius;
		private Vector2 offset;
		private Vector2 adjustedOffset;
		private bool usingDefaultRadius;

		public float Radius 
		{ 
			get => radius;
			set
			{
				radius = value;
				usingDefaultRadius = false;
				RecalculateBounds();
			}
		}
		internal float ScaledRadius => scaledRadius;
		public Vector2 Offset
		{
			get => offset;
			set
			{
				offset = value;
				RecalculateBounds();
			}
		}
		internal override Vector2 Position => base.Position + adjustedOffset;

		public CircleCollider()
		{
			radius = 1;
			offset = Vector2.Zero;
			usingDefaultRadius = true;
		}

		internal override void AssignGameObject(GameObject gameObject)
		{
			base.AssignGameObject(gameObject);
			if (usingDefaultRadius)
			{
				SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
				if (sr != null && sr.Sprite != null && sr.Sprite.Texture != null)
				{
					radius = Mathf.Max(sr.Sprite.Width / 100f, sr.Sprite.Height / 100f);
					RecalculateBounds();
				}
			}
		}

		protected override void DrawCollisionDebug()
		{
			Gizmos.DrawWireCircle(Position, ScaledRadius, 1, CollisionColour);
		}

		protected override void RecalculateBounds()
		{
			scaledRadius = radius * Mathf.Max(Transform.Scale.X, Transform.Scale.Y);
			adjustedOffset = offset.Transform(Transform.Rotation);
		}
	}
}