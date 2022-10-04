
namespace CosmosEngine.Factory
{
	public abstract class Factory<T> : ScriptableObject, IFactory<T> where T : new()
	{
		/// <summary>
		/// Creates an item (<typeparamref name="T"/>).
		/// </summary>
		/// <returns>A new instantiated of <typeparamref name="T"/>.</returns>
		public abstract T Create();
	}
}
