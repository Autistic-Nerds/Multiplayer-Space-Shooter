//Written by Philip Wittusen
namespace CosmosEngine.ObjectPooling
{
	public abstract class ObjectPool<T> : Pool<T> where T : CosmosEngine.Component
	{
		public T Request(bool active)
		{
			T item = base.Request();
			item.GameObject.SetActive(active);
			return item;
		}

		public override void Return(T item)
		{
			item.GameObject.SetActive(false);
			base.Return(item);
		}

		protected override void OnDestroy()
		{
			foreach(var item in activePool)
			{
				pool.Push(item);
			}
			activePool.Clear();
			int amount = Count;
			for(int i = 0; i < amount; i++)
			{
				T item = pool.Pop();
				Destroy(item);
			}
		}
	}
}