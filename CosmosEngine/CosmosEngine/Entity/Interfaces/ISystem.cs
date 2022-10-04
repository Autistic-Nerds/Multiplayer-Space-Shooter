using System;

namespace Cosmos.Entity.CoreModule
{
	[System.Obsolete("", false)]
	public interface ISystem : IDisposable
	{
		void Initialize(EntityWorld world);
	}
}