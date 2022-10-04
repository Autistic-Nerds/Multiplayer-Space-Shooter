
namespace CosmosEngine.UI
{
	public struct Margin
	{
		private int top, bottom, left, right;
		private int spacing;

		public int Top { get => top; set => top = value; }
		public int Bottom { get => bottom; set => bottom = value; }
		public int Left { get => left; set => left = value; }
		public int Right { get => right; set => right = value; }
		public int Width => Right + Left;
		public int Height => Top + Bottom;
		/// <summary>
		/// Vertical spacing between lines.
		/// </summary>
		public int Spacing { get => spacing; set => spacing = value; }

		/// <summary>
		/// Returns a new <see cref="ALP.UI.Margin"/> with an area of 8 in all directions and a vertical spacing of 2.
		/// </summary>
		public static Margin Default => new Margin(8, 8, 8, 8, 2);
		/// <summary>
		/// Returns a new <see cref="ALP.UI.Margin"/> with an area of 0 in all directions.
		/// </summary>
		public static Margin Zero => new Margin(0);

		/// <summary>
		/// Create a new <see cref="ALP.UI.Margin"/> with uniform area.
		/// </summary>
		/// <param name="margin"></param>
		public Margin(int margin)
		{
			this.top = margin;
			this.bottom = margin;
			this.left = margin;
			this.right = margin;
			this.spacing = 0;
		}

		/// <summary>
		/// Create a new <see cref="ALP.UI.Margin"/> with specific <paramref name="top"/>, <paramref name="bottom"/>, <paramref name="left"/> and <paramref name="right"/>.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public Margin(int top, int bottom, int left, int right)
		{
			this.top = top;
			this.bottom = bottom;
			this.left = left;
			this.right = right;
			this.spacing = 0;
		}

		public Margin(int top, int bottom, int left, int right, int spacing)
		{
			this.top = top;
			this.bottom = bottom;
			this.left = left;
			this.right = right;
			this.spacing = spacing;
		}

		public override string ToString()
		{
			return $"[{top} : {bottom} : {left} : {right}]";
		}

		public static Vector2 operator +(Margin margin, Vector2 vector) => new Vector2(vector.X + margin.Left + margin.right, vector.Y + margin.Top + margin.bottom);
		public static Vector2 operator +(Vector2 vector, Margin margin) => new Vector2(vector.X + margin.Left + margin.right, vector.Y + margin.Top + margin.bottom);
		public static Vector2 operator -(Vector2 vector, Margin margin) => new Vector2(vector.X - (margin.Left + margin.right), vector.Y - (margin.Top + margin.bottom));
	}
}
