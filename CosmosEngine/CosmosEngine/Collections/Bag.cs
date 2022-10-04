using System;
using System.Collections;
using System.Collections.Generic;

namespace CosmosEngine.Collection
{
	/// <summary>
	/// A Bag is an un-ordered array of items with fast Add and Remove properties.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Bag<T> : IEnumerable<T>, IEnumerable
	{
		private T[] items;
		private readonly bool isPrimitive;
		private int count;

		public int Capacity => items.Length;
		public bool IsEmpty => count == 0;
		public int Count => count;

		public Bag(int capacity = 16)
		{
			isPrimitive = typeof(T).IsPrimitive;
			items = new T[capacity];
		}

		public T this[int index]
		{
			get => index < items.Length ? items[index] : default(T);
			set
			{
				EnsureCapacity(index + 1);
				if (index >= count)
					count = index + 1;
				items[index] = value;
			}
		}

		public void Add(T element)
		{
			EnsureCapacity(count + 1);
			items[count] = element;
			++count;
		}

		public void AddRange(Bag<T> range)
		{
			int index = 0;
			for (int count = range.count; count > index; ++index)
				Add(range[index]);
		}

		public void Clear()
		{
			if (count == 0)
				return;
			if (!isPrimitive)
				Array.Clear((Array)items, 0, count);
			count = 0;
		}

		public bool Contains(T element)
		{
			for (int index = count - 1; index >= 0; --index)
			{
				if (element.Equals((object)items[index]))
					return true;
			}
			return false;
		}

		public T RemoveAt(int index)
		{
			T obj = items[index];
			--count;
			items[index] = items[count];
			items[count] = default(T);
			return obj;
		}

		public bool Remove(T element)
		{
			for (int index = count - 1; index >= 0; --index)
			{
				if (element.Equals((object)items[index]))
				{
					--count;
					items[index] = items[count];
					items[count] = default(T);
					return true;
				}
			}
			return false;
		}

		public bool RemoveAll(Bag<T> bag)
		{
			bool flag = false;
			for (int index = bag.count - 1; index >= 0; --index)
			{
				if (Remove(bag[index]))
					flag = true;
			}
			return flag;
		}

		private void EnsureCapacity(int capacity)
		{
			if (capacity < items.Length)
				return;
			int length = Math.Max((int)((double)items.Length * 1.5), capacity);
			T[] collection = items;
			items = new T[length];
			Array.Copy((Array)collection, 0, (Array)items, 0, items.Length);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>)new Bag<T>.BagEnumerator(this);

		IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)new Bag<T>.BagEnumerator(this);

		internal struct BagEnumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private volatile Bag<T> _bag;
			private volatile int _index;

			public BagEnumerator(Bag<T> bag)
			{
				_bag = bag;
				_index = -1;
			}

			T IEnumerator<T>.Current => _bag[_index];

			object IEnumerator.Current => (object)_bag[_index];

			public bool MoveNext() => ++_index < _bag.count;

			public void Dispose()
			{
			}

			public void Reset()
			{
			}
		}
	}
}