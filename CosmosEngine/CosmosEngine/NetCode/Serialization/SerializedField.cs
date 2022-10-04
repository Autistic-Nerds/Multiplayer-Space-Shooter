namespace CosmosEngine.NetCode.Serialization
{
	public struct SerializedField
	{
		private string name;
		private object value;

		public string Name => name;
		public object Value => value;

		public SerializedField(string name, object value)
		{
			this.name = name;
			this.value = value;
		}

		public SerializedField(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		public override string ToString()
		{
			return $"{name}:{value} | {value.GetType()}";
		}
	}
}