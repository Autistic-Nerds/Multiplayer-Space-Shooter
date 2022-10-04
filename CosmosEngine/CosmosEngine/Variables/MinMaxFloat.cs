
namespace CosmosEngine
{
	[System.Serializable]
	public struct MinMaxFloat
	{
		private float min;
		private float max;

		public float Min { get => min; set => min = value; }
		public float Max { get => max; set => max = value; }

		public MinMaxFloat(float min, float max)
		{
			this.min = min;
			this.max = max;
			if (min >= max)
			{
				Debug.Log($"{GetType().FullName}, min value ({min}) cannot be greater or equals to max value ({max}) - max will be increased to {min + 1}.", LogFormat.Warning);
				this.max = min + 1;
			}
		}

		public float RandomInRange()
		{
			return Random.Range(min, Max);
		}
	}
}