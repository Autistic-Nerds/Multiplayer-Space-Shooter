using System.Collections.Generic;

namespace CosmosEngine.Collections
{
	public class RandomAssortment<T>
	{
		private readonly List<T> list;
		private readonly int[] occupiedIndex;

		public RandomAssortment() => list = new List<T>();
		public RandomAssortment(int capacity) => list = new List<T>(capacity);
		public RandomAssortment(IEnumerable<T> collection) => list = new List<T>(collection);

		public int Count => list.Count;
		public int Capacity => list.Capacity;

		public void Reset()
		{

		}

		public void Clear()
		{

		}

		public void Add(T item)
		{
			list.Add(item);
		}

		public T Peek()
		{
			int index = Random.Range(0, list.Count);
			return list[index];
		}

		//public T[] PeekRange()
		//{

		//}

		public T Get()
		{
			int index = Random.Range(0, list.Count);
			T item = list[index];
			list.RemoveAt(index);
			return item;
		}

		//public T[] GetRange(int count)
		//{
		//	T[] result = new T[count];

		//}
	}
}