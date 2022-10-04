
namespace CosmosEngine
{
	[System.Obsolete("The Vector2Int is deprecated and is missing a lot of functionality, it can still be used, but should be updated asap.", false)]
	public struct Vector2Int
	{
		#region Static Properties
		/// <summary>
		/// Shorthand Vector2(0, 1).
		/// </summary>
		public static Vector2Int Up = new Vector2Int(0, 1);
		/// <summary>
		/// Shorthand Vector2(0, -1).
		/// </summary>
		public static Vector2Int Down = new Vector2Int(0, -1);
		/// <summary>
		/// Shorthand Vector2(-1, 0).
		/// </summary>
		public static Vector2Int Left = new Vector2Int(-1, 0);
		/// <summary>
		/// Shorthand Vector2(1, 0).
		/// </summary>
		public static Vector2Int Right = new Vector2Int(1, 0);
		/// <summary>
		/// Shorthand Vector2(1, 1).
		/// </summary>
		public static Vector2Int One = new Vector2Int(1, 1);
		/// <summary>
		/// Shorthand Vector2(0, 0).
		/// </summary>
		public static Vector2Int Zero = new Vector2Int(0, 0);
		#endregion

		#region Fields
		private int x;
		private int y;

		/// <summary>
		/// X component of the vector.
		/// </summary>
		public int X { get => x; set => x = value; }
		/// <summary>
		/// 	Y component of the vector.
		/// </summary>
		public int Y { get => y; set => y = value; }
		/// <summary>
		/// Returns the length of this vector.
		/// </summary>
		public float Magnitude => Mathf.Sqrt(x * x + y * y);
		/// <summary>
		/// Returns the squared length of this vector.
		/// </summary>
		public float SqrMagnitude => (x * x + y * y);
		/// <summary>
		/// Returns this vector as a unit vector (magnitude of 1).
		/// </summary>
		public Vector2 Normalized => (new Vector2(x, y) / Magnitude);
		/// <summary>
		/// Access the x or y component using [0] or [1] respectively.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public float this[int index] => index == 0 ? X : Y;

		#endregion

		public Vector2Int(int x, int y) : this()
		{
			this.x = x;
			this.y = y;
		}

		#region Public Methods

		public override bool Equals(object obj)
		{
			if(obj is Vector2Int)
			{
				return this == (Vector2Int)obj;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return System.HashCode.Combine(X, Y);
		}

		/// <summary>
		/// Set x and y components of an existing Vector2.
		/// </summary>
		/// <param name="newX"></param>
		/// <param name="newY"></param>
		public void Set(int newX, int newY)
		{
			this.x = newX;
			this.y = newY;
		}

		/// <summary>
		/// Converts the Vector2 to Microsoft.Xna.Framework.Vector2 used in Core for the XNA framework.
		/// </summary>
		/// <returns></returns>
		internal Microsoft.Xna.Framework.Vector2 ToXna()
		{
			return new Microsoft.Xna.Framework.Vector2(X, Y);
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Returns the distance between a and b.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float Distance(Vector2Int a, Vector2Int b)
		{
			return (a - b).Magnitude;
		}

		/// <summary>
		/// Returns the squared distance between a and b.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DistanceSqrt(Vector2Int a, Vector2Int b)
		{
			return (a - b).SqrMagnitude;
		}

		/// <summary>
		/// Returns the unsigned angle in degrees between from and to.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static float Angle(Vector2Int from, Vector2Int to)
		{
			return Mathf.Abs(SignedAngle(from, to));
		}

		/// <summary>
		/// Returns the signed angle in degrees between from and to.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static float SignedAngle(Vector2Int from, Vector2Int to)
		{
			double sin = from.X * to.Y - to.X * from.Y;
			double cos = from.X * to.X + from.Y * to.Y;
			return (float)System.Math.Atan2(sin, cos) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Returns the Dot Product of two vectors.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static float Dot(Vector2Int lhs, Vector2Int rhs)
		{
			return Mathf.Cos(Angle(lhs, rhs));
		}

		public override string ToString()
		{
			return $"(X: {X:F2} : Y: {Y:F2})";
		}
		#endregion

		#region Operators
		/// <summary>
		/// Adds a number to a vector.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator +(Vector2Int a, int d)
		{
			return new Vector2Int(a.x + d, a.y + d);
		}
		/// <summary>
		/// Subtracts a vector with a number.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator -(Vector2Int a, int d)
		{
			return new Vector2Int(a.x - d, a.y - d);
		}
		/// <summary>
		/// Multiplies a vector by a number.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator *(Vector2Int a, int d)
		{
			return new Vector2Int(a.x * d, a.y * d);
		}
		/// <summary>
		/// Divides a vector by a number.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator /(Vector2Int a, int d)
		{
			return new Vector2Int(a.x / d, a.y / d);
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator +(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x + b.x, a.y + b.y);
		}
		/// <summary>
		/// Subtracts two vectors from eachother.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator -(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x - b.x, a.y - b.y);
		}
		/// <summary>
		/// Multiplies two vectors together.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator *(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x * b.x, a.y * b.y);
		}
		/// <summary>
		/// Divides two vectors from eachother.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2Int operator /(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x / b.x, a.y / b.y);
		}
		/// <summary>
		/// Returns true if two vectors are approximately equal. To allow for floating point inaccuracies, the two vectors are considered equal if the magnitude of their difference is less than 1e-5.
		/// </summary>
		/// <param name="lhs">Left hand side</param>
		/// <param name="rhs">Right hand side</param>
		/// <returns></returns>
		public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		/// <summary>
		/// Returns false if two vectors are approximately equal. To allow for floating point inaccuracies, the two vectors are considered equal if the magnitude of their difference is less than 1e-5.
		/// </summary>
		/// <param name="lhs">Left hand side</param>
		/// <param name="rhs">Right hand side</param>
		/// <returns></returns>
		public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
		{
			return !(lhs == rhs);
		}

		public static implicit operator Vector2(Vector2Int value) => new Vector2(value.x, value.y);

		public static explicit operator Vector2Int(Vector2 value) => new Vector2Int((int)value.X, (int)value.Y);

		#endregion
	}
}
