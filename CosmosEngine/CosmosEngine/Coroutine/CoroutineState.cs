
namespace CosmosEngine.Async
{
	public enum CoroutineState
	{
		/// <summary>The <see cref="CosmosEngine.Coroutine"/> has successfully executed its function and has now stopped running.</summary>
		Stopped,
		/// <summary>The <see cref="CosmosEngine.Coroutine"/> is currently executing its function.</summary>
		Running,
		/// <summary>The <see cref="CosmosEngine.Coroutine"/> is waiting for a <see cref="CosmosEngine.Async.YieldInstruction"/> to complete before executing again.</summary>
		Waiting,
		/// <summary>The <see cref="CosmosEngine.Coroutine"/> has been paused using <see cref="CosmosEngine.Coroutine.Pause"/> and will start executing its function again once <see cref="CosmosEngine.Coroutine.Resume"/> is invoked.</summary>
		Paused,
		/// <summary>The <see cref="CosmosEngine.Coroutine"/> was interrupted using StopCoroutine method and was not executed successfully.</summary>
		Interrupted,
	}
}