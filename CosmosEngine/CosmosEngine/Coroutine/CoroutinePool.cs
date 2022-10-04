
using CosmosEngine.Factory;
using CosmosEngine.ObjectPooling;

namespace CosmosEngine.Async
{
	internal class CoroutinePool : Pool<Coroutine>
	{
		private CoroutineFactory factory = Instance<CoroutineFactory>();
		protected override IFactory<Coroutine> Factory => factory;

		public static Coroutine Get() => Instance<CoroutinePool>().Request();
		public static void Release(Coroutine item) => Instance<CoroutinePool>().Return(item);
	}
}