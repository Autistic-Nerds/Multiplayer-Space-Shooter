
namespace CosmosEngine.CoreModule
{
	/// <summary>
	/// Base class for a game feature that has continuously and conditional functionality that is invoked within the game. A <see cref="CosmosEngine.CoreModule.Behaviour"/> is also a reference to a <see cref="CosmosEngine.SceneManagement.Scene"/> object, that will always have a <see cref="CosmosEngine.Transform"/> attached to it, such as <see cref="CosmosEngine.GameObject"/> and <see cref="CosmosEngine.Component"/>.
	/// </summary>
	public abstract class Behaviour : Object, IBehaviour
	{
		private bool started = false;
		public abstract Transform Transform { get; }
		/// <summary>
		/// Returns if the <see cref="CosmosEngine.CoreModule.Behaviour"/> has invoked the <see cref="CosmosEngine.CoreModule.Behaviour.Start"/> method.
		/// </summary>
		internal bool Started => started;

		#region Initial Methods
		internal void InvokeStart()
		{
			if (started)
				return;
			started = true;
			Start();
		}
		/// <summary>
		/// Start is invoked before the first update frame when the script is enabled for the first time, just before any of the Continuous methods are called the first time.
		/// </summary>
		protected virtual void Start()
		{

		}

		#endregion

		#region Continuous Methods
		/// <summary>
		/// Update is invoked every frame, if the Object is enabled.
		/// </summary>
		protected virtual void Update()
		{

		}

		/// <summary>
		/// LateUpdate is invoked every frame, if the Object is enabled. LateUpdate is invoked after all Update methods have been invoked.
		/// </summary>
		protected virtual void LateUpdate()
		{

		}

		/// <summary>
		/// Implementation of OnDrawGizmos allows for drawing debug only shapes and figures that are not visible when running the game in release. Use <see cref="CosmosEngine.Gizmos"/> or <see cref="CosmosEngine.Rendering.Draw"/> to render the desired shapes.
		/// </summary>
		protected virtual void OnDrawGizmos()
		{

		}

		#endregion

		#region Public Methods
		public abstract T AddComponent<T>() where T : Component;
		public abstract Component AddComponent(System.Type componentType);
#nullable enable
		public abstract T? GetComponent<T>() where T : class;
		public abstract Component? GetComponent(System.Type componentType);
		public abstract T[] GetComponents<T>() where T : class;
		public abstract Component[] GetComponents(System.Type componentType);
#nullable disable

		#endregion
	}
}