
using CosmosEngine.Async;

namespace CosmosEngine
{
	/// <summary>
	/// Used to suspend a coroutine execution for the given amount of seconds using <see cref="CosmosEngine.Time.DeltaTime"/>.
	/// </summary>
	public sealed class WaitForSeconds : YieldInstruction
	{
		internal float seconds;
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.WaitForSeconds"/>
		/// </summary>
		/// <param name="seconds">Suspends the coroutine execution for the given amount of <paramref name="seconds"/> using scaled time.</param>
		public WaitForSeconds(float seconds) => this.seconds = seconds;

		public override bool KeepWaiting
		{
			get
			{
				seconds -= Time.DeltaTime;
				return seconds > 0;
			}
		}
		public override string ToString() => $"{GetType().Name}({seconds:F3}s)";
	}
}