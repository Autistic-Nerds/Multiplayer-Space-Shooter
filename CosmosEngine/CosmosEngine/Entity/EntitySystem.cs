using System;
using System.Collections.Generic;
using System.Reflection;
using Cosmos.Entity.CoreModule;
using CosmosEngine.Collection;

namespace Cosmos.Entity
{
	public abstract class EntitySystem : IDisposable
	{
		private ComponentManager componentManager;
		private bool disposed;
		private bool dirty;
		private Bag<int> entities;

		internal ComponentManager ComponentManager => componentManager;

		internal void Activate(ComponentManager componentManager)
		{
			this.componentManager = componentManager;
			this.entities = new Bag<int>();
		}

		protected virtual void Initialize(EntityWorld world)
		{

		}

		protected virtual void Update()
		{

		}

		protected virtual void Render()
		{

		}

		protected virtual void UI()
		{
		}

		protected IEnumerable<T> GetEntities<T>() where T : struct
		{
				List<T> entities = new List<T>();
				if (componentManager == null)
				{
					//something must have went wrong.
					return entities;
				}

				Type filter = typeof(T);
				FieldInfo[] fields = filter.GetFields(BindingFlags.Public | BindingFlags.Instance);
				foreach (FieldInfo field in fields)
				{
					int componentTypeId = componentManager.GetComponentTypeId(field.FieldType);
					Mapper mapper = componentManager.GetMapper(componentTypeId);
					//MethodInfo method = typeof(ComponentManager).GetMethod("GetMapper");
					//method = method.GetGenericMethodDefinition();
					//if (method.IsGenericMethodDefinition)
					//{
					//	method = method.MakeGenericMethod(field.FieldType);
					//	object value = method.Invoke(componentManager, null);
					//	Mapper mapper = (Mapper)value;
					//}
				}
				return entities;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize((object)this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
				return;
			if (disposing)
			{
				//dispose of unmanaged resources.
			}
			this.disposed = true;
		}
	}
}