
using System;
using System.Diagnostics.CodeAnalysis;
using Color = Microsoft.Xna.Framework.Color;
namespace CosmosEngine
{
	[System.Serializable]
	/// <summary>
	/// Representation of RGBA colours.
	/// </summary>
	public partial struct Colour : IEquatable<Colour>
	{
		#region Static Colours

		public static Colour Random => new Colour(
			CosmosEngine.Random.Range(0, 255),
			CosmosEngine.Random.Range(0, 255),
			CosmosEngine.Random.Range(0, 255));

		/// <summary>
		/// White (R: 255, G: 255, B: 255)
		/// </summary>
		public static Colour White { get; private set; } = new Colour(255, 255, 255);
		/// <summary>
		/// Black (R: 0, G: 0, B: 0)
		/// </summary>
		public static Colour Black { get; private set; } = new Colour(0, 0, 0);

		/// <summary>
		/// Silver (R: 192, G: 192, B: 192)
		/// </summary>
		public static Colour Silver { get; private set; } = new Colour(192, 192, 192);
		/// <summary>
		/// Light Grey (R: 169, G: 169, B: 169)
		/// </summary>
		public static Colour LightGrey { get; private set; } = new Colour(169, 169, 169);
		/// <summary>
		/// Grey (R: 128, G: 128, B: 128)
		/// </summary>
		public static Colour Grey { get; private set; } = new Colour(128, 128, 128);
		/// <summary>
		/// Dim Grey (R: 87, G: 87, B: 87)
		/// </summary>
		public static Colour DimGrey { get; private set; } = new Colour(87, 87, 87);
		/// <summary>
		/// Dark Grey (R: 64, G: 64, B: 64)
		/// </summary>
		public static Colour DarkGrey { get; private set; } = new Colour(64, 64, 64);
		/// <summary>
		/// Jet (R: 52, G: 52, B: 52)
		/// </summary>
		public static Colour Jet { get; private set; } = new Colour(52, 52, 52);
		/// <summary>
		/// Charcoal (R: 33, G: 33, B: 33)
		/// </summary>
		public static Colour Charcoal { get; private set; } = new Colour(33, 33, 33);

		/// <summary>
		/// Red (R: 255, G: 0, B: 0)
		/// </summary>
		public static Colour Red { get; private set; } = new Colour(255, 0, 0);
		/// <summary>
		/// Green (R: 0, G: 255, B: 0)
		/// </summary>
		public static Colour Green { get; private set; } = new Colour(0, 255, 0);
		/// <summary>
		/// Blue (R: 0, G: 0, B: 255)
		/// </summary>
		public static Colour Blue { get; private set; } = new Colour(0, 0, 255);

		/// <summary>
		/// Dark Red (R: 128, G: 0, B: 0)
		/// </summary>
		public static Colour DarkRed { get; private set; } = new Colour(128, 0, 0);
		/// <summary>
		/// Dark Green (R: 0, G: 128, B: 0)
		/// </summary>
		public static Colour DarkGreen { get; private set; } = new Colour(0, 128, 0);
		/// <summary>
		/// Dark Blue (R: 0, G: 0, B: 128)
		/// </summary>
		public static Colour DarkBlue { get; private set; } = new Colour(0, 0, 128);

		/// <summary>
		/// Magenta (R: 255, G: 0, B: 255)
		/// </summary>
		public static Colour Magenta { get; private set; } = new Colour(255, 0, 255);
		/// <summary>
		/// Cyan (R: 0, G: 255, B: 255)
		/// </summary>
		public static Colour Cyan { get; private set; } = new Colour(0, 255, 255);
		/// <summary>
		/// Yellow (R: 255, G: 255, B: 0)
		/// </summary>
		public static Colour Yellow { get; private set; } = new Colour(255, 255, 0);

		/// <summary>
		/// Orange (R: 255, G: 153, B: 0)
		/// </summary>
		public static Colour Orange { get; private set; } = new Colour(255, 153, 0);
		/// <summary>
		/// Skye Blue (R: 76, G: 178, B: 255)
		/// </summary>
		public static Colour SkyeBlue { get; private set; } = new Colour(76, 178, 255);
		/// <summary>
		/// Cornflower Blue (R: 100, G: 148, B: 234)
		/// </summary>
		public static Colour CornflowerBlue { get; private set; } = new Colour(100, 148, 234);

		/// <summary>
		/// Lime (R: 191, G: 255, B: 0)
		/// </summary>
		public static Colour Lime { get; private set; } = new Colour(191, 255, 0);
		/// <summary>
		/// Violet (R: 171, G: 0, B: 232)
		/// </summary>
		public static Colour Violet { get; private set; } = new Colour(171, 0, 232);
		/// <summary>
		/// Purple (R: 128, G: 0, B: 128)
		/// </summary>
		public static Colour Purple { get; private set; } = new Colour(128, 0, 128);
		/// <summary>
		/// Indigo (R: 75, G: 0, B: 130)
		/// </summary>
		public static Colour Indigo { get; private set; } = new Colour(75, 0, 130);

		/// <summary>
		/// Dark Slate Grey (R: 46, G: 76, B: 76)
		/// </summary>
		public static Colour DarkSlateGrey { get; private set; } = new Colour(46, 76, 76);
		/// <summary>
		/// Dark Slate Red (R: 138, G: 61, B: 138)
		/// </summary>
		public static Colour DarkSlateRed { get; private set; } = new Colour(138, 61, 138);
		/// <summary>
		/// Dark Slate Green (R: 71, G: 138, B: 61)
		/// </summary>
		public static Colour DarkSlateGreen { get; private set; } = new Colour(71, 138, 61);
		/// <summary>
		/// Dark Slate Blue (R: 71, G: 61, B: 138)
		/// </summary>
		public static Colour DarkSlateBlue { get; private set; } = new Colour(71, 61, 138);

		/// <summary>
		/// Desaturated Red (R: 59, G: 25, B: 40)
		/// </summary>
		public static Colour DesaturatedRed { get; private set; } = new Colour(59, 25, 40);
		/// <summary>
		/// Desaturated Green (R: 40, G: 59, B: 25)
		/// </summary>
		public static Colour DesaturatedGreen { get; private set; } = new Colour(40, 59, 25);
		/// <summary>
		/// Desaturated Blue (R: 25, G: 40, B: 59)
		/// </summary>
		public static Colour DesaturatedBlue { get; private set; } = new Colour(25, 40, 59);

		/// <summary>
		/// Desaturated Blue (R: 0, G: 0, B: 0, A: 0)
		/// </summary>
		public static Colour TransparentBlack { get; private set; } = new Colour(0f, 0f, 0f, 0f);
		/// <summary>
		/// Desaturated Blue (R: 255, G: 255, B: 255, A: 0)
		/// </summary>
		public static Colour TransparentWhite { get; private set; } = new Colour(1f, 1f, 1f, 0f);


		#endregion

		#region Fields
		private Color packedColour;
		private float r;
		private float g;
		private float b;
		private float a;
		/// <summary>
		/// Red component of the colour.
		/// </summary>
		public float R
		{
			get => r;
			set
			{
				r = Mathf.Clamp01(value);
				packedColour.R = (byte)(r * byte.MaxValue);
			}
		}
		/// <summary>
		/// Green component of the colour.
		/// </summary>
		public float G
		{
			get => g;
			set
			{
				g = Mathf.Clamp01(value);
				packedColour.G = (byte)(g * byte.MaxValue);
			}
		}
		/// <summary>
		/// Blue component of the colour.
		/// </summary>
		public float B
		{
			get => b;
			set
			{
				b = Mathf.Clamp01(value);
				packedColour.B = (byte)(b * byte.MaxValue);
			}
		}
		/// <summary>
		/// Alpha component of the colour (0 is transparent, 1 is opaque).
		/// </summary>
		public float A
		{
			get => a;
			set
			{
				a = Mathf.Clamp01(value);
				packedColour.A = (byte)(a * byte.MaxValue);
			}
		}
		#endregion

		#region Constructor
		public Colour(uint packedValue)
		{
			packedColour = new Color(packedValue);
			packedColour.Deconstruct(out float r, out float g, out float b, out float a);
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		/// <summary>
		/// Constructs a new <see cref="CosmosEngine.Colour"/> with given <paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/>, <paramref name="a"/> values.
		/// </summary>
		/// <param name="r">Red component.</param>
		/// <param name="g">Green component.</param>
		/// <param name="b">Blue component.</param>
		/// <param name="a">Alpha component.</param>
		public Colour(float r, float g, float b, float a)
		{
			this.r = Mathf.Clamp01(r);
			this.g = Mathf.Clamp01(g);
			this.b = Mathf.Clamp01(b);
			this.a = Mathf.Clamp01(a);
			packedColour = new Color(r, g, b, a);
		}

		/// <summary>
		/// Constructs a new <see cref="CosmosEngine.Colour"/> with given <paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/> values.
		/// </summary>
		/// <param name="r">Red component.</param>
		/// <param name="g">Green component.</param>
		/// <param name="b">Blue component.</param>
		public Colour(float r, float g, float b) : this(r, g, b, 1f) { }

		/// <summary>
		/// Constructs a new <see cref="CosmosEngine.Colour"/> with given <paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/> values.
		/// </summary>
		/// <param name="r">Red component.</param>
		/// <param name="g">Green component.</param>
		/// <param name="b">Blue component.</param>
		public Colour(int r, int g, int b) : this(r, g, b, 255) {	}

		/// <summary>
		/// Constructs a new <see cref="CosmosEngine.Colour"/> with given <paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/>, <paramref name="a"/> values.
		/// </summary>
		/// <param name="r">Red component.</param>
		/// <param name="g">Green component.</param>
		/// <param name="b">Blue component.</param>
		/// <param name="a">Alpha component.</param>
		public Colour(int r, int g, int b, int a) : this(Mathf.Clamp01(r, 0, byte.MaxValue), Mathf.Clamp01(g, 0, byte.MaxValue), Mathf.Clamp01(b, 0, byte.MaxValue), Mathf.Clamp01(a, 0, byte.MaxValue)) {  }

		#endregion

		#region Public Methods
		/// <summary>
		/// Sets the <paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/> and <paramref name="a"/> values of the colour.
		/// </summary>
		/// <param name="r">Red component.</param>
		/// <param name="g">Green component.</param>
		/// <param name="b">Blue component.</param>
		/// <param name="a">Alpha component.</param>
		public void Set(float r, float g, float b, float a = 1)
		{
			this.r = Mathf.Clamp01(r);
			this.g = Mathf.Clamp01(g);
			this.b = Mathf.Clamp01(b);
			this.a = Mathf.Clamp01(a);
			packedColour = new Color(r, g, b, a);
		}
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Colour.Set(float, float, float, float)"/>
		/// </summary>
		/// <param name="r">Red component.</param>
		/// <param name="g">Green component.</param>
		/// <param name="b">Blue component.</param>
		/// <param name="a">Alpha component.</param>
		public void Set(int r, int g, int b, int a = 255)
		{
			Set(Mathf.Clamp01(r, 0, byte.MaxValue), Mathf.Clamp01(g, 0, byte.MaxValue), Mathf.Clamp01(b, 0, byte.MaxValue), Mathf.Clamp01(a, 0, byte.MaxValue));
		}
		/// <summary>
		/// Deconstructs a colour into its base <paramref name="r"/>, <paramref name="g"/> and <paramref name="b"/> components.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public void Deconstruct(out float r, out float g, out float b) => Deconstruct(out r, out g, out b, out _);
		/// <summary>
		/// Deconstructs a colour into its base <paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/> and <paramref name="a"/> components.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public void Deconstruct(out float r, out float g, out float b, out float a)
		{
			r = R;
			g = G;
			b = B;
			a = A;
		}
		/// <summary>
		/// Deconstructs a colour into its base <paramref name="r"/>, <paramref name="g"/> and <paramref name="b"/> components.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public void Deconstruct(out int r, out int g, out int b) => Deconstruct(out r, out g, out b, out _);
		/// <summary>
		/// Deconstructs a colour into its base <paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/> and <paramref name="a"/> components.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public void Deconstruct(out int r, out int g, out int b, out int a)
		{
			r = (int)(R * byte.MaxValue);
			g = (int)(G * byte.MaxValue);
			b = (int)(B * byte.MaxValue);
			a = (int)(A * byte.MaxValue);
		}

		/// <summary>
		/// Access the R, G, B, A components using [0], [1], [2], [3] respectively.
		/// </summary>
		/// <param name="index">The index to access.</param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public float this[int index] => index switch
		{
			0 => R,
			1 => G,
			2 => B,
			3 => A,
			_ => throw new IndexOutOfRangeException($"Trying to access colour value index of {index} when it's only on a range from [0..3]"),
		};

		/// Returns all components in both colours are approximately equal. To allow for floating point inaccuracies, the two colours are considered equal if the all components difference is less than 1E-05f.
		public bool Equals(Colour other)
		{
			return Mathf.Abs(R - other.R) < 1E-05f &&
				Mathf.Abs(G - other.G) < 1E-05f &&
				Mathf.Abs(B - other.B) < 1E-05f &&
				Mathf.Abs(A - other.A) < 1E-05f;
		}

		public override bool Equals([NotNullWhen(true)] object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode() => HashCode.Combine(r, g, b, a);

		public override string ToString()
		{
			return $"({R}, {G}, {B}, {A})";
		}

		#endregion

		#region Static Methods
		/// <summary>
		/// Linearly interpolates between colours <paramref name="a"/> and <paramref name="b"/> by <paramref name="t"/>.
		/// </summary>
		/// <param name="a">Colour a.</param>
		/// <param name="b">Colour b.</param>
		/// <param name="t">Float for combining a and b, <paramref name="t"/> is clamped between 0 and 1. When <paramref name="t"/> is 0 returns <paramref name="a"/>. When <paramref name="t"/> is 1 returns <paramref name="b"/></param>
		/// <returns></returns>
		public static Colour Lerp(Colour a, Colour b, float t)
		{
			return new Colour(
				Mathf.Lerp(a.R, b.R, t),
				Mathf.Lerp(a.G, b.G, t),
				Mathf.Lerp(a.B, b.B, t),
				Mathf.Lerp(a.A, b.A, t));
		}

		/// <summary>
		/// Creates a new <see cref="CosmosEngine.Colour"/> from a hex string, if hex is incorrect a default white colour will be returned.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static Colour FromHex(string hex)
		{
			return new Colour(Convert.ToUInt32(hex, 16));
		}

		//public static string ToHex(Colour colour)
		//{

		//}

		#endregion

		#region Operators
		/// <summary>
		/// Subtracts colour b (<paramref name="rhs"/>) from colour a (<paramref name="lhs"/>). Each component is subtracted separately, alpha values are ignored.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Colour operator -(Colour lhs, Colour rhs) => new Colour(lhs.R - rhs.R, lhs.G - rhs.G, lhs.B - rhs.B);
		/// <summary>
		/// Adds two colours together. Each component is added separately, alpha values are ignored.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Colour operator +(Colour lhs, Colour rhs) => new Colour(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B);
		/// <summary>
		/// Multiplies two colours together. Each component is multiplied separately, alpha values are ignored.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Colour operator *(Colour lhs, Colour rhs) => new Colour(lhs.R * rhs.R, lhs.G * rhs.G, lhs.B * rhs.B);
		/// <summary>
		/// Divides colour a (<paramref name="lhs"/>) by the float b (<paramref name="rhs"/>). Each colour component is scaled separately, alpha values are ignored.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Colour operator /(Colour lhs, float rhs) => new Colour(lhs.R / rhs, lhs.G / rhs, lhs.B / rhs);
		/// <summary>
		/// Multiplies colour a (<paramref name="lhs"/>) with float b (<paramref name="rhs"/>). Each colour component is scaled separately, alpha values are ignored.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Colour operator *(Colour lhs, float rhs) => new Colour(lhs.R * rhs, lhs.G * rhs, lhs.B * rhs);
		public static bool operator ==(Colour lhs, Colour rhs) => lhs.Equals(rhs);
		public static bool operator !=(Colour lhs, Colour rhs) => !lhs.Equals(rhs);
		/// <summary>
		/// Implicit conversion of <see cref="CosmosEngine.Colour"/> to a <see cref="Microsoft.Xna.Framework.Color"/>, one is used by CosmosEngine the other is used by MonoGame.
		/// </summary>
		/// <param name="colour"></param>
		public static implicit operator Color(Colour colour) => colour.packedColour;
		#endregion
	}
}