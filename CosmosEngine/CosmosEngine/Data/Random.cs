
namespace CosmosEngine
{
	public static class Random
	{
		private static System.Random random;
		private static int seed;

		private static System.Random Rnd => random;

		static Random()
		{
			InitSeed();
		}

		/// <summary>
		/// Seed used to initialize the random number generator.
		/// </summary>
		public static int Seed
		{
			get => seed;
			set
			{
				seed = value;
				random = new System.Random(seed);
			}
		}

		/// <summary>
		/// Returns a random float within [0.0, 1.0] (range is inclusive)
		/// </summary>
		public static float Value => (float)(Rnd.Next(0, 101) / 100f);
		/// <summary>
		/// Returns a random signed value, either 1 or -1.
		/// </summary>
		public static int Sign => (int)(Value < 0.5f ? 1 : -1);

		public static void InitSeed() => InitSeed((int)System.DateTime.Now.Ticks);
		public static void InitSeed(int seed)
		{
			Seed = seed;
		}

		/// <summary>
		/// Returns a random point inside or on a circle with radius 1.0.
		/// </summary>
		/// <returns></returns>
		public static Vector2 InsideUnitCircle()
		{
			Vector2 point;
			float x;
			float y;
			do
			{
				x = Value * 2.0f - 1.0f;
				y = Value * 2.0f - 1.0f;
				point = new Vector2(x, y);
			} while (x == 0 || y == 0);
			return point;
		}

		/// <summary>
		/// Returns a random point on the edge of a circle with radius 1.0.
		/// </summary>
		/// <returns></returns>
		public static Vector2 OnUnitCircle()
		{
			Vector2 point;
			float x;
			float y;
			do
			{
				x = Value * 2.0f - 1.0f;
				y = Value * 2.0f - 1.0f;
				point = new Vector2(x, y);
			} while (x == 0 || y == 0);
			return point.Normalized;
		}

		/// <summary>
		/// Returns a random float within [min, max] (range is inclusive).
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static float Range(float min, float max)
		{
			if (min > max)
			{
				Debug.Log($"Random: min value ({min:F2}) cannot be greater than max value ({max:F2})", LogFormat.Error);
				return 0;
			}
			if (max < min)
			{
				Debug.Log($"Random: max value ({max:F2}) cannot be less than min value ({min:F2})", LogFormat.Error);
				return 0;
			}
			return Rnd.Next((int)(min * 100f), (int)(max * 100f) + 1) / 100f;
		}

		/// <summary>
		/// Returns a random int within [min, max] (max is exclusive)
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Range(int min, int max)
		{
			if(min > max)
			{
				Debug.Log($"Random: min value ({min}) cannot be greater than max value ({max})", LogFormat.Error);
				return 0;
			}
			if(max < min)
			{
				Debug.Log($"Random: max value ({max}) cannot be less than min value ({min})", LogFormat.Error);
				return 0;
			}
			return Rnd.Next(min, max);
		}

		/// <summary>
		/// Returns true if Random.Value is above the given percentage must be within [0.0, 1.0]
		/// </summary>
		/// <param name="percent">The percentage in a range of [0.0, 1.0].</param>
		/// <param name="inclusive">If inclusive the value itself will include in the check.</param>
		/// <returns></returns>
		public static bool AbovePercentage(float percent, bool inclusive = false)
		{
			return (inclusive ? Value >= Mathf.Clamp01(percent) : Value > Mathf.Clamp01(percent));
		}

		/// <summary>
		/// Returns <see langword="true"/> if <see cref="CosmosEngine.Random.Value"/> is below the given percentage, within range of [0.0, 1.0].
		/// </summary>
		/// <param name="percent">The percentage in a range of [0.0, 1.0].</param>
		/// <param name="inclusive">If inclusive the value itself will include in the check.</param>
		/// <returns></returns>
		public static bool BelowPercentage(float percent, bool inclusive = false)
		{
			return (inclusive ? Value <= Mathf.Clamp01(percent) : Value < Mathf.Clamp01(percent));
		}
	}
}
