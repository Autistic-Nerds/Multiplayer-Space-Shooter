using Microsoft.Xna.Framework.Graphics;

namespace Cosmos.Entity
{
	public class SpriteRenderer : EntityComponent
	{
		public Texture2D sprite;

		public SpriteRenderer()
		{

		}

		public SpriteRenderer(Texture2D sprite)
		{
			this.sprite = sprite;
		}
	}
}