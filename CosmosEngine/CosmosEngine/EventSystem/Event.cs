
using System;
using System.Collections.Generic;

namespace CosmosEngine.EventSystems
{
	/// <summary>
	/// Works like System.Action, but has a more userfriendly method to Add or Remove events. It forces the user to use the Invoke() to invoke the event.
	/// </summary>
	public class Event : EventBase, IReplicatable<Event>
	{
		private Action action = delegate { };
		private List<Event> events = new List<Event>();

		/// <summary>
		/// Add a non persistent listener to the Event.
		/// </summary>
		/// <param name="call"></param>
		public void Add(Action call)
		{
			action += call;
		}

		public void Add(Event call)
		{
			events.Add(call);
		}

		/// <summary>
		/// Remove a non persistent listener from the Event.
		/// </summary>
		/// <param name="call"></param>
		public void Remove(Action call)
		{
			action -= call;
		}

		public void Remove(Event call)
		{
			events.Remove(call);
		}

		public override void RemoveAllListeners() => action = delegate { };

		/// <summary>
		/// Invoke all registered callbacks (runtime and persistent).
		/// </summary>
		public void Invoke()
		{
			action?.Invoke();
			foreach(var listener in events)
			{
				listener?.Invoke();
			}
		}

		public Event Clone(Event original)
		{
			Event newEvent = new Event();
			newEvent.Add(original.action);
			return newEvent;
		}
	}

	/// <summary>
	/// Works like System.Action, but has a more userfriendly method to Add or Remove events. It forces the user to use the Invoke() to invoke the event.
	/// </summary>
	public class Event<T> : EventBase, IReplicatable<Event<T>>
	{
		private Action<T> action = delegate { };

		public virtual void Add(Action<T> call)
		{
			action += call;
		}

		public virtual void Remove(Action<T> call)
		{
			action -= call;
		}

		public override void RemoveAllListeners() => action = delegate { };

		public virtual void Invoke(T arg)
		{
			action?.Invoke(arg);
		}

		public Event<T> Clone(Event<T> original)
		{
			Event<T> newEvent = new Event<T>();
			newEvent.Add(original.action);
			return newEvent;
		}
	}

	/// <summary>
	/// Works like System.Action, but has a more userfriendly method to Add or Remove events. It forces the user to use the Invoke() to invoke the event.
	/// </summary>
	public class Event<T1, T2> : EventBase, IReplicatable<Event<T1, T2>>
	{
		private Action<T1, T2> action = delegate { };

		public void Add(Action<T1, T2> call)
		{
			action += call;
		}

		public void Remove(Action<T1, T2> call)
		{
			action -= call;
		}

		public override void RemoveAllListeners() => action = delegate { };

		public void Invoke(T1 arg1, T2 arg2)
		{
			action?.Invoke(arg1, arg2);
		}

		public Event<T1, T2> Clone(Event<T1, T2> original)
		{
			Event<T1, T2> newEvent = new Event<T1, T2>();
			newEvent.Add(original.action);
			return newEvent;
		}
	}
}
