using System;
using System.Collections.Generic;

namespace Cosmos.Entity.CoreModule
{
	internal abstract class Mapper
	{
		private readonly int id;
		private readonly Type componentType;

		public int Id => id;
		public Type ComponentType => componentType;
		protected Mapper(int id, Type componentType)
		{
			this.id = id;
			this.componentType = componentType;
		}

		public abstract IEnumerable<int> All();
		public abstract bool Has(int entityId);
		public abstract void Delete(int entityId);
	}
}