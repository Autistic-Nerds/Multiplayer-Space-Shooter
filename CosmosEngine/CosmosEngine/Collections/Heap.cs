
using System.Collections;
using System.Collections.Generic;

namespace CosmosEngine.Collection
{
	public class Heap<T> : IEnumerable<T> where T : IHeapItem<T>
	{
		private T[] items;
		private int currentItemCount;

		public int Count => currentItemCount;

		public Heap(int maxSize)
		{
			items = new T[maxSize];
		}

		public void Add(T item)
		{
			item.HeapIndex = currentItemCount;
			items[currentItemCount] = item;
			SortUp(item);
			currentItemCount++;
		}

		public T Pop()
		{
			T first = items[0];
			currentItemCount--;
			items[0] = items[currentItemCount];
			items[0].HeapIndex = 0;
			SortDown(items[0]);
			return first;
		}

		public void UpdateItem(T item)
		{
			SortUp(item);
		}

		public bool Contains(T item)
		{
			return Equals(items[item.HeapIndex], item);
		}

		private void SortDown(T item)
		{
			while (true)
			{
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;
				int swapIndex = 0;

				if (childIndexLeft < currentItemCount)
				{
					swapIndex = childIndexLeft;
					if (childIndexRight < currentItemCount)
					{
						if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
						{
							swapIndex = childIndexRight;
						}
					}

					if (item.CompareTo(items[swapIndex]) < 0)
					{
						Swap(item, items[swapIndex]);
					}
					else
					{
						return;
					}

				}
				else
				{
					return;
				}

			}
		}

		private void SortUp(T item)
		{
			int parentIndex = (item.HeapIndex - 1) / 2;

			while (true)
			{
				T parentItem = items[parentIndex];
				if (item.CompareTo(parentItem) > 0)
				{
					Swap(item, parentItem);
				}
				else
				{
					break;
				}

				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		private void Swap(T itemA, T itemB)
		{
			items[itemA.HeapIndex] = itemB;
			items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}

		public IEnumerator<T> GetEnumerator()
		{
			List<T> list = new List<T>();
			foreach(T item in items)
			{
				list.Add(item);
			}
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
