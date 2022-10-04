
using CosmosEngine.Factory;

namespace CosmosEngine.Async
{
	internal class CoroutineFactory : Factory<Coroutine>
	{
		public override Coroutine Create()
		{
			return new Coroutine();
		}
	}
}