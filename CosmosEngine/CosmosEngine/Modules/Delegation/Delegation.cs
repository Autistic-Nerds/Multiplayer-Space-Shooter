
namespace CosmosEngine.Modules
{
	public class Delegation<T> : IDelegation where T : class
	{
		private System.Action<T> subscribeAction;
		private System.Predicate<T> match;

		public System.Type Type => typeof(T);

		public Delegation(System.Action<T> subscribeAction, System.Predicate<T> match)
		{
			this.subscribeAction = subscribeAction;
			this.match = match;
		}

		public void Invoke(object obj)
		{
			subscribeAction.Invoke((T)obj);
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