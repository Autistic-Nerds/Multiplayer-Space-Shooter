
using System;

namespace CosmosEngine.Collection
{
	public interface IHeapItem<T> : IComparable<T>
	{
		int HeapIndex { get; set; }
	}
}
