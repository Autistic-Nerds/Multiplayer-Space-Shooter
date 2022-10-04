
using CosmosEngine.CoreModule;

namespace CosmosEngine
{
	public static class Time
	{
		private static float timeScale;
		static Time()
		{
			timeScale = 1.0f;
		}
		/// <summary>
		/// The scale at which time passes.
		/// </summary>
		public static float TimeScale 
		{ 
			get => timeScale * (Application.IsPaused ? 0 : 1); 
			set
			{
				if(value < 0)
				{
					Debug.QuickLog($"Trying to set Time.TimeScale to {value}, Time.TimeScale cannot be negative.", LogFormat.Warning);
					return;	
				}
				timeScale = value;
			}
		}
		/// <summary>
		/// The interval in seconds from the last frame to the current one.
		/// </summary>
		public static float DeltaTime => Core.GameTime != null && (float)Core.GameTime.ElapsedGameTime.TotalSeconds > 0 ? (float)Core.GameTime.ElapsedGameTime.TotalSeconds * TimeScale : 1f;
		/// <summary>
		/// The TimeScale-independent interval in seconds from the last frame to the current one.
		/// </summary>
		public static float UnscaledDeltaTime => Core.GameTime != null && (float)Core.GameTime.ElapsedGameTime.TotalSeconds > 0 ? (float)Core.GameTime.ElapsedGameTime.TotalSeconds : 1f;
		/// <summary>
		/// The time at the beginning of this frame.
		/// </summary>
		public static float ElapsedTime => Core.GameTime != null ? (float)Core.GameTime.TotalGameTime.TotalSeconds : 1f;
	}
}
