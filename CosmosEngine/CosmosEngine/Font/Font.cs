
using CosmosEngine.CoreModule;
using CosmosEngine.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace CosmosEngine
{
	public sealed class Font : Resource
	{
		#region Static Fonts
		private static Font arial;
		private static Font calibri;
		private static Font inter;
		private static Font montserrat;
		private static Font trebuchet;
		private static Font verdana;
		public static Font Arial => arial ??= new Font("Fonts/arial");
		public static Font Calibri => calibri ??= new Font("Fonts/calibri");
		public static Font Inter => inter ??= new Font("Fonts/inter");
		public static Font Montserrat => montserrat ??= new Font("Fonts/montserrat");
		public static Font TrebuchetMS => trebuchet ??= new Font("Fonts/trebuc");
		public static Font Verdana => verdana ??= new Font("Fonts/verdana");
		#endregion

		internal const int FontBitmapWidth = 1024;
		internal const int FontBitmapHeight = 1024;
		private readonly Dictionary<int, SpriteFont> regularFont;
		private string fontPath;
		private int lineSpacing;
		private float spacing;
		private int lastUsedFontSize;

		/// <summary>
		/// The line spacing, distance from baseline to baseline of the font.
		/// </summary>
		public int LineSpacing { get => lineSpacing; set => lineSpacing = value; }

		[System.Obsolete("Spacing between characters does not function on a global font.", false)]
		/// <summary>
		/// The spacing between each character in the font. (tracking)
		/// </summary>
		public float Spacing
		{
			get => spacing;
			set
			{
				spacing = value;
				foreach (var font in regularFont.Values)
				{
					font.Spacing = spacing;
				}
			}
		}

		public Font(string path) : this(path, 0, 0)
		{
		}

		public Font(string path, int lineSpacing, float spacing)
		{
			lastUsedFontSize = 12;
			regularFont = new Dictionary<int, SpriteFont>();
			if (path.ToLower().EndsWith(".ttf"))
			{
				fontPath = path;
			}
			else
			{
				fontPath = path + ".ttf";
			}
			this.lineSpacing = lineSpacing;
			this.spacing = spacing;
		}

		public SpriteFont this[int size] => Get(size);

		public SpriteFont Get(int size)
		{
			if (regularFont == null)
				throw new System.NullReferenceException();
			if (size < 8)
				size = 8;
			if (size > byte.MaxValue)
				size = byte.MaxValue;

			lastUsedFontSize = size;
			SpriteFont font;
			if (regularFont.ContainsKey(size))
			{
				font = regularFont[size];
			}
			else
			{
				int fontSize = Mathf.RoundToInt((float)size * 1.667f);
				//Font size does not exist, we have to create a new size.
				TtfFont fontBakeResult;
				using (var stream = File.OpenRead("Assets/" + fontPath))
				{
					fontBakeResult = FontFactory.Bake(stream,
						fontSize,
						FontBitmapWidth,
						FontBitmapHeight,
						new[]
						{
							CharacterRange.BasicLatin,
							CharacterRange.Latin1Supplement,
							CharacterRange.LatinExtendedA,
							CharacterRange.Cyrillic,
						}
					);
					font = fontBakeResult.CreateSpriteFont(CoreModule.Core.Instance.GraphicsDevice);
				}
				regularFont.Add(size, font);
			}
			return font;
		}

		/// <summary>
		/// Returns the vertical height of this font when rendered at the last used Font Size.
		/// </summary>
		/// <returns></returns>
		public float FontHeight() => (Get(lastUsedFontSize).LineSpacing + LineSpacing);
		/// <summary>
		/// Returns the vertical height of this font when rendered at a given <paramref name="size"/>.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public float FontHeight(int size) => (Get(size).LineSpacing + LineSpacing);
		/// <summary>
		/// Returns the size of the contents of the <paramref name="text"/>, as a <see cref="CosmosEngine.Vector2"/>, when rendered in this font at the last used Font Size.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string text) => MeasureString(text, lastUsedFontSize);
		/// <summary>
		/// Returns the size of the contents of the <paramref name="stringBuilder"/>, as a <see cref="CosmosEngine.Vector2"/>, when rendered in this font at the last used Font Size.
		/// </summary>
		/// <param name="stringBuilder"></param>
		/// <returns></returns>
		public Vector2 MeasureString(System.Text.StringBuilder stringBuilder) => MeasureString(stringBuilder, lastUsedFontSize);
		/// <summary>
		/// Returns the size of the contents of the <paramref name="text"/>, as a <see cref="CosmosEngine.Vector2"/>, when rendered in this font at the given <paramref name="size"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string text, int size)
		{
			Vector2 measure = ((Vector2)Get(size).MeasureString(text));
			measure.Y += LineSpacing;
			return measure;
		}
		/// <summary>
		/// Returns the size of the contents of the <paramref name="stringBuilder"/>, as a <see cref="CosmosEngine.Vector2"/>, when rendered in this font at the given <paramref name="size"/>.
		/// </summary>
		/// <param name="stringBuilder"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public Vector2 MeasureString(System.Text.StringBuilder stringBuilder, int size)
		{
			Vector2 measure = ((Vector2)Get(size).MeasureString(stringBuilder));
			measure.Y += LineSpacing;
			return measure;
		}

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed && disposing)
			{
				regularFont.Clear();
			}
			base.Dispose(disposing);
		}
	}
}