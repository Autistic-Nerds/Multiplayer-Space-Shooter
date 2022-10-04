using CosmosEngine.Collection;
using System;
using System.Collections.Generic;

namespace Cosmos.Entity.CoreModule
{
	internal sealed class ComponentManager
	{
		private Bag<Mapper> componentMappers;
		private Dictionary<Type, int> componentTypes;
		private event Action<int> componentChangedEvent = delegate { };
		public Action<int> ComponentChangedEvent { get => componentChangedEvent; set => componentChangedEvent = value; }

		public ComponentManager()
		{
			componentMappers = new Bag<Mapper>();
			componentTypes = new Dictionary<Type, int>();
		}

		private ComponentMapper<T> CreateMapperForType<T>(int componentTypeId) where T : class
		{
			ComponentMapper<T> mapperForType = componentTypeId < 128 ? new ComponentMapper<T>(componentTypeId, componentChangedEvent) : throw new InvalidOperationException("Component type limit exceeded. Currently only 128 component types are allowed for performance reasons.");
			componentMappers[componentTypeId] = (Mapper)mapperForType;
			return mapperForType;
		}

		public int GetComponentTypeId(Type type)
		{
			int componentTypeId;
			if (componentTypes.TryGetValue(type, out componentTypeId))
				return componentTypeId;
			int count = componentTypes.Count;
			componentTypes.Add(type, count);
			return count;
		}
		public Mapper GetMapper(int componentTypeId) => componentMappers[componentTypeId];
		public ComponentMapper<T> GetMapper<T>() where T : class
		{
			int componentTypeId = GetComponentTypeId(typeof(T));
			return componentMappers[componentTypeId] != null ? componentMappers[componentTypeId] as ComponentMapper<T> : CreateMapperForType<T>(componentTypeId);
		}

		public void Attach<T>(int entityId) where T : class, new() => Attach(entityId, new T());

		public void Attach<T>(int entityId, T component) where T : class
		{
			GetMapper<T>().Put(entityId, component);
		}

		public void Detach<T>(int entityId) where T : class => GetMapper<T>().Delete(entityId);


	}
}