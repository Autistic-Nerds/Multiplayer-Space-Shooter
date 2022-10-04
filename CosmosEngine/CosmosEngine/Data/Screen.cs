
using Microsoft.Xna.Framework.Graphics;
using CosmosEngine.CoreModule;
using System;

namespace CosmosEngine
{
	public static class Screen
	{
		private static int previousWidth;
		private static int previousHeight;
		private static event Action onScreenSizeChanged = delegate { };
		/// <summary>
		/// Event invoked if the display window size is changed.
		/// </summary>
		public static Action OnScreenSizeChanged { get => onScreenSizeChanged; set => onScreenSizeChanged = value; }

		/// <summary>
		/// Get or set the application window width.
		/// </summary>
		public static int Width
		{
			get => Core.ViewportSize.X;
			set => Core.Instance.SetResolution(value, Height, FullScreenMode);
		}


		/// <summary>
		/// Get or set the application window height.
		/// </summary>
		public static int Height
		{
			get => Core.ViewportSize.Y;
			set => Core.Instance.SetResolution(Width, value, FullScreenMode);
		}
		/// <summary>
		/// Returns the pixel centre of the screen.
		/// </summary>
		public static Vector2 Centre => new Vector2(Width / 2, Height / 2);
		/// <summary>
		/// Returns the ratio of the screen Width / Height.
		/// </summary>
		public static float Ratio => (float)Screen.Width / (float)Screen.Height;

		/// <summary>
		/// Returns the screen width.
		/// </summary>
		public static int DisplayWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		/// <summary>
		/// Returns the screen height.
		/// </summary>
		public static int DisplayHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		public static Vector2Int Resolution
		{
			get => new Vector2Int(Width, Height);
			set => Core.Instance.SetResolution(value.X, value.Y, FullScreenMode);
		}
		public static bool FullScreenMode
		{
			get => Core.IsFullScreen;
			set => Core.Instance.SetResolution(Width, Height, value);
		}
		public static void ChangeFullScreenMode()
		{
			previousWidth = Width;
			previousHeight = Height;
			if (FullScreenMode)
			{
				SetResolution(previousWidth, previousHeight, false);
			}
			else
			{
				SetResolution(DisplayWidth, DisplayHeight, true);
			}
		}
		public static void SetResolution(int width, int height, bool fullScreen)
		{
			Core.Instance.SetResolution(width, height, fullScreen);
		}
	}
}
