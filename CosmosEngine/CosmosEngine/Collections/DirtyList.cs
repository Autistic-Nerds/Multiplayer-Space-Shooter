using System;
using System.Collections.Generic;

namespace CosmosEngine.Collection
{
	/// <summary>
	/// <inheritdoc cref="List{T}"/> And can be marked with a <see href="https://gameprogrammingpatterns.com/dirty-flag.html">Dirty Flag</see> to reduce call time.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DirtyList<T> : List<T>
	{
		private bool dirty;

		public bool IsDirty { get => dirty; set => dirty = value; }

		/// <summary>
		/// This method can only be invoked if the list is marked as dirty. <inheritdoc cref="System.Collections.Generic.List{T}.RemoveAll(Predicate{T})"/> Will remove the dirty mark on this list.
		/// </summary>
		/// <param name="match"></param>
		public void DisposeAll(Predicate<T> match)
		{
			if (!dirty)
				return;
			RemoveAll(match);
			this.dirty = false;
		}
	}
}
