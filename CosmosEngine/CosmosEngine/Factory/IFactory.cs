//Written by Philip Wittusen
namespace CosmosEngine.Factory
{
	public interface IFactory<T>
	{
		T Create();
	}
}
