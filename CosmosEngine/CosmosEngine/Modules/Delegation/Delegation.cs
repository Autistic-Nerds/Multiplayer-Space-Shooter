
namespace CosmosEngine.Modules
{
	public class Delegation<T> : IDelegation where T : class
	{
		private System.Action<T> subscribeEvent;
		private System.Predicate<T> match;

		public System.Type Type => typeof(T);

		public Delegation(System.Action<T> subscribeEvent, System.Predicate<T> match)
		{
			this.subscribeEvent = subscribeEvent;
			this.match = match;
		}

		public void Invoke(object obj)
		{
			subscribeEvent.Invoke((T)obj);
		}

		public bool Match(object obj)
		{
			bool assignable = obj.GetType().IsAssignableTo(typeof(T));
			if (assignable)
			{
				if (match == null)
					return assignable;
				else
					return match.Invoke((T)obj);
			}
			return false;
		}
	}
}