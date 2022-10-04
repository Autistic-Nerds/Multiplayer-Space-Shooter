using Cosmos.Entity.CoreModule;
using System.Collections.Generic;

namespace Cosmos.Entity
{
	public class EntityWorld
	{
		private static EntityWorld instance;
		public static EntityWorld Instance => instance;

		private ComponentManager componentManager;
		private readonly List<EntitySystem> entitySystems = new List<EntitySystem>();

		public EntityWorld()
		{
			if(instance == null)
			{
				instance = this;
			}
			componentManager = new ComponentManager();
		}

		public EntityWorld AddSystem<T>() where T : EntitySystem, new()
		{
			T system = new T();
			entitySystems.Add(new T());
			system.Activate(componentManager);
			return this;
		}
	}
}