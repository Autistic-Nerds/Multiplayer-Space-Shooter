using CosmosEngine.Factory;
using CosmosEngine.ObjectPooling;

namespace CosmosEngine.Async
{
	internal class YieldInstructionPool<T> : Pool<T> where T : YieldInstruction, new()
	{
		protected override IFactory<T> Factory => null;

		protected override T Create()
		{
			return new T();
		}
	}
}