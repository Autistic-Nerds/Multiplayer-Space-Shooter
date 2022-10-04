
using System.Collections.Generic;

namespace CosmosEngine.Modules
{
	public static class ObjectDelegater
	{
		private static readonly List<IDelegation> objectDelegations = new List<IDelegation>();
		/// <summary>
		/// This method is invoked whenever a new <see cref="CosmosEngine.CoreModule.Object"/> is instantiated. From here the <see cref="CosmosEngine.CoreModule.Object"/> will invoke the subscribe event of all the classes that has requested it through the <see cref="CosmosEngine.Modules.ObjectDelegater.CreateNewDelegation{T}(System.Action{T})"/> or <see cref="CosmosEngine.Modules.ObjectDelegater.CreateNewDelegation{T}(System.Action{T}, System.Predicate{T})"/>.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns><see langword="true"/> if the instantiation operation was a success and the <paramref name="obj"/> has been assigned to a <see cref="CosmosEngine.Modules.GameModule{T}"/>, or <see langword="false"/> if the <paramref name="obj"/> was not assigned to any <see cref="CosmosEngine.Modules.GameModule{T}"/>.</returns>
		internal static bool OnInstantiate(CoreModule.Object obj)
		{
			//The plan is to have Game Modules "subscribe" to this delegater.
			//Then whenever a new Object is instantiated it will go through the delegater.
			//The delegater will have have a dictionary of predicates.
			//When the Object is passed to this method it will loop over all predicates.
			//Each predicates that matches with this Object will then have the Object "subscribed" to that system
			bool match = false;
			foreach (IDelegation delegation in objectDelegations)
			{
				if (delegation.Match(obj))
				{
					delegation.Invoke(obj);
					match = true;
				}
			}
			return match;
		}

#nullable enable
		/// <summary>
		/// Establish a <see langword="new"/> <see cref="CosmosEngine.Modules.Delegation{T}"/>. Whenever a <see langword="new"/> <see cref="CosmosEngine.CoreModule.Object"/> is instantiated, if the <see cref="CosmosEngine.CoreModule.Object"/> is assignable to <typeparamref name="T"/> the <paramref name="subscribeEvent"/> will be invoked with the <see cref="CosmosEngine.CoreModule.Object"/> as parameter. All objects that was instantiated before this delegation was added, will not be taken into consideration.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="subscribeEvent">The event invoked if the instantiated <see cref="CosmosEngine.CoreModule.Object"/> is assignable to <typeparamref name="T"/>.</param>
		public static void CreateNewDelegation<T>(System.Action<T> subscribeEvent) where T : class => CreateNewDelegation<T>(subscribeEvent, null);
		/// <summary>
		/// Establish a <see langword="new"/> <see cref="CosmosEngine.Modules.Delegation{T}"/>. Whenever a <see langword="new"/> <see cref="CosmosEngine.CoreModule.Object"/> is instantiated, if the <see cref="CosmosEngine.CoreModule.Object"/> is assignable to <typeparamref name="T"/> and matching the assigned <paramref name="predicate"/> the <paramref name="subscribeEvent"/> will be invoked with the <see cref="CosmosEngine.CoreModule.Object"/> as parameter. All objects that was instantiated before this delegation was added, will not be taken into consideration.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="subscribeEvent">The event invoked if the instantiated <see cref="CosmosEngine.CoreModule.Object"/> is assignable to <typeparamref name="T"/>.</param>
		/// <param name="predicate">The match any <see langword="new"/> <see cref="CosmosEngine.CoreModule.Object"/> will be be compared against.</param>
		public static void CreateNewDelegation<T>(System.Action<T> subscribeEvent, System.Predicate<T>? predicate) where T : class => objectDelegations.Add(new Delegation<T>(subscribeEvent, predicate));

#nullable disable
	}
}