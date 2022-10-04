
using Microsoft.Xna.Framework.Input;

namespace CosmosEngine.InputModule
{
	internal static class InputExtension
	{
		public static bool IsButtonUp(this MouseState mouse, MouseButton input)
		{
			switch(input)
			{
				case MouseButton.Left:
					return mouse.LeftButton == ButtonState.Released;
				case MouseButton.Right:
					return mouse.RightButton == ButtonState.Released;
				case MouseButton.Middle:
					return mouse.MiddleButton == ButtonState.Released;
				case MouseButton.ThumbButton1:
					return mouse.XButton1 == ButtonState.Released;
				case MouseButton.ThumbButton2:
					return mouse.XButton2 == ButtonState.Released;
			}
			return false;
		}

		public static bool IsButtonDown(this MouseState mouse, MouseButton input)
		{
			switch (input)
			{
				case MouseButton.Left:
					return mouse.LeftButton == ButtonState.Pressed;
				case MouseButton.Right:
					return mouse.RightButton == ButtonState.Pressed;
				case MouseButton.Middle:
					return mouse.MiddleButton == ButtonState.Pressed;
				case MouseButton.ThumbButton1:
					return mouse.XButton1 == ButtonState.Pressed;
				case MouseButton.ThumbButton2:
					return mouse.XButton2 == ButtonState.Pressed;
			}
			return false;
		}

		public static Microsoft.Xna.Framework.Input.Keys Convert(this CosmosEngine.InputModule.Keys value)
		{
			return (Microsoft.Xna.Framework.Input.Keys)(int)value;
		}

		public static CosmosEngine.InputModule.Keys Convert(this Microsoft.Xna.Framework.Input.Keys value)
		{
			return (CosmosEngine.InputModule.Keys)(int)value;
		}

		public static Microsoft.Xna.Framework.Input.Buttons Convert(this CosmosEngine.InputModule.GamepadButton value)
		{
			return (Microsoft.Xna.Framework.Input.Buttons)(int)value;
		}

		public static CosmosEngine.InputModule.GamepadButton Convert(this Microsoft.Xna.Framework.Input.Buttons value)
		{
			return (CosmosEngine.InputModule.GamepadButton)(int)value;
		}
	}
}
