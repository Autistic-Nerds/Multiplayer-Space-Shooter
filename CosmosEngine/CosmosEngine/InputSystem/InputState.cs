
using CosmosEngine.InputModule;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using GamePadState = Microsoft.Xna.Framework.Input.GamePadState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;

namespace CosmosEngine
{
	/// <summary>
	/// The base system for input CosmosEngine input control, <see cref="CosmosEngine.InputState"/> can be used to handle the most essential user input from static keys, mouse or gamepad. It is a static class that always persist within the system and can't be removed as the other input modules can. It is not recommended to use the <see cref="CosmosEngine.InputState"/> for user input. Instead consider one of the alternative systems that allow for custom user inputs.
	/// <list type="table">
	/// <item><see cref="CosmosEngine.InputManager"/> a string based system with predefined buttons and axis with room for user defined input, allow for buttons / axis control and multiple keys for a single input.</item>
	/// <item><see cref="CosmosEngine.InputSystem"/> an event based system that comes with no predefined inputs - requires more setup, with the exchange of invoking methods on inputs and allowing for custom return types.</item>
	/// </list>
	/// </summary>
	public static class InputState
	{
		private static bool anyKeyPressed;
		private static Vector2 mouseScrollWheelDelta;
		private static Vector2 mouseScrollWheel;
		private static Vector2 mousePosition;
		private static Vector2 mouseDelta;

		private static KeyboardState keyboardState, previousKeyboardState;
		private static MouseState mouseState, previousMouseState;
		private static GamePadState gamepadState, previousGamepadState;

		public static bool AnyKey => anyKeyPressed;
		public static Vector2 MousePosition => mousePosition;
		public static Vector2 MouseDelta => mouseDelta;
		public static Vector2 MouseScrollWheel => mouseScrollWheel;
		public static Vector2 MouseScrollWheelDelta => mouseScrollWheelDelta;
		internal static KeyboardState KeyboardState => keyboardState;
		internal static KeyboardState PreviousKeyboardState => previousKeyboardState;

		public static void Update()
		{
			previousKeyboardState = keyboardState;
			previousMouseState = mouseState;
			previousGamepadState = gamepadState;

			keyboardState = Keyboard.GetState();
			mouseState = Mouse.GetState();
			gamepadState = GamePad.GetState(0);

			AnyKeyPressed();
			CompareMouseState();
		}

		private static void AnyKeyPressed()
		{
			//Are any keyboard keys pressed?
			int keysPressed = keyboardState.GetPressedKeyCount();
			if (keysPressed > 0)
			{
				anyKeyPressed = true;
			}
			else
			{
				//Are any mouse button pressed?
				if (Held(MouseButton.Left) ||
					Held(MouseButton.Right) ||
					Held(MouseButton.Middle) ||
					Held(MouseButton.ThumbButton1) ||
					Held(MouseButton.ThumbButton2))
				{
					anyKeyPressed = true;
				}
				else
					anyKeyPressed = false;
				//If not sey anyKey to false.
			}
		}

		private static void CompareMouseState()
		{
			mouseScrollWheelDelta = Vector2.Zero;
			if (previousMouseState.ScrollWheelValue != mouseState.ScrollWheelValue)
			{
				mouseScrollWheelDelta.Y = mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
				mouseScrollWheel.Y = mouseState.ScrollWheelValue;
			}
			if (previousMouseState.HorizontalScrollWheelValue != mouseState.HorizontalScrollWheelValue)
			{
				mouseScrollWheelDelta.X = mouseState.HorizontalScrollWheelValue - previousMouseState.HorizontalScrollWheelValue;
				mouseScrollWheel.X = mouseState.HorizontalScrollWheelValue;
			}

			mouseDelta = Vector2.Zero;
			if (previousMouseState.Position != mouseState.Position)
			{
				mouseDelta = (Vector2)(mouseState.Position - previousMouseState.Position).ToVector2();
				mousePosition = (Vector2)mouseState.Position.ToVector2();
			}
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Pressed</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		public static bool Pressed(Keys input)
		{
			return previousKeyboardState.IsKeyUp(input.Convert()) && keyboardState.IsKeyDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Pressed</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		public static bool Pressed(MouseButton input)
		{
			return previousMouseState.IsButtonUp(input) && mouseState.IsButtonDown(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Pressed</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		public static bool Pressed(GamepadButton input)
		{
			return previousGamepadState.IsButtonUp(input.Convert()) && gamepadState.IsButtonDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Released</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		public static bool Released(Keys input)
		{
			return previousKeyboardState.IsKeyDown(input.Convert()) && keyboardState.IsKeyUp(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Released</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		public static bool Released(MouseButton input)
		{
			return previousMouseState.IsButtonDown(input) && mouseState.IsButtonUp(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Released</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		public static bool Released(GamepadButton input)
		{
			return previousGamepadState.IsButtonDown(input.Convert()) && gamepadState.IsButtonUp(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Hold</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		public static bool Held(Keys input)
		{
			return previousKeyboardState.IsKeyDown(input.Convert()) && keyboardState.IsKeyDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Hold</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		public static bool Held(MouseButton input)
		{
			return previousMouseState.IsButtonDown(input) && mouseState.IsButtonDown(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Hold</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		public static bool Held(GamepadButton input)
		{
			return previousGamepadState.IsButtonDown(input.Convert()) && gamepadState.IsButtonDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Any</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		public static bool Check(Keys input)
		{
			return Pressed(input) || Released(input) || Held(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Any</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		public static bool Check(MouseButton input)
		{
			return Pressed(input) || Released(input) || Held(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if <strong>Any</strong> condition is met on the current frame for <paramref name="input"/>.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		public static bool Check(GamepadButton input)
		{
			return Pressed(input) || Released(input) || Held(input);
		}
	}
}