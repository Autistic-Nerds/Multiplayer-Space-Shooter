
using CosmosEngine.EventSystems;
using System;

namespace CosmosEngine.InputModule
{
	public class InputEvent
	{
		private Event inputEvent;
		private Event<CallbackContext> callbackEvent;

		public InputEvent()
		{
			inputEvent = new Event();
			callbackEvent = new Event<CallbackContext>();
		}

		public void Add(Action action)
		{
			if (action != null)
				inputEvent.Add(action);
		}

		public void Add(Action<CallbackContext> action)
		{
			if (action != null)
				callbackEvent.Add(action);
		}

		public void Invoke(CallbackContext context)
		{
			inputEvent.Invoke();
			callbackEvent.Invoke(context);
		}

		public void Remove(Action action)
		{
			if (action != null)
				inputEvent.Remove(action);
		}

		public void Remove(Action<CallbackContext> action)
		{
			if (action != null)
				callbackEvent.Remove(action);
		}

		public void RemoveAll()
		{
			inputEvent.RemoveAllListeners();
			callbackEvent.RemoveAllListeners();
		}
	}
}
