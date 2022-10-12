
using CosmosEngine.Collection;
using CosmosEngine.CoreModule;
using CosmosEngine.EventSystems;
using CosmosEngine.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CosmosEngine
{
	/// <summary>
	/// A <see cref="CosmosEngine.GameObject"/> represents a scene object, the <see cref="CosmosEngine.GameObject"/> behaviour is represented by the <see cref="CosmosEngine.Component"/> which are attached to it.
	/// </summary>
	public sealed class GameObject : Behaviour
	{
		private readonly List<string> tag = new List<string>();
		private Transform transform;
		private readonly DirtyList<Component> components = new DirtyList<Component>();
		private readonly Event<GameObjectChange> gameObjectModifiedEvent = new Event<GameObjectChange>();

		/// <summary>
		/// The tag of this <see cref="CosmosEngine.GameObject"/>. A tag can be used to identify a game object.
		/// </summary>
		public string[] Tag 
		{ 
			get => tag.ToArray();
			set
			{
				tag.Clear();
				tag.AddRange(value);
			}
		}
		/// <summary>
		/// The <see cref="CosmosEngine.Transform"/> attached to the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		public override Transform Transform => transform ??= ReplaceTransform<Transform>();
		/// <summary>
		/// An event invoked whenever changes happen to this <see cref="CosmosEngine.GameObject"/>, such as a <see cref="CosmosEngine.Component"/> being added or removed.
		/// </summary>
		public Event<GameObjectChange> ModifiedEvent => gameObjectModifiedEvent;

		public override bool DestroyOnLoad 
		{
			get
			{
				bool destroy = base.DestroyOnLoad;
				Finalise();
				foreach(Component c in components)
				{
					if(!c.DestroyOnLoad)
					{
						destroy = false;
						break;
					}
				}
				return destroy;
			}
			set => base.DestroyOnLoad = value;
		}

		#region Constructor
		/// <summary>
		/// Creates a new empty <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		public GameObject()
		{
			if (string.IsNullOrEmpty(Name))
			{
				Name = "New GameObject";
			}
			Initialize();
		}

		/// <summary>
		/// Creates a new empty <see cref="CosmosEngine.GameObject"/> with a given name.
		/// </summary>
		/// <param name="name">The name that the <see cref="CosmosEngine.GameObject"/> is created with.</param>
		public GameObject(string name) : this()
		{
			this.Name = name;
		}

		/// <summary>
		/// Creates a new <see cref="CosmosEngine.GameObject"/> with a given name and a list of <see cref="CosmosEngine.Component"/> that will be added at instantiation.
		/// </summary>
		/// <param name="name">The name that the <see cref="CosmosEngine.GameObject"/> is created with.</param>
		/// <param name="components">A list of <see cref="CosmosEngine.Component"/> to add to the <see cref="CosmosEngine.GameObject"/> on creation.</param>
		public GameObject(string name, params Type[] components) : this(name)
		{
			foreach (Type type in components)
			{
				AddComponent(Activator.CreateInstance(type) as Component);
			}
		}

		#endregion

		#region Public Methods

		protected override void Awake()
		{
			if (Transform == null) { }
		}

		[System.Obsolete("Method has been replaced by generic version.", false)]
		/// <summary>
		/// Replaces <see langword="this"/> game object's <see cref="CosmosEngine.Transform"/> with the <paramref name="newTransform"/>. If <paramref name="keepWorldSpace"/> is <see langword="true"/> the <paramref name="newTransform"/> values will be copied from the old <see cref="CosmosEngine.Transform"/>.
		/// </summary>
		/// <param name="newTransform"></param>
		/// <param name="keepWorldSpace"></param>
		/// <returns></returns>
		internal Transform ReplaceTransform(Transform newTransform, bool keepWorldSpace = true)
		{
			if (transform != null)
			{
				transform.DestroyImmediate();
				if (keepWorldSpace)
				{
					newTransform.Copy(transform);
				}
			}
			transform = newTransform;
			AddComponent(transform);
			return newTransform;
		}

		internal T ReplaceTransform<T>(bool keepWorldSpace = true) where T : Transform, new()
		{
			T newTransform = System.Activator.CreateInstance<T>();
			if (transform != null)
			{
				if (keepWorldSpace)
				{
					newTransform.Copy(transform);
				}
				transform.DestroyImmediate();
			}
			transform = newTransform;
			AddComponent(newTransform);
			return newTransform as T;
		}

		/// <summary>
		/// Finalise will loop all the components attached to the <see cref="CosmosEngine.GameObject"/> and remove any that is null.
		/// </summary>
		internal void Finalise()
		{
			return; // ??
			foreach (Component component in components)
			{
				if (component.Expired)
				{
					components.IsDirty = true;
					break;
				}
			}
			if (components.IsDirty)
			{
				components.RemoveAll(item => item.Expired);
				ModifiedEvent?.Invoke(GameObjectChange.ComponentStructure);
				components.IsDirty = false;
			}
		}

		/// <summary>
		/// Activates or Deactivates the <see cref="CosmosEngine.GameObject"/>, depending on the given <see langword="true"/> or <see langword="false"/> <paramref name="value"/>.
		/// </summary>
		/// <param name="value"></param>
		public void SetActive(bool value)
		{
			Enabled = value;
		}

		#region Behaviour Methods

		protected override void OnEnable()
		{
			foreach (Component component in components)
			{
				if (!component.Enabled)
				{
					component.Invoke("OnEnable");
				}
			}
		}

		protected override void OnDisable()
		{
			foreach (Component component in components)
			{
				if (component.Enabled)
				{
					component.Invoke("OnDisable");
				}
			}
		}

		protected override void OnDestroy()
		{
			Finalise();
			foreach (Component component in components)
			{
				Destroy(component);
			}
			transform.DestroyImmediate();
			components.Clear();
		}

		#endregion

		#region Add and Get Component

		/// <summary>
		/// Adds a <see cref="CosmosEngine.Component"/> of type <typeparamref name="T"/> to the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of the component to add.</typeparam>
		/// <returns></returns>
		public override T AddComponent<T>() => AddComponent<T>(null);

		/// <summary>
		/// Adds a <see cref="CosmosEngine.Component"/> of type <paramref name="componentType"/> to the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <param name="componentType">The type of the component to add.</param>
		/// <returns></returns>
		public override Component AddComponent(Type componentType) => AddComponent(componentType, null);

		/// <summary>
		/// Adds a <see cref="CosmosEngine.Component"/> of type <typeparamref name="T"/> to the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of the component to add.</typeparam>
		/// <param name="parameters">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="parameters"/> is an empty array or null, the parameterless constructor is invoked.</param>
		/// <returns></returns>
		public T AddComponent<T>(params object?[]? parameters) where T : Component
		{
			T component = (T)AddComponent(typeof(T), parameters);
			return component;
		}

		/// <summary>
		/// Adds a <see cref="CosmosEngine.Component"/> of type <paramref name="componentType"/> to the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <param name="componentType">The type of the component to add.</param>
		/// <param name="parameters">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="parameters"/> is an empty array or null, the parameterless constructor is invoked.</param>
		/// <returns></returns>
		public Component AddComponent(Type componentType, params object?[]? parameters)
		{
			if (parameters != null)
			{
				bool validConstructor = false;
				ConstructorInfo[] constructors = componentType.GetConstructors((BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				foreach (ConstructorInfo info in constructors)
				{
					if (info.GetParameters().Length == parameters.Length)
					{
						validConstructor = true;
						break;
					}
				}
				if(!validConstructor)
				{
					throw new TargetParameterCountException($"Mismatch constructor argument amount, {componentType.FullName} has constructor matching [{parameters.Length}] {parameters.ParametersTypeToString()}.");
				}
			}

			Component component = Activator.CreateInstance(componentType, parameters) as Component;
			RequireComponent requireComponent = component.GetType().GetCustomAttribute<RequireComponent>(true);
			if (requireComponent != null)
			{
				foreach (Type c in requireComponent.RequiredComponents)
				{
					if (!GetComponent(c))
						AddComponent(Activator.CreateInstance(c) as Component);
				}
			}
			AddComponent(component);
			return component;
		}

		/// <summary>
		/// Adds the <paramref name="component"/> instance to the <see cref="CosmosEngine.GameObject"/> and initializes it.
		/// </summary>
		internal void AddComponent(Component component)
		{
			if (component == null || component.Expired)
				return;
			if (component is Transform)
				components.Insert(0, component);
			else
				components.Add(component);
			component.AssignGameObject(this);
			component.Initialize();
			ModifiedEvent?.Invoke(GameObjectChange.ComponentStructure);
		}

		/// <summary>
		/// Returns the first <see cref="CosmosEngine.Component"/> instance of Type <typeparamref name="T"/>, if the <see cref="CosmosEngine.GameObject"/> has one attached, <see langword="null"/> if it doesn't.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		public override T GetComponent<T>()
		{
			Finalise();
			T? component = default;
			component = components.Find(GetComponentMatch<T>()) as T;
			return component;
		}

		/// <summary>
		/// Returns the first <see cref="CosmosEngine.Component"/> instance of <paramref name="componentType"/>, if the <see cref="CosmosEngine.GameObject"/> has one attached, <see langword="null"/> if it doesn't.
		/// </summary>
		/// <param name="componentType">The type of <see cref="CosmosEngine.Component"/> to retrieve.</param>
		public override Component GetComponent(Type componentType)
		{
			Finalise();
			Component component = null;
			component = components.Find(GetComponentMatch(componentType));
			return component;
		}

		/// <summary>
		/// Returns the first <see cref="CosmosEngine.Component"/> instance of Type <typeparamref name="T"/>, if the <see cref="CosmosEngine.GameObject"/> has one attached, or adds an instance of <typeparamref name="T"/> to the <see cref="CosmosEngine.GameObject"/> if it doesn't.
		/// </summary>
		/// <typeparam name="T">The type to retrieve or add.</typeparam>
		/// <returns></returns>
		public T GetOrAddComponent<T>() where T : Component
		{
			Finalise();
			T component = GetComponent<T>();
			if (component == null)
			{
				return AddComponent<T>();
			}
			return component;
		}

		/// <summary>
		/// Returns the first <see cref="CosmosEngine.Component"/> instance of <paramref name="componentType"/>, if the <see cref="CosmosEngine.GameObject"/> has one attached, or adds an instance of <typeparamref name="T"/> to the <see cref="CosmosEngine.GameObject"/> if it doesn't.
		/// </summary>
		/// <param name="componentType">The type to retrieve or add.</param>
		/// <returns></returns>
		public Component GetOrAddComponent(Type componentType)
		{
			Finalise();
			Component component = GetComponent(componentType);
			if (component == null)
			{
				return AddComponent(componentType);
			}
			return component;
		}

		/// <summary>
		/// Returns all <see cref="CosmosEngine.Component"/> instances of Type <typeparamref name="T"/> on the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <typeparam name="T">The type to retrieve.</typeparam>
		/// <returns></returns>
		public override T[] GetComponents<T>()
		{
			Finalise();
			List<T> list = new List<T>();
			foreach (Component component in components.FindAll(GetComponentMatch<T>()))
			{
				if (component == null)
					continue;
				list.Add(component as T);
			}
			return list.ToArray();
		}

		/// <summary>
		/// Returns all <see cref="CosmosEngine.Component"/> instances of <paramref name="componentType"/> on the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <param name="componentType">The type to retrieve.</param>
		/// <returns></returns>
		public override Component[] GetComponents(Type componentType)
		{
			Finalise();
			List<Component> list = new List<Component>();
			foreach (Component component in components.FindAll(GetComponentMatch(componentType)))
			{
				if (component == null)
					continue;
				list.Add(component);
			}
			return list.ToArray();
		}

		/// <summary>
		/// Returns all <see cref="CosmosEngine.Component"/>s on the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <returns></returns>
		internal Component[] GetComponentsAll()
		{
			Finalise();
			return components.ToArray();
		}

		private Predicate<Component> GetComponentMatch<T>()
		{
			return t => t.GetType().IsAssignableTo(typeof(T))
				&& !t.Expired;
		}

		private Predicate<Component> GetComponentMatch(Type componentType)
		{
			return t => t.GetType().IsAssignableTo(componentType)
				&& !t.Expired;
		}

		#endregion

		#region Find
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Modules.ObjectManager.Find(string)"/>
		/// </summary>
		public static GameObject Find(string name) => ObjectManager.Find(name);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Modules.ObjectManager.FindGameObjectWithTag(string)"/>
		/// </summary>
		public static GameObject FindGameObjectWithTag(string tag) => ObjectManager.FindGameObjectWithTag(tag);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Modules.ObjectManager.FindGameObjectsWithTag(string)"/>
		/// </summary>
		public static GameObject[] FindGameObjectsWithTag(string tag) => ObjectManager.FindGameObjectsWithTag(tag);
		/// <summary>
		/// Returns true if this <see cref="CosmosEngine.GameObject"/> is tagged with <paramref name="tag"/>.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public bool CompareTag(string tag)
		{
			foreach(string t in this.tag)
			{
				if(t.Equals(tag))
					return true;
			}
			return false;
		}

		public void SetTag(string tag)
		{
			if (string.IsNullOrEmpty(tag))
				return;
			if (!this.tag.Contains(tag))
				this.tag.Add(tag);
		}

		public void RemoveTag(string tag)
		{
			if (string.IsNullOrEmpty(tag))
				return;
				this.tag.Remove(tag);
		}

		#endregion

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.SendMessage(string, SendMessageOption, object[])"/>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		public void SendMessage(string methodName) => SendMessage(methodName, SendMessageOption.RequireReceiver);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.GameObject.SendMessage(string, SendMessageOption, object[])"/>
		/// </summary>
		/// <param name="methodName">The name of the method to call.</param>
		/// <param name="option">Should an error be raised if the method doesn't exist on the target object?</param>
		public void SendMessage(string methodName, SendMessageOption option) => SendMessage(methodName, option, Array.Empty<object>());
		/// <summary>
		/// Invokes the method named <paramref name="methodName"/> on every <see cref="CosmosEngine.GameBehaviour"/> on the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <param name="methodName">	The name of the method to call.</param>
		/// <param name="option">Should an error be raised if the method doesn't exist on the target object?</param>
		/// <param name="args">An optional parameter value to pass to the called method.</param>
		public void SendMessage(string methodName, SendMessageOption option, params object[] args)
		{
			bool recived = false;
			foreach (Component comp in this.components)
			{
				if (comp is GameBehaviour)
				{
					bool v = comp.Invoke(methodName, args);
					recived = v || recived;
				}
			}
			if(!recived && option == SendMessageOption.RequireReceiver)
			{
				throw new Exception("SendMessageOption requires a receiver for the given message.");
				return;
			}
		}

		public string ToInformationString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append($"{ToString()} [{(Enabled ? "Enabled" : "Disabled")}]");
			sb.Append(" {");
			for(int i = 0; i < components.Count; i++)
			{
				if (i > 0)
					sb.Append(", ");
				sb.Append(components[i].GetType().Name);
				sb.Append($"[{(components[i].ActiveSelf ? "E" : "D")}]");
			}
			sb.Append("}");
			return sb.ToString();
		}
		#endregion

		public static Component operator +(GameObject go, Type type)
		{
			Component component = go.AddComponent(type);
			return component;
		}
	}
}