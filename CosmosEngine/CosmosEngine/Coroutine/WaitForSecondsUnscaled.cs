
using CosmosEngine.Async;

namespace CosmosEngine
{
	/// <summary>
	/// Used to suspend a coroutine execution for the given amount of seconds using <see cref="CosmosEngine.Time.UnscaledDeltaTime"/>.
	/// </summary>
	public sealed class WaitForSecondsUnscaled : YieldInstruction
	{
		internal float seconds;
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.WaitForSeconds"/>
		/// </summary>
		/// <param name="seconds">Suspends the coroutine execution for the given amount of <paramref name="seconds"/> using unscaled time.</param>
		public WaitForSecondsUnscaled(float seconds) => this.seconds = seconds;

		public override bool KeepWaiting
		{
			get
			{
				seconds -= Time.UnscaledDeltaTime;
				return seconds > 0;
			}
		}
		public override string ToString() => $"{GetType().Name}({seconds:F3}s)";
	}
}