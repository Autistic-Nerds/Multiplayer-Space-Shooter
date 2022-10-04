
using System.Collections.Generic;
using CosmosEngine.Factory;

namespace CosmosEngine.ObjectPooling
{
	/// <summary>
	/// An object pool that can be used to handle a large amount of simular objects without the need to constantly create new.
	/// </summary>
	/// <typeparam name="T">The item this object pool contains.</typeparam>
	public abstract class Pool<T> : ScriptableObject, IPool<T>
	{
		/// <summary>
		/// Pool of inactive objects, ready to be pulled from the pool
		/// </summary>
		protected readonly Stack<T> pool = new Stack<T>();
		/// <summary>
		/// List of active objects, that needs to be returned to the pool.
		/// </summary>
		protected readonly List<T> activePool = new List<T>();
		private bool hasBeenPrewarmed;
		/// <summary>
		/// The factory used when creating objects.
		/// </summary>
		protected abstract IFactory<T> Factory { get; }
		/// <summary>
		/// Number of objects that are currently available in the pool.
		/// </summary>
		public int Count => pool.Count;
		/// <summary>
		/// Prewarms the pool by creating an initial <paramref name="amount"/> of objects. This can only be done once on a pool.
		/// </summary>
		/// <param name="amount"></param>
		public virtual void Prewarm(int amount)
		{
			if(hasBeenPrewarmed)
			{
				return;
			}
			for(int i = 0; i < amount; i++)
			{
				pool.Push(Create());
			}
			hasBeenPrewarmed = true;
		}

		/// <summary>
		/// Creates a new items by using the factory.
		/// </summary>
		/// <returns>The new object created.</returns>
		protected virtual T Create()
		{
			return Factory.Create();
		}

		/// <summary>
		/// Requests an item from the pool, if no item is present a new one will be created. <typeparamref name="TItem"/> is not modified when requested from the pool, to modify the item before it's returned override this method.
		/// </summary>
		/// <returns>An item from the pool.</returns>
		public virtual T Request()
		{
			T item = pool.Count > 0 ? pool.Pop() : Create();
			activePool.Add(item);
			return item;
		}

		/// <summary>
		/// Requests an amount of items from the pool, if no item is present a new one will be created.
		/// </summary>
		/// <param name="amount"></param>
		/// <returns>An <see cref="IEnumerable{T}"/> containing the <paramref name="amount"/> of items requested.</returns>
		public virtual IEnumerable<T> Request(int amount = 1)
		{
			List<T> items = new List<T>(amount);
			for (int i = 0; i < amount; i++)
			{
				items.Add(Request());
			}
			return items;
		}

		/// <summary>
		/// Returns an item to the pool. <typeparamref name="TItem"/> is not modified when returned to the pool, to modify the item before it's returned override this method.
		/// </summary>
		/// <param name="item"></param>
		public virtual void Return(T item)
		{
			activePool.Remove(item);
			pool.Push(item);
		}

		/// <summary>
		/// Returns an amount of items to the pool.
		/// </summary>
		/// <param name="items"></param>
		public virtual void Return(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				Return(item);
			}
		}
	}
}
