
namespace CosmosEngine
{
	public enum SendMessageOption
	{
		/// <summary>
		/// A receiver is required for SendMessage.
		/// </summary>
		RequireReceiver,
		/// <summary>
		/// No receiver is required for SendMessage.
		/// </summary>
		DontRequireReceiver
	}
}