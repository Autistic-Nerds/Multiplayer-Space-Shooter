
using CosmosEngine.Converter;
using System;
using System.ComponentModel;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace CosmosEngine
{
	[Serializable]
	[TypeConverter(typeof(Vector2Converter))]
	/// <summary>
	/// <see cref="CosmosEngine.Vector2"/> are scaled in unit space, unlike <see cref="Microsoft.Xna.Framework.Vector2"/> which uses pixel space.
	/// </summary>
	public struct Vector2
	{
		#region Static Fields
		private static readonly Vector2 zero = new Vector2(0.0f, 0.0f);
		private static readonly Vector2 up = new Vector2(0.0f, -1.0f);
		private static readonly Vector2 down = new Vector2(0.0f, 1.0f);
		private static readonly Vector2 left = new Vector2(-1.0f, 0.0f);
		private static readonly Vector2 right = new Vector2(1.0f, 0.0f);
		private static readonly Vector2 one = new Vector2(1.0f, 1.0f);
		private static readonly Vector2 minusOne = new Vector2(-1.0f, -1.0f);
		private static readonly Vector2 half = new Vector2(0.5f, 0.5f);
		/// <summary>
		/// Shorthand Vector2(0, 0).
		/// </summary>
		public static Vector2 Zero => zero;
		/// <summary>
		/// Shorthand Vector2(0, -1).
		/// </summary>
		public static Vector2 Up => up;
		/// <summary>
		/// Shorthand Vector2(0, 1).
		/// </summary>
		public static Vector2 Down => down;
		/// <summary>
		/// Shorthand Vector2(-1, 0).
		/// </summary>
		public static Vector2 Left => left;
		/// <summary>
		/// Shorthand Vector2(1, 0).
		/// </summary>
		public static Vector2 Right => right;
		/// <summary>
		/// Shorthand Vector2(1, 1).
		/// </summary>
		public static Vector2 One => one;
		/// <summary>
		/// Shorthand Vector2(-1, -1).
		/// </summary>
		public static Vector2 MinusOne => minusOne;
		/// <summary>
		/// Shorthand Vector2(0.5f, 0.5f).
		/// </summary>
		public static Vector2 Half => half;
		#endregion

		#region Fields
		private float x;
		private float y;

		/// <summary>
		/// X component of the vector.
		/// </summary>
		public float X { get => x; set => x = value; }
		/// <summary>
		/// Y component of the vector.
		/// </summary>
		public float Y { get => y; set => y = value; }
		/// <summary>
		/// Returns the length of this vector.
		/// </summary>
		public float Magnitude => Mathf.Sqrt(SqrMagnitude);
		/// <summary>
		/// Returns the squared length of this vector.
		/// </summary>
		public float SqrMagnitude => (x * x + y * y);
		/// <summary>
		/// Returns this vector as a unit vector (magnitude of 1).
		/// </summary>
		public Vector2 Normalized => Magnitude > Mathf.kEpsilon ? (new Vector2(x, y) / Magnitude) : Vector2.Zero;
		/// <summary>
		/// Access the x or y component using [0] or [1] respectively.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public float this[int index]
		{
			get
			{
				return index switch
				{
					0 => X,
					1 => Y,
					_ => throw new IndexOutOfRangeException(),
				};
			}
			set
			{
				_ = index switch
				{
					0 => X = value,
					1 => Y = value,
					_ => throw new IndexOutOfRangeException(),
				};
			}
		}
		public int ID { get; set; }

		#endregion

		#region Constructors

		public Vector2(float x, float y) : this()
		{
			this.x = x;
			this.y = y;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Returns true if the given vector is exactly equal to this vector. Due to floating point inaccuracies, this might return false for vectors which are essentially (but not exactly) equal. Use the == or != operator to test two vectors for approximate equality.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(!(obj is Vector2))
				return false;
			Vector2 other = (Vector2)obj;
			return this.X.Equals(other.X) && this.Y.Equals(other.Y);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}

		/// <summary>
		/// Makes this vector into a unit vector (magnitude of 1).
		/// </summary>
		public void Normalize()
		{
			float magnitude = this.Magnitude;
			if ((double)magnitude > Mathf.kEpsilon)
				this = this / magnitude;
			else
				this = Vector2.Zero;
		}

		/// <summary>
		/// Set <paramref name="x"/> and <paramref name="y"/> components of the <see cref="CosmosEngine.Vector2"/>.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Set(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Rounds the x and y components of the <see cref="CosmosEngine.Vector2"/> to nearest <see langword="int"/>.
		/// </summary>
		/// <returns></returns>
		public Vector2 Round()
		{
			this.x = Mathf.RoundToInt(this.x);
			this.y = Mathf.RoundToInt(this.y);
			return this;
		}

		/// <summary>
		/// Converts the One.Vector2 to Microsoft.Xna.Framework.Vector2, the vector is scaled to pixel space.
		/// </summary>
		/// <returns></returns>
		public Microsoft.Xna.Framework.Vector2 ToXna()
		{
			return new Microsoft.Xna.Framework.Vector2(X * 100, Y * 100);
		}

		/// <summary>
		/// Creates a new <see cref="CosmosEngine.Vector2"/> that contains a transformation of 2d-vector by the specified <see cref="Microsoft.Xna.Framework.Matrix"/>.
		/// </summary>
		/// <param name="matrix">The transformation.</param>
		/// <returns></returns>
		public Vector2 Transform(Matrix matrix) => Vector2.Transform(this, matrix);
		/// <summary>
		/// Creates a new <see cref="CosmosEngine.Vector2"/> that is rotated by the <paramref name="angle"/>.
		/// </summary>
		/// <param name="angle">Angle in degrees.</param>
		/// <returns></returns>
		public Vector2 Transform(float angle) => Vector2.Transform(new Vector2(X, Y), Matrix.CreateRotationZ(angle * Mathf.Deg2Rad));

		#region Math Operations

		/// <summary>
		/// Returns an absolute of this vector.
		/// </summary>
		/// <returns></returns>
		public Vector2 Abs()
		{
			this.x = Mathf.Abs(this.x);
			this.y = Mathf.Abs(this.y);
			return this;
		}

		/// <summary>
		/// Returns an inverse of this vector.
		/// </summary>
		/// <returns></returns>
		public Vector2 Invert()
		{
			this.x *= -1;
			this.y *= -1;
			return this;
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		public Vector2 Add(Vector2 value)
		{
			this.x += value.x;
			this.y += value.y;
			return this;
		}
		/// <summary>
		/// Adds <paramref name="value"/> to the <see cref="CosmosEngine.Vector2"/> x and y component.
		/// </summary>
		public Vector2 Add(float value)
		{
			this.x += value;
			this.y += value;
			return this;
		}

		/// <summary>
		/// Subtracts two vectors from eachother.
		/// </summary>
		public Vector2 Subtract(Vector2 value)
		{
			this.x -= value.x;
			this.y -= value.y;
			return this;
		}
		/// <summary>
		/// Subtracts <paramref name="value"/> from the <see cref="CosmosEngine.Vector2"/> x and y component.
		/// </summary>
		public Vector2 Subtract(float value)
		{
			this.x -= value;
			this.y -= value;
			return this;
		}

		/// <summary>
		/// Multiplies two vectors together.
		/// </summary>
		public Vector2 Multiply(Vector2 value)
		{
			this.x *= value.x;
			this.y *= value.y;
			return this;
		}
		/// <summary>
		/// Multiplies the <see cref="CosmosEngine.Vector2"/> x and y component with <paramref name="value"/>.
		/// </summary>
		public Vector2 Multiply(float value)
		{
			this.x *= value;
			this.y *= value;
			return this;
		}
				
		/// <summary>
		/// Divides two vectors from eachother.
		/// </summary>
		public Vector2 Divide(Vector2 value)
		{
			this.x /= value.x;
			this.y /= value.y;
			return this;
		}
		/// <summary>
		/// Divides the <see cref="CosmosEngine.Vector2"/> x and y component with <paramref name="value"/>.
		/// </summary>
		public Vector2 Divide(float value)
		{
			this.x /= value;
			this.y /= value;
			return this;
		}

		#endregion

		public override string ToString() => $"({X.ToString("F2", Mathf.NumberFormat)}, {Y.ToString("F2", Mathf.NumberFormat)})";
		#endregion

		#region Static Methods
		/// <summary>
		/// Converts a Microsoft.Xna.Framework.Vector2 to <see cref="CosmosEngine.Vector2"/>, the vector is scaled to unit space.
		/// </summary>
		/// <returns></returns>
		internal static Vector2 FromXna(Microsoft.Xna.Framework.Vector2 value)
		{
			return new Vector2(value.X / 100, value.Y / 100);
		}

		/// <summary>
		/// Returns the distance between a and b. [same as (a-b).Magnitude]
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float Distance(Vector2 a, Vector2 b)
		{
			return (a - b).Magnitude;
		}

		/// <summary>
		/// Returns the squared distance between a and b. [same as (a-b).SqrMagnitude]
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DistanceSqrt(Vector2 a, Vector2 b)
		{
			return (a - b).SqrMagnitude;
		}

		public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
		{
			Vector2 a = target - current;
			float magnitude = a.Magnitude;
			if (magnitude <= maxDistanceDelta || magnitude == 0f)
			{
				return target;
			}
			return current + a / magnitude * maxDistanceDelta;
		}

		/// <summary>
		/// Linearly interpolates between vectors a and b by t.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t">The parameter t is clamped to the range [0, 1].</param>
		/// <returns></returns>
		public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
		{
			return LerpUnclamped(a, b, Mathf.Clamp01(t));
		}

		/// <summary>
		/// Linearly interpolates between vectors a and b by t.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t">The parameter t is unclamped and can be outside the range [0, 1].</param>
		/// <returns></returns>
		public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
		{
			return b * t + a * (1.0f - t);
		}

		/// <summary>
		/// Returns the unsigned angle in degrees between from and to.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static float Angle(Vector2 from, Vector2 to)
		{
			return Mathf.Abs(SignedAngle(from, to));
		}

		/// <summary>
		/// Returns the signed angle in degrees between from and to.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static float SignedAngle(Vector2 from, Vector2 to)
		{
			float sin = from.X * to.Y - to.X * from.Y;
			float cos = from.X * to.X + from.Y * to.Y;
			return (float)Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Returns the Dot Product of two vectors.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static float Dot(Vector2 lhs, Vector2 rhs)
		{
			return Mathf.Cos(Angle(lhs, rhs));
		}

		/// <summary>
		/// Returns the 2D vector perpendicular to this 2D vector. The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.
		/// </summary>
		/// <param name="inDirection">The input direction.</param>
		/// <returns>The perpendicular direction.</returns>
		public static Vector2 Perpendicular(Vector2 inDirection)
		{
			return Vector2.Transform(inDirection, -90);
		}

		/// <summary>
		/// Returns a vector that is made from the smallest components of two vectors.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Vector2 Min(Vector2 lhs, Vector2 rhs)
		{
			return new Vector2(Mathf.Min(lhs.X, rhs.X), Mathf.Min(lhs.Y, rhs.Y));
		}

		/// <summary>
		/// Returns a vector that is made from the largest components of two vectors.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Vector2 Max(Vector2 lhs, Vector2 rhs)
		{
			return new Vector2(Mathf.Max(lhs.X, rhs.X), Mathf.Max(lhs.Y, rhs.Y));
		}

		/// <summary>
		/// Returns a vector2 that is rounded to nearest integer.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Vector2 Round(Vector2 value)
		{
			return new Vector2(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
		}

		/// <summary>
		/// Returns an absolute vector. [same as using +Vector2]
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Vector2 Abs(Vector2 value)
		{
			return +value;
		}

		/// <summary>
		/// Returns an inverted vector. [same as using -<see cref="CosmosEngine.Vector2"/>]
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Vector2 Invert(Vector2 value)
		{
			return -value;
		}

		/// <summary>
		/// Creates a new <see cref="CosmosEngine.Vector2"/> that contains a transformation of 2d-vector by the specified <see cref="Microsoft.Xna.Framework.Matrix"/>.
		/// </summary>
		/// <param name="value">Source <see cref="T:Cosmos.Vector2" />.</param>
		/// <param name="matrix">The transformation.</param>
		/// <returns></returns>
		public static Vector2 Transform(Vector2 value, Matrix matrix)
		{
			float x = (float)((double)value.X * (double)matrix.M11 + (double)value.Y * (double)matrix.M21) + matrix.M41;
			float y = (float)((double)value.X * (double)matrix.M12 + (double)value.Y * (double)matrix.M22) + matrix.M42;
			Vector2 result = Vector2.Zero;
			result.X = x / 100;
			result.Y = y / 100;
			return result;
		}
		/// <summary>
		/// Creates a new <see cref="CosmosEngine.Vector2"/> that is rotated by the <paramref name="angle"/>.
		/// </summary>
		/// <param name="angle">Angles in degress.</param>
		/// <returns></returns>
		public static Vector2 Transform(Vector2 value, float angle) => Vector2.Transform(new Vector2(value.X, value.Y), Matrix.CreateRotationZ(angle * Mathf.Deg2Rad));
		#endregion

		#region Operators
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Abs()"/>
		/// </summary>
		public static Vector2 operator +(Vector2 value) => value.Abs();
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Invert()()"/>
		/// </summary>
		public static Vector2 operator -(Vector2 value) => value.Invert();
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Add(float)"/>
		/// </summary>
		public static Vector2 operator +(Vector2 a, float d) => a.Add(d);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Subtract(float)"/>
		/// </summary>
		public static Vector2 operator -(Vector2 a, float d) => a.Subtract(d);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Multiply(float)"/>
		/// </summary>
		public static Vector2 operator *(Vector2 a, float d) => a.Multiply(d);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Divide(float)"/>
		/// </summary>
		public static Vector2 operator /(Vector2 a, float d) => a.Divide(d);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Add(float)"/>
		/// </summary>
		public static Vector2 operator +(float d, Vector2 a) => a.Add(d);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Multiply(float)"/>
		/// </summary>
		public static Vector2 operator *(float d, Vector2 a) => a.Multiply(d);

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Add(Vector2)"/>
		/// </summary>
		public static Vector2 operator +(Vector2 a, Vector2 b) => a.Add(b);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Subtract(Vector2)"/>
		/// </summary>
		public static Vector2 operator -(Vector2 a, Vector2 b) => a.Subtract(b);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Multiply(Vector2)"/>
		/// </summary>
		public static Vector2 operator *(Vector2 a, Vector2 b) => a.Multiply(b);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Vector2.Divide(Vector2)"/>
		/// </summary>
		public static Vector2 operator /(Vector2 a, Vector2 b) => a.Divide(b);
		/// <summary>
		/// Returns <see langword="true"/> if two vectors are approximately equal. To allow for floating point inaccuracies, the two vectors are considered equal if the magnitude of their difference is less than 1E-05f.
		/// </summary>
		/// <param name="lhs">Left hand side</param>
		/// <param name="rhs">Right hand side</param>
		public static bool operator ==(Vector2 lhs, Vector2 rhs)
		{
			return (lhs - rhs).SqrMagnitude <= Mathf.kEpsilon;
		}
		/// <summary>
		/// Returns <see langword="false"/> if two vectors are approximately equal. To allow for floating point inaccuracies, the two vectors are considered equal if the magnitude of their difference is less than 1E-05f..
		/// </summary>
		/// <param name="lhs">Left hand side</param>
		/// <param name="rhs">Right hand side</param>
		public static bool operator !=(Vector2 lhs, Vector2 rhs)
		{
			return !(lhs == rhs);
		}

		/// <summary>
		/// Convert One.Vector2 to Microsoft.Xna.Framework.Vector2, value is unscaled. Use Vector2.ToXna() to transform into scaled Vector2.
		/// </summary>
		public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2 value) => new Microsoft.Xna.Framework.Vector2(value.X, value.Y);
		/// <summary>
		/// Convert Microsoft.Xna.Framework.Vector2 to One.Vector2, value is unscaled. Use Vector2.FromXna() to transform into scaled Vector2.
		/// </summary>
		public static explicit operator Vector2(Microsoft.Xna.Framework.Vector2 value) => new Vector2(value.X, value.Y);
		#endregion
	}
}
