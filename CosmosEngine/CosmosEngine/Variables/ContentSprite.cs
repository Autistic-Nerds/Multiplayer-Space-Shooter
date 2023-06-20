using Microsoft.Xna.Framework.Graphics;
using System;

namespace CosmosEngine.Variables
{
	public class ContentSprite
	{
		private Texture2D mainTexture;
		private string path;

		public Texture2D Texture => mainTexture ??= Load();

		public ContentSprite(string path)
		{
			this.path = path;
		}

		public Texture2D Load() => Texture2D.FromFile(CoreModule.Core.GraphicsDeviceManager.GraphicsDevice, $"{AppDomain.CurrentDomain.BaseDirectory}/Content/{path}.png");

		public static explicit operator Sprite(ContentSprite sprite) => new Sprite(sprite.Texture);
	}
}