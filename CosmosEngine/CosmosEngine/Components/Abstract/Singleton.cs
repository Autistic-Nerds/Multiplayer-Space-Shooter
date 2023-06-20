#nullable enable
namespace CosmosEngine
{
	/// <summary>
	/// A Singleton <see cref="CosmosEngine.Component"/>, only one instance of <typeparamref name="T"/> can exist at any given time. If a new is instantiated while it already exists, the new one will immediately be destroyed. 
	/// <para><see cref="CosmosEngine.Singleton{T}"/> does not automatically instantiate an instance, the component must be added to a <see cref="CosmosEngine.GameObject"/> before accessing it, otherwise <see langword="null"/> will be returned.</para>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Singleton<T> : Component where T : Singleton<T>
	{
		private static T? instance = null;
		public static T? Instance => instance;

		protected override void OnInstantiated()
		{
			if(instance != null && instance != this)
			{
				DestroyImmediate();
				return;
			}
			instance = (T)this;
		}

		protected override void OnDestroy()
		{
			// release reference on destruction
			if (instance == this)
				instance = null;
			base.OnDestroy();
		}
	}
}