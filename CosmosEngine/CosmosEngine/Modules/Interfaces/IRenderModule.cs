//Written by Philip Wittusen
namespace CosmosEngine.Modules
{
	public interface IRenderModule : IModule
	{
		void RenderWorld();
		void RenderUI();
	}
}