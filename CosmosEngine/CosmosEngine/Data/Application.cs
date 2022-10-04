
using CosmosEngine.CoreModule;

namespace CosmosEngine
{
	public static class Application
	{
		private static bool applicatedIsPaused;

		public static bool IsRunning => Core.ApplicationIsRunning;
		public static bool IsPaused { get => applicatedIsPaused; internal set => applicatedIsPaused = value; }
		public static bool IsFocused => Core.WindowInFocus;

		public static void CloseApplication()
		{
			Core.Instance.CloseApplication();
		}
	}
}