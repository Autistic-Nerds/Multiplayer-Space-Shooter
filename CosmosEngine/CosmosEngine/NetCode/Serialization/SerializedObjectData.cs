namespace CosmosEngine.Netcode.Serialization
{
	[System.Serializable]
	public struct SerializedObjectData
	{
		public uint BehaviourId { get; set; }
		public string Stream { get; set; }
	}
}