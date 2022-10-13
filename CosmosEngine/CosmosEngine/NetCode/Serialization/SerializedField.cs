namespace CosmosEngine.Netcode.Serialization
{
	public struct SerializedField
	{
		private string name;
		private string value;

		public string Name => name;
		public string Value => value;

		public SerializedField(string name, string value)
		{
			this.name = name.Trim('"');
			this.value = value;
		}

		public override string ToString()
		{
			return $"{name}:{value}";
		}
	}
}