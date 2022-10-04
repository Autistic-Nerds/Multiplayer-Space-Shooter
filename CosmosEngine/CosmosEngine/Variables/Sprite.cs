
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CosmosEngine.CoreModule;

namespace CosmosEngine
{
	public class Sprite : Resource
	{
		private Vector2 pivot;
		private Vector2Int origin;
		private readonly Texture2D texture;
		private readonly Vector2 spriteSize;
		private readonly SpriteMode spriteMode;
		private readonly WrapMode wrapMode;
		private readonly FilterMode filterMode;
		private int pixelsPerUnit;

		private static ContentManager ContentManager => Core.Instance.Content;

		/// <summary>
		/// Get the reference to the used texture.
		/// </summary>
		public Texture2D Texture => texture;
		public Vector2 Pivot 
		{ 
			get => pivot;
			set
			{
				pivot = value;
				origin = new Vector2Int(Mathf.RoundToInt(Width * pivot.X), Mathf.RoundToInt(Height * pivot.Y));
			}
		}
		public Vector2 Origin => (Vector2)origin;
		/// <summary>
		/// Sprite texture import mode.
		/// </summary>
		public SpriteMode SpriteMode => spriteMode;
		/// <summary>
		/// Texture coordinate wrapping mode.
		/// </summary>
		public WrapMode WrapMode => wrapMode;
		/// <summary>
		/// Filtering mode of the Texture.
		/// </summary>
		public FilterMode FilterMode => filterMode;
		/// <summary>
		/// The number of pixels in the sprite that correspond to one unit in world space.
		/// </summary>
		public int PixelsPerUnit { get => pixelsPerUnit; set => pixelsPerUnit = value; }
		public Vector2Int Size => (Vector2Int)spriteSize;
		public int Width => Size.X;
		public int Height => Size.Y;

		/// <summary>
		/// Generate an empty sprite.
		/// </summary>
		public Sprite() : this(string.Empty, SpriteMode.Single, WrapMode.Clamped, FilterMode.Linear, 100)
		{
		}

		/// <summary>
		/// Generate a sprite, loading the texture at the given path.
		/// </summary>
		public Sprite(string path) : this(path, SpriteMode.Single, WrapMode.Clamped, FilterMode.Linear, 100)
		{
		}

		/// <summary>
		/// Generate a sprite with a given texture.
		/// </summary>
		public Sprite(Texture2D texture) : this(texture, SpriteMode.Single, WrapMode.Clamped, FilterMode.Linear, 100)
		{
		}

		/// <summary>
		/// Generate a sprite, loading the texture at the given path.
		/// </summary>
		public Sprite(string path, SpriteMode spriteMode = SpriteMode.Single, WrapMode wrapMode = WrapMode.Clamped, FilterMode filterMode = FilterMode.Linear, int pixelsPerUnit = 100)
		{
			if (!string.IsNullOrEmpty(path))
			{
				Texture2D texture = ContentManager.Load<Texture2D>(path);
				if (texture != null)
				{
					this.texture = texture;
					this.spriteSize = new Vector2(texture.Width, texture.Height);
					this.Pivot = new Vector2(0.5f, 0.5f);
				}
			}
			this.spriteMode = spriteMode;
			this.wrapMode = wrapMode;
			this.filterMode = filterMode;
			this.pixelsPerUnit = pixelsPerUnit;
		}

		/// <summary>
		/// Generate a sprite with a given texture.
		/// </summary>
		public Sprite(Texture2D texture, SpriteMode spriteMode = SpriteMode.Single, WrapMode wrapMode = WrapMode.Clamped, FilterMode filterMode = FilterMode.Linear, int pixelsPerUnit = 100)
		{
			this.texture = texture;
			this.spriteSize = new Vector2(texture.Width, texture.Height);
			this.Pivot = new Vector2(0.5f, 0.5f);
			this.spriteMode = spriteMode;
			this.wrapMode = wrapMode;
			this.filterMode = filterMode;
			this.pixelsPerUnit = pixelsPerUnit;
		}

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed && disposing)
			{
				texture.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
