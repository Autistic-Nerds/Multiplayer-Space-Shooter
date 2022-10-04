using CosmosEngine.Collection;
using System;
using System.Collections.Generic;

namespace Cosmos.Entity.CoreModule
{
	internal class ComponentMapper<T> : Mapper where T : class
	{
		private readonly Bag<T> components;
		private readonly Action<int> onCompositionChanged;
		private readonly List<int> entities = new List<int>();

		public Bag<T> Components => components;

		public ComponentMapper(int id, Action<int> onCompositionChanged) : base(id, typeof(T))
		{
			this.onCompositionChanged = onCompositionChanged;
			this.components = new Bag<T>();
		}

		public void Put(int entityId, T component)
		{
			Components[entityId] = component;
			entities.Add(entityId);
			onCompositionChanged.Invoke(entityId);
		}

		public override IEnumerable<int> All() => entities;
		public T Get(Entity entity) => Get(entity.Id);
		public T Get(int entityId) => Components[entityId];
		public T TryGet(Entity entity) => TryGet(entity.Id);
		public T TryGet(int entityId) => Has(entityId) ? Get(entityId) : default(T);
		public override bool Has(int entityId) => entityId < Components.Count && (object)components[entityId] != null;
		public override void Delete(int entityId)
		{
			Components[entityId] = default(T);
			onCompositionChanged.Invoke(entityId);
		}

	}
}