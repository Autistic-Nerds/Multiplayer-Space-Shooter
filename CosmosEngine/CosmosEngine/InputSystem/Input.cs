
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using CosmosEngine.EventSystems;

namespace CosmosEngine.InputModule
{
	[System.Obsolete("This input system is actually the newer Input System, but it will be replaced by a new Input System using the Game Module System. It's possible to use either, both or none InputManager and Input(System)", false)]
	public static class Input
	{
		private const int MaxInputActions = ushort.MaxValue;
		private static bool isUpdating;
		private static readonly Dictionary<uint, InputAction> inputControlActions = new Dictionary<uint, InputAction>();
		private static readonly Dictionary<uint, InputAction> itemsToAdd = new Dictionary<uint, InputAction>();
		private static readonly List<uint> removedInputActions = new List<uint>();

		private static KeyboardState keyboardState, previousKeyboardState;
		private static MouseState mouseState, previousMouseState;
		private static GamePadState gamepadState, previousGamepadState;

		private static int mouseScrollValue;

		private static Event<Vector2> onMousePositionChanged = new Event<Vector2>();
		private static Event<float> onMouseScrollChanged = new Event<float>();

		internal static Dictionary<uint, InputAction> InputControlActions => inputControlActions;
		public static Vector2 MousePosition => (Vector2)mouseState.Position.ToVector2();
		public static Vector2 MouseWorldPosition => Camera.Main.ScreenToWorld(MousePosition);
		public static int MouseScrollValue => mouseScrollValue;
		public static Vector2 GamepadLeftThumbStick => (Vector2)gamepadState.ThumbSticks.Left;
		public static Vector2 GamepadRightThumbStick => (Vector2)gamepadState.ThumbSticks.Right;

		public static Event<Vector2> MousePositionChangedEvent => onMousePositionChanged;
		public static Event<float> MouseScrollChangedEvent => onMouseScrollChanged;

		//User can define multiple InputActions, an input action calls a specific method.
		//Each InputAction contains one or more InputControl.
		//InputManager will loop over all InputActions and look if any of them contains a InputControl with one a key that is pressed.
		//If InputManager matches a pair it will call set its InputActionPhase first frame to Started then Performed afterwards
		//When it no longer matches it will change it to Canceled and then back to Waiting until then.
		//InputAction also holds Action<CallbackContext> that will be invoked whenever a state changes.
		//Started, Performed, Canceled
		//InputActions are instantiated with a unique key, this way the user can access it very easy, should create a custom enum for this key.

		#region Add Input Action

		public static InputAction CreateInputAction(uint key, string name, params InputControl[] control)
		{
			InputAction input = new InputAction(name, control);
			if (!isUpdating)
			{
				InsertInputAction(key, input);
			}
			else
			{
				itemsToAdd.Add(key, input);
			}
			return input;
		}

		/// <summary>
		/// Add an InputAction with a given control scheme, can return a unique object value otherwise it will return a bool.
		/// </summary>
		/// <param name="key">The key is unique indentifier to modify and remove the Input Action, the key cannot not overpass 65535, values above are reserved and may cause error and issues. If a Input Action with the same <paramref name="key"/> is created it will overwrite the existing Input Action.goo A custom Enum could be created to make sure the same key is not used multiple times.</param>
		/// <param name="name">The name of this input action, used for decorative and debug purpose.</param>
		/// <param name="control">The input controls that deteminers when this action is invoked.</param>
		/// <param name="started">Method invoked when input started (pressed). Parameter with CallbackContext is required.</param>
		/// <returns>The created <see cref="CosmosEngine.InputModule.InputAction"/>.</returns>
		public static InputAction AddInputAction(uint key, string name, Action<CallbackContext> started = null, params InputControl[] control) => AddInputAction(key, name, started, null, control);

		/// <summary>
		/// <inheritdoc cref="AddInputAction(uint, string, Action, InputControl[])"/>
		/// </summary>
		/// <param name="key">The key is unique indentifier to modify and remove the Input Action, the key cannot not overpass 65535, values above are reserved and may cause error and issues. If a Input Action with the same <paramref name="key"/> is created it will overwrite the existing Input Action.goo A custom Enum could be created to make sure the same key is not used multiple times.</param>
		/// <param name="name">The name of this input action, used for decorative and debug purpose.</param>
		/// <param name="control">The input controls that deteminers when this action is invoked.</param>
		/// <param name="started">Method invoked when input started (pressed).</param>
		/// <returns>The created <see cref="CosmosEngine.InputModule.InputAction"/>.</returns>
		public static InputAction AddInputAction(uint key, string name, Action started, params InputControl[] control) => AddInputAction(key, name, started, null, null, control);

		/// <summary>
		/// <inheritdoc cref="AddInputAction(uint, string, Action, InputControl[])"/>
		/// </summary>
		/// <param name="key">The key is unique indentifier to modify and remove the Input Action, the key cannot not overpass 65535, values above are reserved and may cause error and issues. If a Input Action with the same <paramref name="key"/> is created it will overwrite the existing Input Action.goo A custom Enum could be created to make sure the same key is not used multiple times.</param>
		/// <param name="name">The name of this input action, used for decorative and debug purpose.</param>
		/// <param name="control">The input controls that deteminers when this action is invoked.</param>
		/// <param name="started">Method invoked when input started (pressed). Parameter with CallbackContext is required.</param>
		/// <param name="canceled">Method invoked when input canceled (released). Parameter with CallbackContext is required.</param>
		/// <returns>The created <see cref="CosmosEngine.InputModule.InputAction"/>.</returns>
		public static InputAction AddInputAction(uint key, string name, Action<CallbackContext> started = null, Action<CallbackContext> canceled = null, params InputControl[] control) => AddInputAction(key, name, started, null, canceled, control);

		/// <summary>
		/// <inheritdoc cref="AddInputAction(uint, string, Action, InputControl[])"/>
		/// </summary>
		/// <param name="key">The key is unique indentifier to modify and remove the Input Action, the key cannot not overpass 65535, values above are reserved and may cause error and issues. If a Input Action with the same <paramref name="key"/> is created it will overwrite the existing Input Action.goo A custom Enum could be created to make sure the same key is not used multiple times.</param>
		/// <param name="name">The name of this input action, used for decorative and debug purpose.</param>
		/// <param name="control">The input controls that deteminers when this action is invoked.</param>
		/// <param name="started">Method invoked when input started (pressed). Parameter with CallbackContext is required.</param>
		/// <param name="canceled">Method invoked when input canceled (released). Parameter with CallbackContext is required.</param>
		/// <returns>The created <see cref="CosmosEngine.InputModule.InputAction"/>.</returns>
		public static InputAction AddInputAction(uint key, string name, Action started = null, Action canceled = null, params InputControl[] control) => AddInputAction(key, name, started, null, canceled, control);
		/// <summary>
		/// <inheritdoc cref="AddInputAction(uint, string, Action, InputControl[])"/>
		/// </summary>
		/// <param name="key">The key is unique indentifier to modify and remove the Input Action, the key cannot not overpass 65535, values above are reserved and may cause error and issues. If a Input Action with the same <paramref name="key"/> is created it will overwrite the existing Input Action.goo A custom Enum could be created to make sure the same key is not used multiple times.</param>
		/// <param name="name">The name of this input action, used for decorative and debug purpose.</param>
		/// <param name="control">The input controls that deteminers when this action is invoked.</param>
		/// <param name="started">Method invoked when input started (pressed).</param>
		/// <param name="performed">Method invoked when input performed (held).</param>
		/// <param name="canceled">Method invoked when input canceled (released).</param>
		/// <returns>The created <see cref="CosmosEngine.InputModule.InputAction"/>.</returns>
		/// <returns></returns>
		public static InputAction AddInputAction(uint key, string name, Action started = null, Action performed = null, Action canceled = null, params InputControl[] control)
		{
			InputAction input = new InputAction(name, control);
			if (started != null)
				input.OnStarted.Add(started);
			if (performed != null)
				input.OnPerformed.Add(performed);
			if (canceled != null)
				input.OnCanceled.Add(canceled);
			//We don't want to add anything to our list while we're updating, this can cause erroes
			//If we're updating add to a temporary list and add them later.
			if (!isUpdating)
			{
				InsertInputAction(key, input);
			}
			else
			{
				itemsToAdd.Add(key, input);
			}
			return input;
		}

		/// <summary>
		/// <inheritdoc cref="AddInputAction(uint, string, Action, InputControl[])"/>
		/// </summary>
		/// <param name="key">The key is unique indentifier to modify and remove the Input Action, the key cannot not overpass 65535, values above are reserved and may cause error and issues. If a Input Action with the same <paramref name="key"/> is created it will overwrite the existing Input Action.goo A custom Enum could be created to make sure the same key is not used multiple times.</param>
		/// <param name="name">The name of this input action, used for decorative and debug purpose.</param>
		/// <param name="control">The input controls that deteminers when this action is invoked.</param>
		/// <param name="started">Method invoked when input started (pressed). Parameter with CallbackContext is required.</param>
		/// <param name="performed">Method invoked when input performed (held). Parameter with CallbackContext is required.</param>
		/// <param name="canceled">Method invoked when input canceled (released). Parameter with CallbackContext is required.</param>
		/// <returns>The created <see cref="CosmosEngine.InputModule.InputAction"/>.</returns>
		public static InputAction AddInputAction(uint key, string name, Action<CallbackContext> started = null, Action<CallbackContext> performed = null, Action<CallbackContext> canceled = null, params InputControl[] control)
		{
			InputAction input = new InputAction(name, control);
			if (started != null)
				input.OnStarted.Add(started);
			if (performed != null)
				input.OnPerformed.Add(performed);
			if (canceled != null)
				input.OnCanceled.Add(canceled);

			//We don't want to add anything to our list while we're updating, this can cause erroes
			//If we're updating add to a temporary list and add them later.
			if (!isUpdating)
			{
				InsertInputAction(key, input);
			}
			else
			{
				itemsToAdd.Add(key, input);
			}
			return input;
		}
		#endregion

		public static void RemoveInputAction(uint key)
		{
			if(!isUpdating)
			{
				ClearInputAction(key);
			}
			else
			{
				removedInputActions.Add(key);
			}
		}

		private static void ClearInputAction(uint key)
		{
			inputControlActions.Remove(key);
		}

		internal static void InsertInputAction(uint key, InputAction action)
		{
			if (InputControlActions.ContainsKey(key))
			{
				InputControlActions[key] = action;
				//Possibly make a modifying possibility? Instead of overwrite.
				//InputControlActions[key].AddInputControl(action.Controls.ToArray());
			}
			else
			{
				InputControlActions.Add(key, action);
			}
		}

		internal static void Update()
		{
			isUpdating = true;
			previousKeyboardState = keyboardState;
			previousMouseState = mouseState;
			previousGamepadState = gamepadState;

			keyboardState = Keyboard.GetState();
			mouseState = Mouse.GetState();
			gamepadState = GamePad.GetState(PlayerIndex.One);

			foreach (InputAction action in InputControlActions.Values)
			{
				if(action == null)
				{
					continue;
				}
				action.CheckState();
			}

			if (previousMouseState.ScrollWheelValue != mouseState.ScrollWheelValue)
			{
				mouseScrollValue = mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
				MouseScrollChangedEvent?.Invoke(mouseScrollValue);
			}

			if (previousMouseState.Position != mouseState.Position)
			{
				MousePositionChangedEvent?.Invoke(MousePosition);
			}

			isUpdating = false;

			foreach (KeyValuePair<uint, InputAction> entry in itemsToAdd)
			{
				InsertInputAction(entry.Key, entry.Value);
			}
			itemsToAdd.Clear();

			foreach (uint key in removedInputActions)
			{
				ClearInputAction(key);
			}
			removedInputActions.Clear();
		}

		/// <summary>
		/// Returns <see langword="true"/> if Released input condition is met.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		internal static bool WasPressed(Keys input)
		{
			return previousKeyboardState.IsKeyUp(input.Convert()) && keyboardState.IsKeyDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if Released input condition is met.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		internal static bool WasPressed(MouseButton input)
		{
			return previousMouseState.IsButtonUp(input) && mouseState.IsButtonDown(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if Pressed input condition is met.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		internal static bool WasPressed(GamepadButton input)
		{
			return previousGamepadState.IsButtonUp(input.Convert()) && gamepadState.IsButtonDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if Released input condition is met.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		internal static bool WasReleased(Keys input)
		{
			return previousKeyboardState.IsKeyDown(input.Convert()) && keyboardState.IsKeyUp(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if Released input condition is met.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		internal static bool WasReleased(MouseButton input)
		{
			return previousMouseState.IsButtonDown(input) && mouseState.IsButtonUp(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if Released input condition is met.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		internal static bool WasReleased(GamepadButton input)
		{
			return previousGamepadState.IsButtonDown(input.Convert()) && gamepadState.IsButtonUp(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if Hold input condition is met.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		internal static bool IsHeld(Keys input)
		{
			return previousKeyboardState.IsKeyDown(input.Convert()) && keyboardState.IsKeyDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> if Hold input condition is met.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		internal static bool IsHeld(MouseButton input)
		{
			return previousMouseState.IsButtonDown(input) && mouseState.IsButtonDown(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> if Hold input condition is met.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		internal static bool IsHeld(GamepadButton input)
		{
			return previousGamepadState.IsButtonDown(input.Convert()) && gamepadState.IsButtonDown(input.Convert());
		}

		/// <summary>
		/// Returns <see langword="true"/> is any input condition is met [Pressed, Released, Hold]
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <returns></returns>
		internal static bool CheckButton(Keys input)
		{
			return WasPressed(input) || WasReleased(input) || IsHeld(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> is any input condition is met [Pressed, Released, Hold]
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <returns></returns>
		internal static bool CheckButton(MouseButton input)
		{
			return WasPressed(input) || WasReleased(input) || IsHeld(input);
		}

		/// <summary>
		/// Returns <see langword="true"/> is any input condition is met [Pressed, Released, Hold]
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <returns></returns>
		internal static bool CheckButton(GamepadButton input)
		{
			return WasPressed(input) || WasReleased(input) || IsHeld(input);
		}

		/// <summary>
		/// Returns an InputActionPhase according to the limited Interaction.
		/// </summary>
		/// <param name="input">Keyboard Key</param>
		/// <param name="interaction"></param>
		/// <returns></returns>
		internal static InputActionPhase[] CheckButton(Keys input, Interaction interaction, out bool result)
		{
			result = false;
			InputActionPhase[] results = null;
			switch (interaction)
			{
				case Interaction.All:
					if (CheckButton(input))
					{
						results = new InputActionPhase[]
						{
						WasPressed(input) ? InputActionPhase.Started : InputActionPhase.Disabled,
						IsHeld(input) ? InputActionPhase.Performed : InputActionPhase.Disabled,
						WasReleased(input) ? InputActionPhase.Canceled : InputActionPhase.Disabled
						};
						result = true;
					}
					break;
				//case Interaction.PressAndRelease:
				//	if (WasPressed(input) || WasReleased(input))
				//	{
				//		results = new InputActionPhase[]
				//		{
				//		WasPressed(input) ? InputActionPhase.Started : InputActionPhase.Disabled,
				//		WasReleased(input) ? InputActionPhase.Canceled : InputActionPhase.Disabled
				//		};
				//		result = true;
				//	}
				//	break;
				case Interaction.Press:
					if (WasPressed(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Started };
						result = true;
					}
					break;
				case Interaction.Release:
					if (WasReleased(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Canceled };
						result = true;
					}
					break;
				case Interaction.Hold:
					if (IsHeld(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Performed };
						result = true;
					}
					break;
			}
			return results;
		}
		/// <summary>
		/// Returns an InputActionPhase according to the limited Interaction.
		/// </summary>
		/// <param name="input">Mouse Button</param>
		/// <param name="interaction"></param>
		/// <returns></returns>
		internal static InputActionPhase[] CheckButton(MouseButton input, Interaction interaction, out bool result)
		{
			result = false;
			InputActionPhase[] results = null;
			switch (interaction)
			{
				case Interaction.All:
					if (CheckButton(input))
					{
						results = new InputActionPhase[]
						{
						WasPressed(input) ? InputActionPhase.Started : InputActionPhase.Disabled,
						IsHeld(input) ? InputActionPhase.Performed : InputActionPhase.Disabled,
						WasReleased(input) ? InputActionPhase.Canceled : InputActionPhase.Disabled
						};
						result = true;
					}
					break;
				//case Interaction.PressAndRelease:
				//	if (WasPressed(input) || WasReleased(input))
				//	{
				//		results = new InputActionPhase[]
				//		{
				//		WasPressed(input) ? InputActionPhase.Started : InputActionPhase.Disabled,
				//		WasReleased(input) ? InputActionPhase.Canceled : InputActionPhase.Disabled
				//		};
				//		result = true;
				//	}
				//	break;
				case Interaction.Press:
					if (WasPressed(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Started };
						result = true;
					}
					break;
				case Interaction.Release:
					if (WasReleased(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Canceled };
						result = true;
					}
					break;
				case Interaction.Hold:
					if (IsHeld(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Performed };
						result = true;
					}
					break;
			}
			return results;
		}
		/// <summary>
		/// Returns an InputActionPhase according to the limited Interaction.
		/// </summary>
		/// <param name="input">Gamepad Button</param>
		/// <param name="interaction"></param>
		/// <returns></returns>
		internal static InputActionPhase[] CheckButton(GamepadButton input, Interaction interaction, out bool result)
		{
			result = false;
			InputActionPhase[] results = null;
			switch (interaction)
			{
				case Interaction.All:
					if (CheckButton(input))
					{
						results = new InputActionPhase[]
						{
						WasPressed(input) ? InputActionPhase.Started : InputActionPhase.Disabled,
						IsHeld(input) ? InputActionPhase.Performed : InputActionPhase.Disabled,
						WasReleased(input) ? InputActionPhase.Canceled : InputActionPhase.Disabled
						};
						result = true;
					}
					break;
				//case Interaction.PressAndRelease:
				//	if (WasPressed(input) || WasReleased(input))
				//	{
				//		results = new InputActionPhase[]
				//		{
				//		WasPressed(input) ? InputActionPhase.Started : InputActionPhase.Disabled,
				//		WasReleased(input) ? InputActionPhase.Canceled : InputActionPhase.Disabled
				//		};
				//		result = true;
				//	}
				//	break;
				case Interaction.Press:
					if (WasPressed(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Started };
						result = true;
					}
					break;
				case Interaction.Release:
					if (WasReleased(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Canceled };
						result = true;
					}
					break;
				case Interaction.Hold:
					if (IsHeld(input))
					{
						results = new InputActionPhase[] { InputActionPhase.Performed };
						result = true;
					}
					break;
			}
			return results;
		}
	}
}
