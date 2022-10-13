namespace CosmosEngine.Netcode.Serialization
{
	public struct SerializedField
	{
		private byte index;
		private string value;

		public byte Index => index;
		public string Value => value;

		public SerializedField(byte index, string value)
		{
			this.index = index;
			this.value = value;
		}

		public override string ToString()
		{
			return $"{index}:{value}";
		}
	}
}