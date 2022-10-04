
using CosmosEngine.Modules;
using System.Reflection;

namespace CosmosEngine.CoreModule
{
	/// <summary>
	/// Base class for all objects that can be derived from. Gives access to a lot of functionality that is automatically invoked through <see cref="CosmosEngine.CoreModule.Core"/>.
	/// </summary>
	public abstract class Object : Resource, IObject
	{
		#region Fields
		private string name;
		private bool initialized = false;
		private bool awake = false;
		private bool enabled;
		private bool expired = false;
		private bool destroyOnLoad = true;
		private float timeToDestroy = 0.0f;
		private bool markedForDestruction = false;

		/// <summary>
		/// The name of this object.
		/// </summary>
		public virtual string Name { get => name; set => name = value; }
		/// <summary>
		/// Returns if this <see cref="CosmosEngine.CoreModule.Object"/> has invoked the <see cref="CosmosEngine.CoreModule.Object.Awake"/> method. 
		/// </summary>
		internal bool IsAwake => awake;
		/// <summary>
		/// The enabled state of this object. Disabling an object will suppress a lot of functionality like Start() and Update(). Methods like OnEnable and OnDisable are invoked when the object goes from one state to another.
		/// </summary>
		public virtual bool Enabled
		{
			get => enabled;
			set
			{
				if (IsAwake)
				{
					if (Enabled && !value)
					{
						//We are enabled and we are disabling the object.
						OnDisable();
					}
					else if (!Enabled && value)
					{
						//We are disabled and we are enabling the object.
						OnEnable();
					}
				}
				enabled = value;
			}
		}
		/// <summary>
		/// The local enabled state of the object.
		/// </summary>
		public virtual bool ActiveSelf => enabled;
		/// <summary>
		/// Returns the alive / dead state of any Object, if the object has expired it means it's no longer part of the game loop and all references should be nullified when possible, to allow for Garbage Collection. An Object can only be remove by using <see cref="CosmosEngine.CoreModule.Object.Destroy(Object)"/> or <see cref="CosmosEngine.CoreModule.Object.Destroy(Object, float)"/> for a delayed destruction.
		/// </summary>
		public bool Expired { get => expired; private set => expired = value; }
		/// <summary>
		/// The time before this <see cref="CosmosEngine.CoreModule.Object"/> is destroyed.
		/// </summary>
		internal float TimeToDestroy { get => timeToDestroy; set => timeToDestroy = value; }
		/// <summary>
		/// This <see cref="CosmosEngine.CoreModule.Object"/> has been marked for destruction using <see cref="CosmosEngine.CoreModule.Object.Destroy(Object)"/> and should be removed from the game loop and the end of the frame as long as <see cref="CosmosEngine.CoreModule.Object.TimeToDestroy"/> is equals or less than 0f.
		/// </summary>
		internal bool MarkedForDestruction => markedForDestruction;
		public virtual bool DestroyOnLoad { get => destroyOnLoad; set => destroyOnLoad = value; }
		#endregion

		~Object() => Debug.Log($"Collecting Object: {this} for garbage collection.", LogFormat.Complete);

		public Object()
		{
			enabled = true;
		}

		#region Initial Methods
		internal void InvokeAwake()
		{
			if (awake)
				return;
			awake = true;
			Awake();
		}
		/// <summary>
		/// Awake is invoked when the script instance is being loaded even if the object is disabled.
		/// </summary>
		protected virtual void Awake()
		{

		}

		/// <summary>
		/// Invoked right before the <see cref="CosmosEngine.CoreModule.Object"/> is instantiated, this will be invoked before any other methods is invoked. The use of <see cref="CosmosEngine.Transform"/> and other <see cref="CosmosEngine.GameBehaviour"/> methods may be limited. Be thoughtful about the use of this method.
		/// </summary>
		protected virtual void OnInstantiated() { }

		#endregion

		#region Conditional Methods
		/// <summary>
		/// This method is invoked when the object becomes enabled and active.
		/// </summary>
		protected virtual void OnEnable()
		{

		}

		/// <summary>
		/// This method is invoked when the behaviour becomes disabled.
		/// </summary>
		protected virtual void OnDisable()
		{

		}

		/// <summary>
		/// Destroying an Object will disable it before destroing it. If the Object is enabled, OnDisable will be invoked before OnDestroy.
		/// </summary>
		protected virtual void OnDestroy()
		{

		}

		#endregion

		#region Methods

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed && disposing)
			{
				if (!Expired)
					MarkForDestruction();
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Initializes and instantiates the <see cref="CosmosEngine.CoreModule.Object"/> assigning it to the correct modules through the <see cref="CosmosEngine.Modules.ObjectDelegater"/>.
		/// </summary>
		/// <returns>Returns if the initialization was successful or not.</returns>
		internal virtual bool Initialize()
		{
			if (initialized)
				return false;
			initialized = true;
			OnInstantiated();
			return ObjectDelegater.OnInstantiate(this);
		}

		/// <summary>
		/// Invokes the method <paramref name="methodName"/>.
		/// </summary>
		/// <param name="methodName"></param>
		public bool Invoke(string methodName) => Invoke(methodName, System.Array.Empty<object>());

#nullable enable
		/// <summary>
		/// Invokes the method <paramref name="methodName"/> with <paramref name="parameter"/>.
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool Invoke(string methodName, object?[]? parameter)
		{
			MethodInfo? method = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if(method == null)
			{
				Debug.Log($"No method named {methodName} exists on {GetType().FullName}.", LogFormat.Error);
				return false;
			}
			else
			{
				if (parameter != null)
				{
					if (method.GetParameters().Length != parameter.Length)
					{
						Debug.Log($"No method named {methodName} match parameters {parameter.ParametersTypeToString()} on {GetType().FullName}.", LogFormat.Error);
						return false;
					}
				}
				method.Invoke(this, parameter);
				return true;
			}
		}
#nullable disable

		/// <summary>
		///  
		/// </summary>
		/// <param name="t"></param>
		internal virtual void MarkForDestruction(float t = 0.0f)
		{
			if (markedForDestruction)
				return;
			timeToDestroy = t;
			markedForDestruction = true;
		}

		/// <summary>
		/// Destroys this Object immediately. You are strongly recommended to use Destroy instead.
		/// </summary>
		/// <param name="t"></param>
		public void DestroyImmediate()
		{
			Enabled = false;
			OnDestroy();
			Expired = true;
			Dispose();
		}

		public override string ToString()
		{
			return $"{Name} [{GetType().Name}]";
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Removes a game object, component or asset from the game loop. If object is a <see cref="CosmosEngine.Component"/>, this method removes the component from the <see cref="CosmosEngine.GameObject"/> and destroys it. If object is a <see cref="CosmosEngine.GameObject"/>, it destroys the GameObject, all components attached to it will also destroy all <see cref="CosmosEngine.Transform"/> children of the <see cref="CosmosEngine.GameObject"/>.
		/// </summary>
		/// <param name="objectToDestroy">	The object to destroy.</param>
		public static void Destroy(Object objectToDestroy) => Destroy(objectToDestroy, 0f);

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.CoreModule.Object.Destroy(Object)"/>
		/// </summary>
		/// <param name="objectToDestroy">	The object to destroy.</param>
		/// <param name="t">The optional amount of time to delay before destroying the object.</param>
		public static void Destroy(Object objectToDestroy, float t)
		{
			objectToDestroy.MarkForDestruction(t);
		}

		/// <summary>
		/// The <see cref="CosmosEngine.CoreModule.Object"/> not to destroy when clearing all objects.
		/// </summary>
		/// <param name="target"></param>
		public static void DontDestroyOnLoad(Object target)
		{
			target.destroyOnLoad = false;
		}

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.CoreModule.Object.Instantiate{T}(GameObject, Vector2, float)}"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="original">An existing object that you want to make a copy of.</param>
		/// <returns></returns>
		public static T Instantiate<T>(GameObject original) where T : Object, new() => Instantiate<T>(original, Vector2.Zero);

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.CoreModule.Object.Instantiate{T}(GameObject, Vector2, float)}"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="original">An existing object that you want to make a copy of.</param>
		/// <param name="position">Position for the new object.</param>
		/// <returns></returns>
		public static T Instantiate<T>(GameObject original, Vector2 position) where T : Object, new() => Instantiate<T>(original, position, 0.0f);

		/// <summary>
		/// Clones the object <paramref name="original"/> and returns the clone. If you are cloning a Component the GameObject it is attached to is also cloned with an optional <paramref name="position"/> and <paramref name="rotation"/>. After cloning an object you can also use GetComponent to set properties on a specific component attached to the cloned object. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="original">An existing object that you want to make a copy of.</param>
		/// <param name="position">Position for the new object.</param>
		/// <param name="rotation">Orientation of the new object.</param>
		/// <returns>The instantiated clone.</returns>
		public static T Instantiate<T>(GameObject original, Vector2 position, float rotation) where T : Object, new()
		{
			T clone = Prefab.Create<T>(original);
			if (clone is Behaviour b)
			{
				b.Transform.Position = position;
				b.Transform.Rotation = rotation;
			}
			return clone;
		}

		#region FindObjectOfType
		/// <summary>
		/// Returns the first loaded, active and enabled object of a type.
		/// </summary>
		/// <returns></returns>
		public static T FindObjectOfType<T>() where T : Object
		{
			return FindObjectOfType<T>(false);
		}

		/// <summary>
		/// Returns the first loaded object of a type, can include inactive (not enabled).
		/// </summary>
		/// <returns></returns>
		public static T FindObjectOfType<T>(bool includeInactive) where T : Object
		{
			return ObjectManager.FindObjectOfType<T>(includeInactive);
		}

		/// <summary>
		/// Returns a list of all loaded, active and enabled objects of a type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return FindObjectsOfType<T>(false);
		}

		/// <summary>
		/// Returns a list of all loaded objects of a type, can include inactive (not enabled).
		/// </summary>
		/// <returns></returns>
		public static T[] FindObjectsOfType<T>(bool includeInactive) where T : Object
		{
			return ObjectManager.FindObjectsOfType<T>(includeInactive);
		}

		/// <summary>
		/// Returns a list of all loaded, active and enbaled objects.
		/// </summary>
		/// <returns></returns>
		public static Object[] FindObjectsOfAll()
		{
			return FindObjectsOfAll(false);
		}

		/// <summary>
		/// Returns a list of all loaded objects, can include inactive (not enabled).
		/// </summary>
		/// <param name="includeInactive"></param>
		/// <returns></returns>
		public static Object[] FindObjectsOfAll(bool includeInactive)
		{
			return ObjectManager.FindObjectsOfAll(includeInactive);
		}
		#endregion

		#endregion

		#region Operators

		public static implicit operator bool(Object obj) => !object.ReferenceEquals(obj, null);
		#endregion
	}
}