using Cosmos.Entity.CoreModule;
using System;

namespace Cosmos.Entity
{
	public sealed class Entity : IEquatable<Entity>
	{
		private static int nextAvailableId;
		private readonly int id;
		private readonly EntityManager entityManager;
		private readonly ComponentManager componentManager;

		public int Id => id;

		internal Entity(int id, EntityManager entityManager, ComponentManager componentManager)
		{
			this.id = id;
			this.entityManager = entityManager;
			this.componentManager = componentManager;
		}

		public static Entity Create()
		{
			nextAvailableId++;
			return new Entity(nextAvailableId, null, null);
		}

		public Entity Add<T>() where T : class, new() => Add(new T());
		public Entity Add<T>(T component) where T : class => Add(component);
		private Entity Add(EntityComponent component)
		{
			componentManager.Attach(Id, component);
			return this;
		}
		public Entity Remove<T>() where T : EntityComponent
		{
			componentManager.Detach<T>(Id);
			return this;
		}
		public T Get<T>() where T : EntityComponent => componentManager.GetMapper<T>().TryGet(id);

		public bool Equals(Entity other)
		{
			if ((object)other == null)
				return false;
			return (object)this == (object)other || this.Id == other.Id;
		}
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if ((object)this == obj)
				return true;
			return !(obj.GetType() != this.GetType()) && this.Equals((Entity)obj);
		}

		public override int GetHashCode() => Id;

		public static bool operator ==(Entity lhs, Entity rhs) => Equals(lhs, rhs);

		public static bool operator !=(Entity lhs, Entity rhs) => !Equals(lhs, rhs);
	}
}