using Cosmos.Entity.CoreModule;

namespace Cosmos.Entity
{
	[System.Obsolete("", false)]
	public interface IUpdateSystem : ISystem
	{
		void Update();
	}
}