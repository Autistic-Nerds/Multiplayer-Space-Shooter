
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2Xna = Microsoft.Xna.Framework.Vector2;

namespace CosmosEngine.Rendering
{
	public static partial class Draw
	{
		private static WorldSpace renderSpace;
		private static Colour colour;
		/// <summary>
		/// 
		/// </summary>
		public static SpriteBatch SpriteBatch => CoreModule.Core.SpriteBatch;
		/// <summary>
		/// 
		/// </summary>
		public static Colour Colour { get => colour; set => colour = value; }
		/// <summary>
		/// 
		/// </summary>
		public static WorldSpace Space { get => renderSpace; set => renderSpace = value; }
		private static int Scale => Space switch
		{
			WorldSpace.World => 1,
			WorldSpace.Screen => 100,
			_ => 1,
		};

		private static Vector2 Position(Vector2 position)
		{
			if (Scale == 1)
				return position;
			return position / Scale;
		}

		#region Box
		public static void Box(Vector2 position, Vector2 size, short sortingOrder) => Box(position, size, Colour, sortingOrder);

		public static void Box(Vector2 position, Vector2 size, Colour colour, short sortingOrder) => Box(position, size, 0.0f, new Vector2(0.5f, 0.5f), colour, sortingOrder);

		public static void Box(Vector2 position, Vector2 size, float rotation, Vector2 origin, Colour colour, short sortingOrder)
		{
			Vector2 textureSize = (new Vector2(DefaultGeometry.Square.Width, DefaultGeometry.Square.Height) * new Vector2(size.X, size.Y)) / (100 * Scale);
			Sprite(DefaultGeometry.Square, position, rotation, textureSize, null, origin, colour, sortingOrder);
		}

		public static void WireBox(Vector2 position, Vector2 size, short sortingOrder) => WireBox(position, size, Colour, sortingOrder);

		public static void WireBox(Vector2 position, Vector2 size, Colour colour, short sortingOrder) => WireBox(position, size, 2, colour, sortingOrder);
		public static void WireBox(Vector2 position, Vector2 size, int width, Colour colour, short sortingOrder) => WireBox(position, size, width, 0f, new Vector2(0.5f, 0.5f), colour, sortingOrder);
		public static void WireBox(Vector2 position, Vector2 size, int width, float rotation, Vector2 origin, Colour colour, short sortingOrder)
		{
			float edgeMultiplier = (Mathf.Max(1, width) / 100f);
			float boxWidth = size.X - edgeMultiplier;
			float boxHeight = size.Y - edgeMultiplier;
			float originX = position.X - boxWidth * origin.X;
			float originY = position.Y - boxHeight * origin.Y;
			Vector2[] corners = new Vector2[]
			{
				new Vector2(originX, originY),
				new Vector2(originX + boxWidth, originY),
				new Vector2(originX + boxWidth, originY + boxHeight),
				new Vector2(originX, originY + boxHeight),
			};

			for(int i = 0; i < corners.Length;  i++)
			{
				int next = (i == corners.Length - 1 ? 0 : (i + 1));
				Vector2 pointA = corners[i];
				Vector2 pointB = corners[next];
				Vector2 direction = (pointB - pointA).Normalized * (width > 1 ? (width / 200f) : 0);
				Line(pointA, pointB + direction, colour, width, sortingOrder);
			}
		}

		#endregion

		#region Circle
		/// <summary>
		/// Render a Circle at <paramref name="position"/> with a given <paramref name="radius"/> using <see cref="ALP.Graphics.Colour"/> and an origin of (0.5, 0.5).
		/// </summary>
		/// <param name="position">Circle position.</param>
		/// <param name="radius">Circle radius in pixels.</param>
		/// <param name="sortingOrder">The render sorting order on a range [<see langword="-32768"/>, <see langword="32767"/>], with most positive values being rendered on top.</param>
		public static void Circle(Vector2 position, float radius, short sortingOrder = 0) => Circle(position, radius, Colour, sortingOrder);

		/// <summary>
		/// Render a Circle at <paramref name="position"/> with a given <paramref name="radius"/>, <paramref name="colour"/> and an origin of (0.5, 0.5).
		/// </summary>
		/// <param name="position">Circle position.</param>
		/// <param name="radius">Circle radius in pixels.</param>
		/// <param name="colour">The colour of the circle.</param>
		/// <param name="sortingOrder">The render sorting order on a range [<see langword="-32768"/>, <see langword="32767"/>], with most positive values being rendered on top.</param>
		public static void Circle(Vector2 position, float radius, Colour colour, short sortingOrder) => Circle(position, radius, new Vector2(0.5f, 0.5f), colour, sortingOrder);

		/// <summary>
		/// Render a Circle at <paramref name="position"/> with a given <paramref name="radius"/> and <paramref name="colour"/>.
		/// </summary>
		/// <param name="position">Circle position.</param>
		/// <param name="radius">Circle radius in pixels.</param>
		/// <param name="origin">The origin determines where the circle originates from. An origin of (0.5, 0.5) would originate from the centre of the <paramref name="position"/> and out. While an origin of (0, 0) is top left corner.</param>
		/// <param name="colour">The colours of the circle.</param>
		/// <param name="sortingOrder">The render sorting order on a range [<see langword="-32768"/>, <see langword="32767"/>], with most positive values being rendered on top.</param>
		public static void Circle(Vector2 position, float radius, Vector2 origin, Colour colour, short sortingOrder) => Circle(position, radius, 0.0f, origin, colour, sortingOrder);

		/// <summary>
		/// Render a Circle at <paramref name="position"/> rotated by <paramref name="rotation"/>, with a given <paramref name="radius"/> and <paramref name="colour"/>.
		/// </summary>
		/// <param name="position">Circle position.</param>
		/// <param name="radius">Circle radius in pixels.</param>
		/// <param name="rotation">Circle rotation around its origin in degrees.</param>
		/// <param name="origin">The origin determines where the circle originates from. An origin of (0.5, 0.5) would originate from the centre of the <paramref name="position"/> and out. While an origin of (0, 0) is top left corner.</param>
		/// <param name="colour">The colours of the circle.</param>
		/// <param name="sortingOrder">The render sorting order on a range [<see langword="-32768"/>, <see langword="32767"/>], with most positive values being rendered on top.</param>
		public static void Circle(Vector2 position, float radius, float rotation, Vector2 origin, Colour colour, short sortingOrder)
		{
			Vector2 textureSize = (new Vector2(DefaultGeometry.Circle.Width, DefaultGeometry.Circle.Height) * (radius)) / 100;
			Sprite(DefaultGeometry.Circle, position, rotation, textureSize, null, origin, colour, sortingOrder);
		}

		public static void WireCircle(Vector2 position, float radius, short sortingOrder = 0) => WireCircle(position, radius, Colour, sortingOrder);
		public static void WireCircle(Vector2 position, float radius, Colour colour, short sortingOrder) => WireCircle(position, radius, 2, colour, sortingOrder);
		public static void WireCircle(Vector2 position, float radius, int width, Colour colour, short sortingOrder) => WireCircle(position, radius, new Vector2(0.5f, 0.5f), width, colour, sortingOrder);
		public static void WireCircle(Vector2 position, float radius, Vector2 origin, int width, Colour colour, short sortingOrder)
		{
			int segments = (int)((360f / width) * radius);
			Vector2[] points = Mathf.GenerateEllipsePoints(segments, radius);
			for (int i = 0; i < points.Length; i++)
			{
				Sprite(DefaultGeometry.Pixel, points[i] + position, -i * (360f / segments), new Vector2(width, width), null, origin, colour, sortingOrder);
			}
		}

		#endregion

		#region Line
		/// <summary>
		/// Render a line between <paramref name="start"/> and <paramref name="end"/> using <see cref="ALP.Graphics.Colour"/>.
		/// </summary>
		/// <param name="start">The start position of the line.</param>
		/// <param name="end">The end position of the line.</param>
		/// <param name="sortingOrder">The render sorting order on a range [<see langword="-32768"/>, <see langword="32767"/>], with most positive values being rendered on top.</param>
		public static void Line(Vector2 start, Vector2 end, short sortingOrder = 0) => Line(start, end, Colour, sortingOrder);

		/// <summary>
		/// Render a line between <paramref name="start"/> and <paramref name="end"/> with a given <paramref name="colour"/>.
		/// </summary>
		/// <param name="start">The start position of the line.</param>
		/// <param name="end">The end position of the line.</param>
		/// <param name="colour">The colour of the line.</param>
		/// <param name="sortingOrder">The render sorting order on a range [<see langword="-32768"/>, <see langword="32767"/>], with most positive values being rendered on top.</param>
		public static void Line(Vector2 start, Vector2 end, Colour colour, short sortingOrder) => Line(start, end, colour, 2, sortingOrder);

		/// <summary>
		/// Render a line between <paramref name="start"/> and <paramref name="end"/> with a given <paramref name="width"/> and <paramref name="colour"/>.
		/// </summary>
		/// <param name="start">The start position of the line.</param>
		/// <param name="end">The end position of the line.</param>
		/// <param name="colour">The colour of the line.</param>
		/// <param name="width">The thickness of the line.</param>
		/// <param name="sortingOrder">The render sorting order on a range [<see langword="-32768"/>, <see langword="32767"/>], with most positive values being rendered on top.</param>
		public static void Line(Vector2 start, Vector2 end, Colour colour, int width, short sortingOrder)
		{
			float rotation = Mathf.Atan2(end.Y - start.Y, end.X - start.X);
			Vector2 scale = new Vector2(Vector2.Distance(start, end) * (100 / Scale), width);
			Vector2 origin = new Vector2(0f, DefaultGeometry.Pixel.Height / 2f);

			Sprite(DefaultGeometry.Pixel, start, rotation * Mathf.Rad2Deg, scale, null, origin, colour, sortingOrder);
		}

		#endregion

		#region Ray

		public static void Ray(Vector2 origin, Vector2 direction, short sortingOrder) => Ray(origin, direction, Colour, sortingOrder);

		public static void Ray(Vector2 origin, Vector2 direction, Colour colour, short sortingOrder) => Ray(origin, direction, colour, 2, sortingOrder);

		public static void Ray(Vector2 origin, Vector2 direction, Colour colour, int width, short sortingOrder)
		{
			Line(origin, origin + direction, colour, width, sortingOrder);
		}

		#endregion

		#region Sprite

		public static void Sprite(Sprite sprite, Vector2 position, short sortingOrder = 0) => Sprite(sprite, position, Colour, sortingOrder);

		public static void Sprite(Sprite sprite, Vector2 position, Colour colour, short sortingOrder) => Sprite(sprite, position, 0.0f, Vector2.One, null, new Vector2(0.5f, 0.5f), colour, sortingOrder);

		public static void Sprite(Sprite sprite, Transform transform, Colour colour, short sortingOrder) => Sprite(sprite, transform.Position, transform.Rotation, transform.Scale, null, new Vector2(0.5f, 0.5f), colour, sortingOrder);

		public static void Sprite(Sprite sprite, Vector2 position, float rotation, Vector2 scale, Rect? sourceRectangle, Vector2 origin, Colour colour, short sortingOrder)
		{
			if (sprite == null)
				return;
			if (sprite.Texture == null)
				return;

			Vector2 textureOrigin = new Vector2(sprite.Width * origin.X, sprite.Height * origin.Y);
			Vector2Xna point = Position(position).ToXna();

			//point.Round();
			SpriteBatch.Draw(sprite.Texture, point, sourceRectangle, colour, rotation * Mathf.Deg2Rad, textureOrigin, scale * (sprite.PixelsPerUnit / 100), SpriteEffects.None, LayerDepth(sortingOrder));
		}

		#endregion

		#region Text

		public static void Text(string text, Font font, int fontSize, Vector2 position) => Text(text, font, fontSize, position, Colour);
		public static void Text(string text, Font font, int fontSize, Vector2 position, Colour colour) => Text(text, font, fontSize, position, colour, short.MaxValue);
		public static void Text(string text, Font font, int fontSize, Vector2 position, Colour colour, short sortingOrder) => Text(text, font, fontSize, position, Vector2.Zero, colour, sortingOrder);
		public static void Text(string text, Font font, int fontSize, Vector2 position, Vector2 origin, Colour colour, short sortingOrder) => Text(text, font, fontSize, position, origin, 0f, colour, sortingOrder);
		public static void Text(string text, Font font, int fontSize, Vector2 position, Vector2 origin, float rotation, Colour colour, short sortingOrder)
		{
			Vector2 measure = font.MeasureString(text) * origin;
			Vector2Xna point = Position(position).ToXna() - measure;
			point.Round();
			SpriteBatch.DrawString(font[fontSize], text, point, colour, rotation * Mathf.Deg2Rad, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth(sortingOrder));
		}

		#endregion

		/// <summary>
		/// Converts a <paramref name="sortingOrder"/> value into a layer depth value used by the sprite batch.
		/// </summary>
		/// <param name="sortingOrder">Render sorting order is ranged from <see langword="-32768"/> to <see langword="32767"/> with positive values rendered on top.</param>
		/// <returns></returns>
		public static float LayerDepth(short sortingOrder)
		{
			return (float)(sortingOrder + short.MaxValue + 1) / (float)ushort.MaxValue;
		}

		internal static Rectangle ConvertSpriteToRect(Sprite sprite, Vector2 position, Vector2 scale)
		{
			Vector2Int pixelPosition = (Vector2Int)(position * 100);
			int width = Mathf.RoundToInt(sprite.Size.X * scale.X);
			int height = Mathf.RoundToInt(sprite.Size.Y * scale.Y);
			int x = pixelPosition.X - width / 2;
			int y = pixelPosition.Y - height / 2;
			return new Rectangle(x, y, width, height);
		}

	}
}