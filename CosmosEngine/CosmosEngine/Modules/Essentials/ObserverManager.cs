
using CosmosEngine.Collection;
using CosmosEngine.CoreModule;
using System.Reflection;

namespace CosmosEngine.Modules
{
	/// <summary>
	/// The <see cref="CosmosEngine.Modules.ObserverManager{TItem, TManager}"/> is an implementation of the <see href="https://gameprogrammingpatterns.com/observer.html">Observer Pattern</see>, this allows for objects to subscribe and unsubscribe to the manager. Events and calls should be handled by the individual manager in the <see cref="BeginEventCall()"/> method.
	/// </summary>
	/// <typeparam name="TItem">Observed item.</typeparam>
	/// <typeparam name="TManager">The class itself for singleton reference.</typeparam>
	public abstract class ObserverManager<TItem, TManager> : GameModule<TManager>, IUpdateModule where TManager : ObserverManager<TItem, TManager>, new()
	{
		protected const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.NonPublic;
		protected readonly DirtyList<TItem> itemsToAdd = new DirtyList<TItem>();
		protected readonly DirtyList<TItem> itemsToRemove = new DirtyList<TItem>();
		protected readonly DirtyList<TItem> observerList = new DirtyList<TItem>();
		protected bool isUpdating;
		/// <summary>
		/// Count of all objects in the observer list.
		/// </summary>
		public static int Count => Instance.observerList.Count;

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.CoreModule.ObserverManager{TItem, TManager}.SubscribeItem(TItem)"/>
		/// </summary>
		/// <param name="item"></param>
		public static void Subscribe(TItem item)
		{
			if(Instance == null)
			{
				//Log error; Trying to add item to a Manager that has not been added as a Game System.
				return;
			}
			Instance.SubscribeItem(item);
		}

		/// <summary>
		/// Subscribe a <typeparamref name="TItem"/> to this Observer, <paramref name="item"/> will automatically be added at the next frame.
		/// </summary>
		public virtual void SubscribeItem(TItem item)
		{
			if (!SystemExist())
				return;

			if (isUpdating)
			{
				itemsToAdd.Add(item);
			}
			else
			{
				Add(item);
			}
		}

		/// <summary>
		/// Manually adds an item to the Observer, use Subscribe() instead, using Add may interrupt and change the list while it's updating, this may cause a (Collection Was Modified) exception if not handled properly. This is the method which adds the <typeparamref name="TItem"/> to the observer list, if any process should happen on the <paramref name="item"/> as it's added to the list <see langword="override"/> this method.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void Add(TItem item)
		{
			observerList.Add(item);
			observerList.IsDirty = true;
		}

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.CoreModule.ObserverManager{TItem, TManager}.UnsubscribeItem(TItem)"/>
		/// </summary>
		/// <param name="item"></param>
		public static void Unsubscribe(TItem item)
		{
			if (Instance == null)
			{
				//Log error; Trying to remove item from a Manager that has not been added as a Game System.
				return;
			}
			Instance.UnsubscribeItem(item);
		}

		/// <summary>
		/// Unsubscribe a <typeparamref name="TItem"/> from this Observer, <paramref name="item"/> will automatically be removed at the first opportunity.
		/// </summary>
		public virtual void UnsubscribeItem(TItem item)
		{
			if (!SystemExist())
				return;

			if (isUpdating)
			{
				itemsToAdd.Add(item);
			}
			else
			{
				Remove(item);
			}
		}

		/// <summary>
		/// Manually removes an item from the Observer, use Unsubscribe() instead, using Remove may interrupt and change the list while it's updating, this may cause a (Collection Was Modified) exception if not handled properly. This is the method which removes the <typeparamref name="TItem"/> from the observer list, if any process should happen on the <paramref name="item"/> as it's removed from the list <see langword="override"/> this method.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void Remove(TItem item)
		{
			observerList.Remove(item);
			observerList.IsDirty = true;
		}

		/// <summary>
		/// Returns <see langword="true"/> if the <paramref name="methodName"/> exists on the given <paramref name="type"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="methodName"></param>
		/// <returns></returns>
		protected bool MethodExistsOnObject(System.Type type, string methodName)
		{
			bool methodExists = false;
			System.Type t = type;
			MethodInfo method = type.GetMethod(methodName, DefaultFlags);
			do
			{
				if (method != null && method.DeclaringType == t)
				{
					methodExists = true;
					break;
				}
				t = t.BaseType;
			} while (t != typeof(Behaviour));
			return methodExists;
		}

		public virtual void Update()
		{
			isUpdating = true;
			BeginEventCall();
			isUpdating = false;
			foreach (TItem item in itemsToAdd)
			{
				Add(item);
			}
			itemsToAdd.Clear();
			foreach (TItem item in itemsToRemove)
			{
				Remove(item);
			}
			itemsToRemove.Clear();
			if (observerList.IsDirty)
			{
				observerList.RemoveAll(RemoveAllPredicate());
				observerList.IsDirty = false;
			}
		}
		/// <summary>
		/// This method should derived classes override to implement disposing of managed resources. Immediately releases the unmanaged resources used by this object.
		/// </summary>
		/// <param name="disposing">True if objects should be disposed.</param>
		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed && disposing)
			{
				observerList.Clear();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// BegingEventCall is used to handle the events and callbacks of the observed items.
		/// </summary>
		public abstract void BeginEventCall();
		/// <summary>
		/// The <see cref="System.Predicate{TItem}"/> that defines how to remove observed items when the list is mark as dirty. 
		/// </summary>
		/// <returns></returns>
		public abstract System.Predicate<TItem> RemoveAllPredicate();

	}
}
