
using CosmosEngine.CoreModule;
using CosmosEngine.Rendering;

namespace CosmosEngine
{
	public class SpriteRenderer : RenderComponent
	{
		[KeepReference]
		private Sprite sprite;

		public Sprite Sprite 
		{
			get => sprite;
			set
			{
				sprite = value;
				sourceRect = new Rect(Offset, sprite.Size);
			}
		}

		public SpriteRenderer()
		{
			sprite = DefaultGeometry.Square;
		}

		public SpriteRenderer(Sprite sprite)
		{
			this.Sprite = sprite;
		}

		public override void Render()
		{
			if (sprite == null)
				return;
			Draw.Sprite(Sprite, Transform.Position, Transform.Rotation, Transform.Scale, sourceRect, Vector2.Half, Colour, SortingValue);
		}

		public override string ToString()
		{
			return base.ToString() + $" {(Sprite == null ? "null" : Sprite.Texture.Name)}";
		}
	}
}