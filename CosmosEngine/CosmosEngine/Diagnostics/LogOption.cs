
namespace CosmosEngine
{
	[System.Flags]
	public enum LogOption
	{
		None = 0,
		/// <summary>
		/// Log messages will not contain stacktrace information.
		/// </summary>
		NoStacktrace = 1 << 0,
		/// <summary>
		/// Log messages will not display the amount of times they've been invoked.
		/// </summary>
		IgnoreCallCount = 1 << 1,
		/// <summary>
		/// Log messages from the same object will be collapsed on eachother.
		/// </summary>
		Collapse = 1 << 2,
		/// <summary>
		/// When collapsing messages only the Debug call will be compared, not the message. Message will be replaced by the newest.
		/// </summary>
		CompareInitialCallOnly = 1 << 3,
		/// <summary>
		/// If the log is formatted as an error, the game will be paused when the log is invoked.
		/// </summary>
		PauseOnError = 1 << 4,
		/// <summary>
		/// If an enumerable is logged to the context, such as <see cref="System.Collections.Generic.List{T}"/>. The message can be expanded to display individual objects. When logging a collection stacktrace will not be displayed.
		/// </summary>
		Collection = 1 << 5,
	}
}