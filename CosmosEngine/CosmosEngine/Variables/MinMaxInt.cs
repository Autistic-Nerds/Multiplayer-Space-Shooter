
namespace CosmosEngine
{
	[System.Serializable]
	public struct MinMaxInt
	{
		private int min;
		private int max;

		public int Min { get => min; set => min = value; }
		public int Max { get => max; set => max = value; }

		public MinMaxInt(int min, int max)
		{
			this.min = min;
			this.max = max;
			if (min >= max)
			{
				Debug.Log($"{GetType().FullName}, min value ({min}) cannot be greater or equals to max value ({max}) - max will be increased to {min + 1}.", LogFormat.Warning);
				this.max = min + 1;
			}
		}

		public int RandomInRange()
		{
			return Random.Range(min, Max);
		}
	}
}
