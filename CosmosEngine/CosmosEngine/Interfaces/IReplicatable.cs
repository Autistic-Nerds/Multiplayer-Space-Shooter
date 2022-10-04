
namespace CosmosEngine
{
	/// <summary>
	/// Objects with this interfaces, indicates to the Prefab operation that this class has a specific cloning method.
	/// </summary>
	/// <typeparam name="T">The class being replicated.</typeparam>
	public interface IReplicatable<T> where T : class, new()
	{
		T Clone(T original);
	}

	public interface ICopyable<T> where T : class
	{
		void Copy(T original, T copy);
	}
}