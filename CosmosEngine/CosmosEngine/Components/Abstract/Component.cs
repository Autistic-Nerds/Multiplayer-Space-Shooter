
using CosmosEngine.CoreModule;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CosmosEngine
{
	public abstract class Component : Behaviour
	{
		#region Fields
		private GameObject gameObject;
		/// <summary>
		/// The name of the <see cref="CosmosEngine.GameObject"/> this <see cref="CosmosEngine.Component"/> is attached to.
		/// </summary>
		public override string Name { get => GameObject == null ? GetType().Name : GameObject.Name; set => GameObject.Name = value; }
		/// <summary>
		/// The enabled state of a <see cref="CosmosEngine.Component"/> is determined by its <see cref="CosmosEngine.GameObject"/> and its own enabled state. If the <see cref="CosmosEngine.GameObject"/> is disabled this <see cref="CosmosEngine.Component"/> will also be seen as disabled. To see the component's own enabled state use <see cref="CosmosEngine.Component.Active"/>.
		/// </summary>
		public override bool Enabled { get => base.Enabled && (GameObject != null && GameObject.Enabled); set => base.Enabled = value; }
		public override bool DestroyOnLoad
		{
			get
			{
				bool destroy = base.DestroyOnLoad && GameObject.DestroyOnLoad;
				return destroy;
			}
			set => base.DestroyOnLoad = value;
		}
		public GameObject GameObject => gameObject;
		public override Transform Transform => GameObject != null ? GameObject.Transform : null;
		#endregion

		#region Public Methods
		/// <summary>
		/// Method invoked when this <see cref="CosmosEngine.Component"/> has been assigned its <see cref="CosmosEngine.GameObject"/>. Method is invoked before Awake, it's possible to override but requires base call.
		/// </summary>
		/// <param name="gameObject"></param>
		internal virtual void AssignGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		#region Get or Add
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.AddComponent{T}"/>
		/// </summary>
		public override T AddComponent<T>() => GameObject.AddComponent<T>();
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.AddComponent(Type)"/>
		/// </summary>
		public override Component AddComponent(Type componentType) => GameObject.AddComponent(componentType);

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.GetComponent{T}"/>
		/// </summary>
		public override T GetComponent<T>() => GameObject.GetComponent<T>();
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.GetComponent(Type)"/>
		/// </summary>
		public override Component GetComponent(Type componentType) => GameObject.GetComponent(componentType);

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.GetComponents{T}"/>
		/// </summary>
		public override T[] GetComponents<T>() => GameObject.GetComponents<T>();
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.GetComponents(Type)"/>
		/// </summary>
		public override Component[] GetComponents(Type componentType) => GameObject.GetComponents(componentType);
		#endregion

		internal override void MarkForDestruction(float t = 0)
		{
			RequireComponent requireComponent = this.GetType().GetCustomAttribute<RequireComponent>(true);
			if (requireComponent != null)
			{
				List<Type> requiredComponents = new List<Type>();
				requiredComponents.AddRange(requireComponent.RequiredComponents);
				Component[] components = GameObject.GetComponentsAll();
				foreach (Component c in components)
				{
					//if c has the RequireComponent attribute
					//and RequireComponent types contains this component type
					//Then we can't remove this component sicne it's required by another component
					//Return and LogError
					if (requiredComponents.Contains(c.GetType()))
					{
						return;
					}
				}
			}
			base.MarkForDestruction(t);
		}

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed && disposing)
			{
				gameObject = null;
			}
			base.Dispose(disposing);
		}

		public override string ToString()
		{
			return $"{Name} - Type: {GetType().Name}";
		}
		#endregion
	}
}