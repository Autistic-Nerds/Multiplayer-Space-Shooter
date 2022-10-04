
using System;
using System.Diagnostics.CodeAnalysis;

namespace CosmosEngine
{
	/// <summary>
	/// A 2D Rectangle defined by X and Y position, width and height.
	/// </summary>
	public struct Rect : IEquatable<Rect>
	{
		#region Static Fields
		private static readonly Rect zero = new Rect(0, 0, 0, 0);
		/// <summary>
		/// Shorthand for Rect(0,0,0,0).
		/// </summary>
		public static Rect Zero => zero;
		#endregion

		#region Fields
		private float x;
		private float y;
		private float width;
		private float height;

		/// <summary>
		/// The X coordinate of the rectangle.
		/// </summary>
		public float X { get => x; set => x = value; }
		/// <summary>
		/// The Y coordinate of the rectangle.
		/// </summary>
		public float Y { get => y; set => y = value; }
		/// <summary>
		/// The width of the rectangle.
		/// </summary>
		public float Width { get => width; set => width = value; }
		/// <summary>
		/// The height of the rectangle.
		/// </summary>
		public float Height { get => height; set => height = value; }
		/// <summary>
		/// The position of the center of the rectangle.
		/// </summary>
		public Vector2 Center => new Vector2(x + width / 2, y + height / 2);
		/// <summary>
		/// The X and Y position of the rectangle.
		/// </summary>
		public Vector2 Position
		{
			get => new Vector2(X, Y);
			set
			{
				this.x = value.X;
				this.y = value.Y;
			}
		}
		/// <summary>
		/// The width and height of the rectangle.
		/// </summary>
		public Vector2 Size
		{
			get => new Vector2(Width, Height);
			set
			{
				this.width = value.X;
				this.height = value.Y;
			}
		}
		/// <summary>
		/// The minimum X coordinate of the rectangle.
		/// </summary>
		public float xMin => x;
		/// <summary>
		/// The maximum X coordinate of the rectangle.
		/// </summary>
		public float xMax => x + width;
		/// <summary>
		/// The minimum Y coordinate of the rectangle.
		/// </summary>
		public float yMin => y;
		/// <summary>
		/// The maximum Y coordinate of the rectangle.
		/// </summary>
		public float yMax => y + height;

		#endregion

		#region Constructor

		public Rect(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public Rect(Vector2 position, Vector2 size)
		{
			this.x = position.X;
			this.y = position.Y;
			this.width = size.X;
			this.height = size.Y;
		}

		public Rect(Rect source)
		{
			this.x = source.x;
			this.y = source.y;
			this.width = source.width;
			this.height = source.height;
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Set the components of the <see cref="CosmosEngine.Rect"/>.
		/// </summary>
		public void Set(Vector2 position, Vector2 size) => Set(position.X, position.Y, size.X, size.Y);

		/// <summary>
		/// Set the components of the <see cref="CosmosEngine.Rect"/>.
		/// </summary>
		public void Set(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		/// <summary>
		/// Round the components to nearest integral value of the <see cref="CosmosEngine.Rect"/>.
		/// </summary>
		/// <returns></returns>
		public Rect Round()
		{
			this.x = Mathf.RoundToInt(this.x);
			this.y = Mathf.RoundToInt(this.y);
			this.width = Mathf.RoundToInt(this.width);
			this.height = Mathf.RoundToInt(this.height);
			return this;
		}

		/// <summary>
		/// Returns <see langword="true"/> if the x and y of <paramref name="point"/> is a point inside the <see cref="CosmosEngine.Rect"/>.
		/// </summary>
		public bool Contains(Vector2 point) => Contains(point.X, point.Y);

		public bool Contains(float x, float y)
		{
			return (x >= xMin) && (x < xMax) && (y >= yMin) && (y < yMax);
		}

		/// <summary>
		/// Returns <see langword="true"/> if the <paramref name="other"/> rectangle intersects with the <see cref="CosmosEngine.Rect"/>.
		/// </summary>
		public bool Intersects(Rect other)
		{
			return PhysicsModule.PhysicsIntersection.BoxBox(
				this.X, this.Y, this.Width, this.Height,
				other.X, other.Y, other.Width, other.Height);
		}

		public Rect Add(Rect other)
		{
			this.x += other.x;
			this.y += other.y;
			this.width += other.width;
			this.height += other.height;
			return this;
		}

		public Rect Subtract(Rect other)
		{
			this.x -= other.x;
			this.y -= other.y;
			this.width -= other.width;
			this.height -= other.height;
			return this;
		}
		public Rect Mulitiply(float f)
		{
			this.x *= f;
			this.y *= f;
			this.width *= f;
			this.height *= f;
			return this;
		}
		public Rect Divide(float f)
		{
			this.x /= f;
			this.y /= f;
			this.width /= f;
			this.height /= f;
			return this;
		}

		public Microsoft.Xna.Framework.Rectangle ToXna() => (Microsoft.Xna.Framework.Rectangle)(this * 100);

		public bool Equals(Rect other)
		{
			return x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);
		}

		public override bool Equals([NotNullWhen(true)] object obj)
		{
			if (!(obj is Rect))
				return false;
			return Equals((Rect)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y, width, height);
		}

		public override string ToString()
		{
			return $"(x:{X.ToString("F2", Mathf.NumberFormat)}, y:{Y.ToString("F2", Mathf.NumberFormat)}, w:{Width.ToString("F2", Mathf.NumberFormat)}, h:{Height.ToString("F2", Mathf.NumberFormat)})";
		}

		#endregion

		#region Operators

		public static Rect operator +(Rect lhs, Rect rhs) => lhs.Add(rhs);
		public static Rect operator -(Rect lhs, Rect rhs) => lhs.Subtract(rhs);
		public static Rect operator *(Rect lhs, float rhs) => lhs.Mulitiply(rhs);
		public static Rect operator /(Rect lhs, float rhs) => lhs.Divide(rhs);

		public static bool operator ==(Rect lhs, Rect rhs) =>
			Mathf.Abs(lhs.X - rhs.X) < Mathf.kEpsilon &&
			Mathf.Abs(lhs.Y - rhs.Y) < Mathf.kEpsilon &&
			Mathf.Abs(lhs.Width - rhs.Width) < Mathf.kEpsilon &&
			Mathf.Abs(lhs.Height - rhs.Height) < Mathf.kEpsilon;

		public static bool operator !=(Rect lhs, Rect rhs) => !(lhs == rhs);

		public static implicit operator Microsoft.Xna.Framework.Rectangle(Rect rect) => new Microsoft.Xna.Framework.Rectangle((int)(rect.X), (int)(rect.Y), (int)(rect.Width), (int)(rect.Height));
		#endregion
	}
}