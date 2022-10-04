
using CosmosEngine.CoreModule;

namespace CosmosEngine.Modules
{
	public sealed class EditorStats : EditorModule<EditorStats>, IStartModule, IUpdateModule
	{
		private float delta;
		private LogOption logOption;

		public EditorStats()
		{
			ExecutionOrder = 100;
		}

		public override void Initialize()
		{
			base.Initialize();
			logOption = Debug.DefaultOption | LogOption.IgnoreCallCount | LogOption.CompareInitialCallOnly;
		}

		public void Start()
		{
			LogThreadTime();
			LogFPS();
		}

		public void Update()
		{
			if (delta < Time.ElapsedTime)
			{
				LogThreadTime();
				LogFPS();
				delta = Time.ElapsedTime + 0.1f;
			}
		}

		private void LogFPS()
		{
			int fps = (int)(1f / Time.UnscaledDeltaTime);
			Debug.Log($"FPS: {fps} - {Time.UnscaledDeltaTime:F3} {(Core.GameTime.IsRunningSlowly ? "(Running Slow)" : "")}", (Core.GameTime.IsRunningSlowly) ? LogFormat.Warning : LogFormat.Complete, logOption);
		}

		private void LogThreadTime()
		{
			LogOption logOption = Debug.DefaultOption | LogOption.IgnoreCallCount | LogOption.CompareInitialCallOnly;
			Debug.Log($"Update Task: {Core.MainThreadTime:F2}ms --- Render Task: {Core.RenderThreadTime:F2}ms", LogFormat.Message, logOption);
		}
	}
}