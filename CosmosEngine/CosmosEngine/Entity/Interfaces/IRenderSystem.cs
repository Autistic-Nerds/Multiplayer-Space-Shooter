using Cosmos.Entity.CoreModule;

namespace Cosmos.Entity
{
	[System.Obsolete("", false)]
	public interface IRenderSystem : ISystem
	{
		void Render();
	}
}