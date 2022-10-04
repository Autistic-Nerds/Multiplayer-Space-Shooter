
using CosmosEngine.Modules;
using System;
using System.Collections.Generic;

namespace CosmosEngine.CoreModule
{
	public abstract class Game : GameModule<Game>, IStartModule, IUpdateModule
	{
		private static Game game = null;
		private static readonly List<IModule> gameModules = new List<IModule>();
		private Colour backgroundColour = Colour.CornflowerBlue;

		internal List<IModule> GameModules => gameModules;
		public Colour BackgroundColour { get => backgroundColour; set => backgroundColour = value; }
		public static Microsoft.Xna.Framework.Content.ContentManager ContentManager => Core.ContentManager;

		/// <summary>
		/// Initialize is invoked only once, before the first frame and before start. Instantiation should not occur within Initialize, use Start instead.
		/// </summary>
		public override abstract void Initialize();
		[System.Obsolete("The Load Content method is obsolete as content should be implemented using Eager Loading pattern. The method is still required for the Game to work but might be replaced in a further version.", false)]
		/// <summary>
		/// Use AddContentLoader(IContentLoader) to load content files in this method.
		/// </summary>
		public virtual void LoadContent()
		{

		}
		/// <summary>
		/// Start is invoked only once, before the first frame update.
		/// </summary>
		public abstract void Start();
		/// <summary>
		/// Update is called once per frame.
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// Creates a new <typeparamref name="T"/> from where instantiation of <see cref="CosmosEngine.GameObject"/> can occur and the initial Game Logic. Only a single instance of <see cref="CosmosEngine.CoreModule.Game"/> will be executed at any time. Creating a <see langword="new"/> <see cref="CosmosEngine.CoreModule.Game"/> will add the essential <see cref="CosmosEngine.Modules.GameModule"/> components. Further functionality can be accessed by adding additional <see cref="CosmosEngine.Modules.GameModule"/> components, use <see cref="CosmosEngine.CoreModule.Game.AddModule{T}"/>, some modules will only exist while the game is in EDITOR mode. After <see cref="CosmosEngine.CoreModule.Game.LaunchApplication{T}"/> has been invoked no further <see cref="CosmosEngine.Modules.GameModule"/> components can be added or removed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Game Create<T>() where T : Game, new()
		{
			gameModules.Clear();
			game = new T();

			game.AddEssential();
			gameModules.Add(game);
#if EDITOR
			game.AddEditor();
#endif
			return game;
		}

		private Game AddEssential()
		{
			game.AddModule<ObjectManager>();
			game.AddModule<BehaviourManager>();
			game.AddModule<RenderManager>();
			return this;
		}

		/// <summary>
		/// Adds default <see cref="CosmosEngine.Modules.GameModule"/> components to the <see cref="CosmosEngine.CoreModule.Game"/>.
		/// </summary>
		/// <returns></returns>
		public Game AddDefault()
		{
			AddModule<CoroutineManager>();
			AddModule<InputManager>();
			AddModule<InputSystem>();
			AddModule<EventManager>();
			AddModule<AudioSystem>();

			return this;
		}

		public Game AddEditor()
		{
			game.AddModule<Debug>();
			game.AddModule<GizmosModule>();
			game.AddModule<EditorGrid>();
			game.AddModule<EditorStats>();

			return this;
		}

		/// <summary>
		/// Adds the <see cref="CosmosEngine.Modules.GameModule"/> <typeparamref name="T"/> to the <see cref="CosmosEngine.CoreModule.Game"/>, multiple modules of the same type cannot be added. After <see cref="CosmosEngine.CoreModule.Game.LaunchApplication{T}"/> has been invoked no further <see cref="CosmosEngine.Modules.GameModule"/> components can be added or removed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Game AddModule<T>() where T : IModule => AddModule(Activator.CreateInstance<T>());

		/// <summary>
		/// Adds an instance of a <see cref="CosmosEngine.Modules.GameModule"/> to the game, will check for duplicates.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="module"></param>
		/// <returns></returns>
		internal Game AddModule<T>(T module) where T : IModule
		{
			if (Core.ApplicationIsRunning)
			{
				//Debug.Log($"Not possible to add game modules once the game has been launched, make sure to add all required modules before running Game.LaunchApplication().", LogFormat.Error);
				return this;
			}
			if (gameModules.Exists(item => item.GetType() == typeof(T)))
			{
				//Debug.Log($"Not possible to add multiple game modules of the same type ({typeof(T).Name}).", LogFormat.Warning);
			}
			else
			{
				gameModules.Add(module);
			}
			return this;
		}

		/// <summary>
		/// Removes the <see cref="CosmosEngine.Modules.GameModule"/> <typeparamref name="T"/> from the <see cref="CosmosEngine.CoreModule.Game"/>, essential modules cannot be removed. After <see cref="CosmosEngine.CoreModule.Game.LaunchApplication{T}"/> has been invoked no further <see cref="CosmosEngine.Modules.GameModule"/> components can be added or removed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Game RemoveModule<T>() where T : IModule
		{
			if(Core.ApplicationIsRunning)
			{
				//Debug.Log($"Not possible to remove game modules once the game has been launched.", LogFormat.Error);
				return this;
			}
			if (gameModules.Exists(item => item.GetType() == typeof(T)))
			{
				gameModules.RemoveAll(item => item.GetType() == typeof(T));
			}
			else
			{
				//Debug.Log($"Not possible to remove game module {typeof(T).Name} since it does not exist.", LogFormat.Warning);
			}
			return this;
		}

		/// <summary>
		/// Launches the <see cref="CosmosEngine.CoreModule.Game"/>.
		/// </summary>
		public static void StartApplication()
		{
			using (game)
			{
				Core.StartApplication(game);
			}
		}

		/// <summary>
		/// Use <see cref="CosmosEngine.CoreModule.Game.LaunchApplication"/> as the final augment to start the application.
		/// </summary>
		public void LaunchApplication()
		{
			StartApplication();
		}

		public static void ExitApplication()
		{
			Core.Instance.CloseApplication();
		}

		public void CloseApplication()
		{
			ExitApplication();
		}
	}
}