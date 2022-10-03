//Written by Philip Wittusen
namespace CosmosEngine.Modules
{
	public interface IModule
	{
		int ExecutionOrder { get; }
		void Initialize();
	}
}
