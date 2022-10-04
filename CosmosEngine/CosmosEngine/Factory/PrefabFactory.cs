
using CosmosEngine.CoreModule;

namespace CosmosEngine.Factory
{
	public class PrefabFactory<T> : Factory<T> where T : Object, new()
	{
		private GameObject prefab;
		/// <summary>
		/// The prefab created using <see cref="CosmosEngine.Factory.PrefabFactory{T}.SetPrefab"/>.
		/// </summary>
		protected GameObject Prefab => prefab ??= SetPrefab();
		/// <summary>
		/// Creates the initial prefab. To accomplish this override this method by creating a new GameObject and assigning desired components and values, then use <see cref="CosmosEngine.GameObject.MarkAsPrefab"/> to make it hidden from everything. This will keep the <see cref="CosmosEngine.GameObject"/> in the memory and allow it to be used by the <see cref="CosmosEngine.Factory.Factory{T}"/>, but it will be ignored by everything else.
		/// </summary>
		/// <returns></returns>
		protected virtual GameObject SetPrefab()
		{
			GameObject gameObject = new GameObject(typeof(T).Name + " (Prefab)");
			if (typeof(T).IsAssignableFrom(typeof(Component)))
			{
				gameObject.AddComponent(System.Activator.CreateInstance<T>() as Component);
			}
			gameObject.Enabled = false;
			return gameObject;
		}
		/// <summary>
		/// Creates a clone of the prefab and returns the <typeparamref name="T"/> attached to the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <returns>A new instantiated prefab with <typeparamref name="T"/>.</returns>
		public override T Create() => Instantiate<T>(Prefab);
	}
}