
using CosmosEngine.Collection;
using CosmosEngine.CoreModule;
using CosmosEngine.Rendering;

namespace CosmosEngine
{
	public class TrailRenderer : RenderComponent
	{
		private readonly DirtyList<TrailSegment> trailSegments = new DirtyList<TrailSegment>();
		private Vector2 previousPosition;
		private float minDistance = 0.05f;
		private float widthMultiplier = 0.1f;

		private float Width => widthMultiplier * 100;

		protected override void Start()
		{
			previousPosition = Transform.Position;
		}

		protected override void Update()
		{
			Debug.QuickLog("Trail Length: " + trailSegments.Count);
			float distance = Vector2.Distance(Transform.Position, previousPosition);
			if (distance >= minDistance)
			{
				Vector2 direction = (Transform.Position - previousPosition).Normalized * minDistance;
				do
				{
					previousPosition += direction;
					AddSegment(previousPosition);
					distance -= minDistance;
				} while (distance > minDistance);
				//AddSegment(Transform.Position);
				previousPosition = Transform.Position;
			}

			for (int i = 0; i < trailSegments.Count; i++)
			{
				TrailSegment segment = trailSegments[i];
				segment.Delta -= Time.DeltaTime;
				trailSegments[i] = segment;

				if(segment.Delta <= 0.0f)
				{
					trailSegments.IsDirty = true;
				}
			}
			if(trailSegments.IsDirty)
			{
				trailSegments.RemoveAll(item => item.Delta <= 0.0f);
				trailSegments.IsDirty = false;
			}
		}

		private void AddSegment(Vector2 point)
		{
			trailSegments.Add(new TrailSegment(point, Transform.Rotation, 5.0f));
		}

		public override void Render()
		{
			foreach(TrailSegment segment in trailSegments)
			{
				Colour colour = Colour.Lerp(Colour.White, Colour.TransparentWhite, 1f - Mathf.Clamp01(segment.Delta, 0.0f, 5.0f));
				Draw.SpriteBatch.Draw(
					DefaultGeometry.Pixel.Texture, 
					segment.Position.ToXna(), 
					null, 
					colour, 
					segment.Rotation * Mathf.Deg2Rad, 
					new Vector2(0.5f, 0.5f), 
					Vector2.One * Width, 
					Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 
					Draw.LayerDepth(SortingValue));
			}
		}

		private struct TrailSegment
		{
			private readonly Vector2 position;
			private readonly float rotation;
			private float delta;

			public Vector2 Position => position;
			public float Rotation => rotation;
			public float Delta { get => delta; set => delta = value; }

			public TrailSegment(Vector2 position, float rotation, float delta)
			{
				this.position = position;
				this.rotation = rotation;
				this.delta = delta;
			}

		}

	}
}