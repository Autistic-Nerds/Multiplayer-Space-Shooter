namespace CosmosEngine
{
	public class BoxCollider : Collider
	{
		private Vector2 scaledSize;
		private Vector2 size;
		private Vector2 offset;
		private Vector2 adjustedOffset;
		private Vector2 topLeft;
		private Vector2 topRight;
		private Vector2 bottomLeft;
		private Vector2 bottomRight;

		internal override Vector2 Position => base.Position + adjustedOffset;
		public Vector2 Size
		{
			get => size;
			set
			{
				size = value;
				RecalculateBounds();
			}
		}
		public Vector2 Offset
		{
			get => offset;
			set
			{
				offset = value;
				RecalculateBounds();
			}
		}
		internal Vector2 ScaledSize => scaledSize;
		/// <summary>
		/// The top left corner X position.
		/// </summary>
		public float X => topLeft.X;
		/// <summary>
		/// The top left corner Y position.
		/// </summary>
		public float Y => topRight.Y;
		/// <summary>
		/// The width of the <see cref="CosmosEngine.BoxCollider"/> relative to the <see cref="CosmosEngine.Transform.Scale"/>.
		/// </summary>
		public float Width 
		{ 
			get => scaledSize.X;
			set
			{
				scaledSize.X = value;
				RecalculateBounds();
			}
		}
		/// <summary>
		/// The height of the <see cref="CosmosEngine.BoxCollider"/> relative to the <see cref="CosmosEngine.Transform.Scale"/>.
		/// </summary>
		public float Height 
		{ 
			get => scaledSize.Y;
			set
			{ 
				scaledSize.Y = value;
				RecalculateBounds();
			}
		}

		internal override void AssignGameObject(GameObject gameObject)
		{
			base.AssignGameObject(gameObject);
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
			if(sr != null && sr.Sprite != null && sr.Sprite.Texture != null)
			{
				Size = new Vector2(sr.Sprite.Width / 100f, sr.Sprite.Height / 100f);
				RecalculateBounds();
			}
		}

		protected override void RecalculateBounds()
		{
			scaledSize = size * Transform.Scale;
			adjustedOffset = offset.Transform(Transform.Rotation);
			topLeft = new Vector2(Position.X - Width / 2, Position.Y - Height / 2);
			topRight = new Vector2(Position.X + Width / 2, Position.Y - Height / 2);
			bottomLeft = new Vector2(Position.X - Width / 2, Position.Y + Height / 2);
			bottomRight = new Vector2(Position.X + Width / 2, Position.Y + Height / 2);
		}
		protected override void DrawCollisionDebug()
		{
			Gizmos.DrawLine(topLeft, topRight, CollisionColour);
			Gizmos.DrawLine(topLeft, bottomLeft, CollisionColour);
			Gizmos.DrawLine(bottomLeft, bottomRight, CollisionColour);
			Gizmos.DrawLine(bottomRight, topRight, CollisionColour);

		}
	}
}