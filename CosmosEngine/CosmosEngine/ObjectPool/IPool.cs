
using System.Collections.Generic;

namespace CosmosEngine.ObjectPooling
{
	public interface IPool<T>
	{
		void Prewarm(int amount);
		T Request();
		IEnumerable<T> Request(int amount = 1);
		void Return(T item);
		void Return(IEnumerable<T> items);
	}
}
