
namespace CosmosEngine.EventSystems
{
	public abstract class EventBase
	{
		/// <summary>
		/// Remove all non-persisent (ie created from script) listeners from the event.
		/// </summary>
		public abstract void RemoveAllListeners();
	}
}
