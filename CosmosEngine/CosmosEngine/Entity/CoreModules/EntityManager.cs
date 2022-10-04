namespace Cosmos.Entity.CoreModule
{
	internal class EntityManager
	{
		private Entity[] entities;

		public void CreateEntity()
		{

		}

		public void DestroyEntity()
		{

		}

		public Entity[] All<T>() where T : struct
		{
			throw new System.NotImplementedException();
		}

		public Entity[] One<T>() where T : struct
		{
			throw new System.NotImplementedException();
		}

		public Entity[] Exclude<T>() where T : struct
		{
			throw new System.NotImplementedException();
		}
	}
}