
namespace CosmosEngine.Modules
{
	/// <summary>
	/// Game Modules are what defines how the game functions, modules can only be loaded at the beginning of the game. If any module is not nessecary it can simply be left out of the game and will in not way consume performance.
	/// </summary>
	/// <typeparam name="TModule">The module <typeparamref name="TModule"/>.</typeparam>
	public abstract class GameModule<TModule> : BaseModule where TModule : GameModule<TModule>
	{
		private static TModule instance;
		public static TModule Instance => instance;
		internal static bool Exist => Instance != null && Instance.SystemExist() && !instance.IsDisposed;
		public override void Initialize() 
		{
			if(instance != null)
			{
				//Debug.Log($"Multiple modules of {typeof(T)} cannot be added.", LogFormat.Error, LogOption.NoStacktrace);
				Dispose(true);
				return;
			}
			instance = (TModule)this;
		}
		public virtual bool SystemExist()
		{
			if (instance == null)
			{
				Debug.Log($"Trying to use module {typeof(TModule)} while it does not exist. Remember to add all required modules before starting the game application.", LogFormat.Error, LogOption.NoStacktrace);
				return false;
			}
			return true;
		}
	}
}