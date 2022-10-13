
using System;
using System.Globalization;

namespace CosmosEngine
{
	/// <summary>
	/// Provides constants and static methods for trigonometric, logarithmic, common mathematical functions, linear interpolation, clamping and other useful functionality to use within the <see cref="CosmosEngine"/>.
	/// <para>It's an extension that is build on top of the <see cref="System.Math"/> library.</para>
	/// </summary>
	public static class Mathf
	{
		#region Fields
		public const float E = MathF.E;
		/// <summary>
		/// The ratio of the circumference of a circle to its diameter. PI = 3.14159265358979
		/// </summary>
		public const float PI = MathF.PI;
		public const float Infinity = float.MaxValue;
		/// <summary>
		/// Degrees-to-radians conversion constant.
		/// </summary>
		public const float Deg2Rad = (PI * 2) / 360;
		/// <summary>
		/// Radians-to-degrees conversion constant.
		/// </summary>
		public const float Rad2Deg = 360 / (PI * 2);
		public const float kEpsilon = 1E-05f;

		private static NumberFormatInfo numberFormat;
		public static NumberFormatInfo NumberFormat => numberFormat;

		#endregion

		static Mathf()
		{
			numberFormat = new NumberFormatInfo();
			numberFormat.NumberDecimalSeparator = ".";
		}

		#region Math
		/// <summary>
		/// <inheritdoc cref="System.MathF.Abs(float)"/>
		/// </summary>
		public static float Abs(float value)
		{
			return (float)MathF.Abs(value);
		}
		/// <summary>
		/// <inheritdoc cref="System.MathF.Abs(int)"/>
		/// </summary>
		public static int Abs(int value)
		{
			return (int)MathF.Abs(value);
		}
		/// <summary>
		/// <inheritdoc cref="System.MathF.Acos(float)"/>
		/// </summary>
		public static float Acos(float f) => MathF.Acos(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Asin(float)"/>
		/// </summary>
		public static float Asin(float f) => MathF.Asin(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Atan2(float, float)"/>
		/// </summary>
		public static float Atan2(float y, float x)
		{
			return (float)MathF.Atan2(y, x);
		}
		/// <summary>
		/// <inheritdoc cref="System.MathF.Cos(float)"/>
		/// </summary>
		public static float Cos(float f)
		{
			return (float)MathF.Cos(f);
		}
		/// <summary>
		/// <inheritdoc cref="System.MathF.Exp(float)"/>
		/// </summary>
		public static float Exp(float f) => MathF.Exp(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Log(float)"/>
		/// </summary>
		public static float Log(float f) => MathF.Log(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Log10(float)"/>
		/// </summary>
		public static float Log10(float f) => (float)MathF.Log10(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Pow(float, float)"/>
		/// </summary>
		public static float Pow(float f, float p) => (float)MathF.Pow(f, p);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Sign(float)"/>
		/// </summary>
		/// <returns>A number that indicates the sign of <paramref name="f"/>, as shown in the following table.
		/// <list type="table">
		/// <item>-1 <paramref name="f"/> is less than zero.</item>
		/// <item>0 <paramref name="f"/> is equal to zero.</item>
		/// <item>1 <paramref name="f"/> is greater than zero.</item>
		/// </list></returns>
		public static int Sign(float f) => (int)MathF.Sign(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Sin(float)"/>
		/// </summary>
		public static float Sin(float f) => (float)MathF.Sin(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Sqrt(float)"/>
		/// </summary>
		public static float Sqrt(float f) => (float)MathF.Sqrt(f);
		public static float Sum(params float[] values)
		{
			float sum = 0;
			for(int i = 0; i < values.Length; i++)
				sum += values[i];
			return sum;
		}
		/// <summary>
		/// <inheritdoc cref="System.MathF.Tan(float)"/>
		/// </summary>
		public static float Tan(float f) => (float)MathF.Tan(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Truncate(float)"/>
		/// </summary>
		public static float Truncate(float f) => (float)MathF.Truncate(f);

		#endregion

		public static Vector2[] GenerateEllipsePoints()
		{
			return GenerateEllipsePoints(360, 1, 1, Vector2.Zero);
		}

		public static Vector2[] GenerateEllipsePoints(int segments)
		{
			return GenerateEllipsePoints(segments, 1, 1, Vector2.Zero);
		}

		public static Vector2[] GenerateEllipsePoints(int segments, float radius)
		{
			return GenerateEllipsePoints(segments, radius, radius, Vector2.Zero);
		}

		/// <summary>
		/// Generates points around in an ellipse.
		/// </summary>
		/// <returns></returns>
		public static Vector2[] GenerateEllipsePoints(int segments, float xRadius, float yRadius, Vector2 position)
		{
			Vector2[] points = new Vector2[segments];
			float x;
			float y;

			float angle = 0f;

			for (int i = 0; i < points.Length; i++)
			{
				x = Mathf.Sin(Mathf.Deg2Rad * angle) * (xRadius / 2);
				y = Mathf.Cos(Mathf.Deg2Rad * angle) * (yRadius / 2);

				points[i] = new Vector2(x + position.X, y + position.Y);
				angle += (360f / segments);
			}
			return points;
		}

		#region Min / Max

		/// <summary>
		/// Returns largest value of <paramref name="a"/> or <paramref name="b"/>.
		/// </summary>
		public static float Max(float a, float b)
		{
			return a > b ? a : b;
		}
		/// <summary>
		/// <inheritdoc cref="Max(float, float)"/>
		/// </summary>
		public static int Max(int a, int b)
		{
			return a > b ? a : b;
		}
		/// <summary>
		/// Returns largest of two or more <paramref name="values"/>.
		/// </summary>
		public static float Max(params float[] values)
		{
			float max = float.MinValue;
			for (int i = 0; i < values.Length; i++)
				max = Max(max, values[i]);
			return max;
		}
		/// <summary>
		/// <inheritdoc cref="Max(float[])"/>
		/// </summary>
		public static int Max(params int[] values)
		{
			int max = int.MinValue;
			for (int i = 0; i < values.Length; i++)
				max = Max(max, values[i]);
			return max;
		}

		/// <summary>
		/// Returns smallest of value of <paramref name="a"/> and <paramref name="b"/>.
		/// </summary>
		public static float Min(float a, float b)
		{
			return a < b ? a : b;
		}
		/// <summary>
		/// <inheritdoc cref="Min(float, float)"/>
		/// </summary>
		public static int Min(int a, int b)
		{
			return a < b ? a : b;
		}
		/// <summary>
		/// Returns smallest of two or more <paramref name="values"/>.
		/// </summary>
		public static float Min(params float[] values)
		{
			float min = float.MaxValue;
			for (int i = 0; i < values.Length; i++)
				min = Min(min, values[i]);
			return min;
		}
		/// <summary>
		/// <inheritdoc cref="Min(float[])"/>
		/// </summary>
		public static int Min(params int[] values)
		{
			int min = int.MaxValue;
			for (int i = 0; i < values.Length; i++)
				min = Min(min, values[i]);
			return min;
		}
		#endregion

		#region Round
		/// <summary>
		/// <inheritdoc cref="System.MathF.Floor(float)"/>
		/// </summary>
		public static float Ceil(float f) => (float)Math.Floor(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Floor(float)"/>
		/// </summary>
		public static float Floor(float f) => (float)Math.Floor(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Round(float)"/>
		/// </summary>
		public static float Round(float f) => (float)MathF.Round(f);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Round(float, int)"/>
		/// </summary>
		public static float Round(float f, int digits) => (float)MathF.Round(f, digits);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Round(float, MidpointRounding)"/>
		/// </summary>
		public static float Round(float f, MidpointRounding mode) => (float)MathF.Round(f, mode);
		/// <summary>
		/// <inheritdoc cref="System.MathF.Round(float, int, MidpointRounding)"/>
		/// </summary>
		public static float Round(float f, int digits, MidpointRounding mode) => (float)MathF.Round(f, digits, mode);
		/// <summary>
		/// <inheritdoc cref="Mathf.Ceil(float)"/>
		/// </summary>
		public static int CeilToInt(float f) => (int)Math.Ceiling(f);
		/// <summary>
		/// <inheritdoc cref="Mathf.Floor(float)"/>
		/// </summary>
		public static int FloorToInt(float f) => (int)Math.Floor(f);
		/// <summary>
		/// <inheritdoc cref="Mathf.Round(float)(float)"/>
		/// </summary>
		public static int RoundToInt(float f) => (int)Math.Round(f);

		#endregion

		#region Clamp
		/// <summary>
		/// Clamps the given <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.
		/// </summary>
		/// <returns>Will return the <paramref name="value"/> if it's within <paramref name="min"/> and <paramref name="max"/>, else returns which is closest.</returns>
		public static float ClampBetween(float value, float min, float max)
		{
			return value < min ? min : value > max ? max : value;
		}

		/// <summary>
		/// Clamps the given <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.
		/// </summary>
		/// <returns>Will return the <paramref name="value"/> if it's within <paramref name="min"/> and <paramref name="max"/>, else returns which is closest.</returns>
		public static int ClampBetween(int value, int min, int max)
		{
			return value < min ? min : value > max ? max : value;
		}

		/// <summary>
		/// Clamps <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/> and returns the value on a scale from 0 to 1.
		/// </summary>
		/// <returns>The returend value can be above 1 or below 0.</returns>
		public static float Clamp(float value, float min, float max)
		{
			return (min + value) / max;
		}

		/// <summary>
		/// Clamps <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/> and returns the value on a scale from <paramref name="low"/> to <paramref name="high"/>.
		/// </summary>
		/// <returns>If value is less than <paramref name="min"/>, <paramref name="low"/> will be returned, if value is greater than <paramref name="max"/>, <paramref name="high"/> will be returned.</returns>
		public static float Clamp(float value, float min, float max, float low, float high)
		{
			if (value <= min)
				return low;
			if (value >= max)
				return high;
			return Lerp(low, high, Clamp(value, min, max));
		}

		/// <summary>
		/// Clamps <paramref name="value"/> between 0 and 1.
		/// </summary>
		/// <returns></returns>
		public static float Clamp01(float value)
		{
			return ClampBetween(value, 0.0f, 1.0f);
		}

		/// <summary>
		/// Clamps <paramref name="value"/> between 0 and 1.
		/// </summary>
		/// <returns></returns>
		public static int Clamp01(int value)
		{
			if (value < 0)
				return 0;
			else if (value > 1)
				return 1;
			else
				return value;
		}
		/// <summary>
		/// Clamps <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/> and returns the value on a scale from 0 to 1.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns>The returend value can be above 1 or below 0.</returns>
		public static float Clamp01(float value, float min, float max) => Clamp(value, min, max);
		#endregion

		#region Lerp
		/// <summary>
		/// Linearly interpolates between <paramref name="min"/> and <paramref name="max"/> by <paramref name="t"/>, while clamping the interpolant between 0 and 1.
		/// </summary>
		/// <param name="t">The parameter t is clamped to the range [0, 1].</param>
		public static float Lerp(float min, float max, float t) => LerpUnclamped(min, max, Clamp01(t));

		/// <summary>
		/// Linearly interpolates between <paramref name="min"/> and <paramref name="max"/> by <paramref name="t"/>, without clamping the interpolant.
		/// </summary>
		public static float LerpUnclamped(float min, float max, float t)
		{
			return min + (max - min) * t;
		}
		/// <summary>
		/// Linearly interpolates between <paramref name="min"/> and <paramref name="max"/> by <paramref name="t"/>, but interpolate correctly when they wrap around 360 degrees.
		/// </summary>
		public static float LerpAngle(float a, float b, float t)
		{
			float delta = Repeat((b - a), 360);
			if (delta > 180)
				delta -= 360;
			return a + delta * Clamp01(t);
		}
		/// <summary>
		/// Calculates the InverseLerp parameter between of two values.
		/// </summary>
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
				return Clamp01((value - a) / (b - a));
			else
				return 0.0f;
		}

		/// <summary>
		/// Moves a value <paramref name="current"/> towards <paramref name="target"/> by <paramref name="delta"/>.
		/// </summary>
		public static float MoveTowards(float current, float target, float delta)
		{
			if (Mathf.Abs(target - current) <= delta)
				return target;
			return current + Math.Sign(target - current) * delta;
		}

		[System.Obsolete("Unsure what the reason for this method is...", false)]
		public static bool MoveTowards(out float current, float target, float delta)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Moves a value <paramref name="current"/> towards <paramref name="target"/> by <paramref name="delta"/>, but interpolate correctly when they wrap around 360 degrees.
		/// </summary>
		public static float MoveTowardsAngle(float current, float target, float delta)
		{
			float deltaAngle = DeltaAngle(current, target);
			if (-delta < deltaAngle && deltaAngle < delta)
				return target;
			target = current + deltaAngle;
			return MoveTowards(current, target, delta);
		}

		/// <summary>
		/// Interpolates between <paramref name="from"/> and <paramref name="to"/> by <paramref name="t"/> with smoothing at the limits.
		/// </summary>
		public static float SmoothStep(float from, float to, float t)
		{
			t = Mathf.Clamp01(t);
			t = -2.0f * t * t * t + 3.0f * t * t;
			return to * t + from * (1.0f - t);
		}

		#endregion

		#region Mathf
		/// <summary>
		/// 
		/// </summary>
		public static float Bias(float x, float bias)
		{
			float p = Pow(1 - bias, 3);
			return (x * p) / (x * p - x + 1);
		}
		/// <summary>
		/// Calculates the shortest difference between two given angles.
		/// </summary>
		public static float DeltaAngle(float current, float target)
		{
			float delta = Mathf.Repeat((target - current), 360.0f);
			if (delta > 180.0f)
				delta -= 360.0f;
			return delta;
		}
		/// <summary>
		/// Loops the value <paramref name="t"/>, so that it is never larger than <paramref name="length"/> and never smaller than 0.
		/// </summary>
		public static float Repeat(float t, float length)
		{
			return ClampBetween(t - Mathf.Floor(t / length) * length, 0.0f, length);
		}
		#endregion
	}
}
