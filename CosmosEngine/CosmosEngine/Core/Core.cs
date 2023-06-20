
using CosmosEngine.InputModule;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CosmosEngine.Modules;

namespace CosmosEngine.CoreModule
{
	internal sealed class Core : Microsoft.Xna.Framework.Game
	{
		#region Fields
		private static Core instance;
		private readonly GraphicsDeviceManager graphics;
		private readonly Game gameController;
		private static bool initialStart;
		private static bool applicationIsRunning = false;

		private SpriteBatch spriteBatch;
		private readonly List<IModule> gameModules = new List<IModule>();

		private Stopwatch updateThreadSW;
		private Stopwatch renderThreadSW;

		public static Core Instance => instance;
		internal static Viewport Viewport => Instance.GraphicsDevice.Viewport;
		internal static Vector2Int ViewportSize => new Vector2Int(Viewport.Width, Viewport.Height);
		internal static bool IsFullScreen => instance.graphics.IsFullScreen;
		internal static GraphicsDeviceManager GraphicsDeviceManager => instance.graphics;
		internal static GameTime GameTime { get; private set; }
		internal static GameTime VariableGameTime { get; private set; }
		public static SpriteBatch SpriteBatch => Instance.spriteBatch;
		internal static Microsoft.Xna.Framework.Content.ContentManager ContentManager => Instance.Content;
		internal static double MainThreadTime { get; private set; }
		internal static double RenderThreadTime { get; private set; }
		internal static bool ApplicationIsRunning => applicationIsRunning;
		internal static bool WindowInFocus => Instance.IsActive;
		#endregion

		public Core(Game game)
		{
			instance = this;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			this.gameController = game;
		}

		protected override void Initialize()
		{
			graphics.PreferredBackBufferWidth = 854; // Screen.Width;
			graphics.PreferredBackBufferHeight = 480; // Screen.Height;
			graphics.IsFullScreen = false;

			IsFixedTimeStep = false;
			graphics.SynchronizeWithVerticalRetrace = true;
			TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 120);
			InactiveSleepTime = new TimeSpan(0);
			graphics.PreferMultiSampling = true;

			graphics.ApplyChanges();
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowClientSizeChanged);

			//Add desired Game Manager Systems
			List<IModule> modules = new List<IModule>();
			modules.AddRange(gameController.GameModules);
			modules.Sort((a, b) => b.ExecutionOrder.CompareTo(a.ExecutionOrder));
			foreach(IModule module in modules)
			{
				AddGameSystem(module);
			}
			applicationIsRunning = true;
			SystemsInitialization();
#if EDITOR
			updateThreadSW = Stopwatch.StartNew();
			renderThreadSW = Stopwatch.StartNew();
#endif
			base.Initialize();
		}

		public void AddGameSystem<T>(T manager) where T : IModule => gameModules.Add(manager);
		public void AddGameSystem<T>() where T : IModule, new() => gameModules.Add(Activator.CreateInstance<T>() as IModule);

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		#region Update / Draw

		protected override void Update(GameTime gameTime)
		{
#if EDITOR
			updateThreadSW.Restart();
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
				CloseApplication();
#endif
			GameTime = gameTime;
			GameSystemUpdate();

#if EDITOR
			updateThreadSW.Stop();
			MainThreadTime = updateThreadSW.Elapsed.TotalMilliseconds;
#endif
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
#if EDITOR
			renderThreadSW.Restart();
#endif
			if (Camera.Main != null)
			{
				GraphicsDevice.Clear(gameController.BackgroundColour);
				Camera main = Camera.Main;
				SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.NonPremultiplied, samplerState: SamplerState.LinearClamp, transformMatrix: main.ViewMatrix);
				GameSystemRender();
				SpriteBatch.End();
			}
			else
			{
				GraphicsDevice.Clear(Color.Transparent);
			}

			SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.LinearWrap);
			GameSystemUI();
			SpriteBatch.End();

#if EDITOR
			renderThreadSW.Stop();
			RenderThreadTime = renderThreadSW.Elapsed.TotalMilliseconds;
#endif

			base.Draw(gameTime);
		}
#endregion

		#region Application

		internal void SetResolution(int width, int height, bool fullScreenMode)
		{
			graphics.PreferredBackBufferWidth = width; // Screen.Width;
			graphics.PreferredBackBufferHeight = height; // Screen.Height;
			graphics.IsFullScreen = fullScreenMode;
			graphics.ApplyChanges();
		}

		private void WindowClientSizeChanged(object sender, EventArgs e)
		{
			graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
			graphics.ApplyChanges();
			Screen.OnScreenSizeChanged.Invoke();
		}

		public static void StartApplication(Game game)
		{
			using (Core core = new Core(game))
				core.Run();
		}

		internal void CloseApplication()
		{
			applicationIsRunning = false;
			//gameController.Shutdown();
			this.Exit();
		}

	#endregion

		#region Game Systems
		private void SystemsInitialization()
		{
			foreach (IModule system in gameModules)
			{
				system.Initialize();
			}
		}

		private void GameSystemStart()
		{
			foreach(IModule system in gameModules)
			{
				if(system is IStartModule start)
				{
					start.Start();
				}
			}
		}

		private void GameSystemUpdate()
		{
			if (!initialStart)
			{
				GameSystemStart();
				initialStart = true;
			}

			Rendering.Draw.Colour = Colour.White;
			InputState.Update();
			Input.Update(); //<--- Should be made into a Game System.
			foreach (IModule system in gameModules)
			{
				if (system is IUpdateModule update)
				{
					update.Update();
				}
			}
		}

		private void GameSystemRender()
		{
			foreach (IModule system in gameModules)
			{
				if (system is IRenderModule renderer)
				{
					renderer.RenderWorld();
				}
			}
		}

		private void GameSystemUI()
		{
			foreach (IModule system in gameModules)
			{
				if (system is IRenderModule ui)
				{
					ui.RenderUI();
				}
			}
		}
	#endregion

	}
}
