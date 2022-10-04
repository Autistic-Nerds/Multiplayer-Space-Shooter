
using System.Collections.Generic;
using CosmosEngine.InputModule;
using CosmosEngine.Modules;

namespace CosmosEngine
{
	/// <summary>
	/// <see cref="CosmosEngine.InputManager"/> is a beginner friendly input system, than the more advanced <see cref="CosmosEngine.InputSystem"/>. With the benefit of predefined input buttons and less setup.
	/// </summary>
	public sealed class InputManager : GameModule<InputManager>
	{
		private static readonly Dictionary<string, InputButton> inputButtons = new Dictionary<string, InputButton>();

		/// <summary>
		/// Returns true if any key or mouse is currently held.
		/// </summary>
		public static bool AnyKey => InputState.AnyKey;
		/// <summary>
		/// The current mouse position in pixel coordinates.
		/// </summary>
		public static Vector2 MousePosition => InputState.MousePosition;
		/// <summary>
		/// The current mouse delta.
		/// </summary>
		public static Vector2 MouseDelta => InputState.MouseDelta;
		/// <summary>
		/// The current mouse scroll position.
		/// </summary>
		public static float MouseScrollPosition => InputState.MouseScrollWheel.Y;
		/// <summary>
		/// The current mouse scroll delta.
		/// </summary>
		public static float MouseScrollDelta => InputState.MouseScrollWheelDelta.Y;
		public static bool ActiveAndReady => (CoreModule.Core.WindowInFocus && Exist);

		public override void Initialize()
		{
			base.Initialize();
			CreateAxis("vertical", Keys.S, Keys.W);
			CreateAxis("vertical", Keys.Down, Keys.Up);
			CreateAxis("horizontal", Keys.D, Keys.A);
			CreateAxis("horizontal", Keys.Right, Keys.Left);

			CreateButton("mouse0", MouseButton.Left);
			CreateButton("mouse1", MouseButton.Right);
			CreateButton("mouse2", MouseButton.Middle);
			CreateButton("mouse3", MouseButton.ThumbButton1);
			CreateButton("mouse4", MouseButton.ThumbButton2);

			CreateButton("a", Keys.A);
			CreateButton("b", Keys.B);
			CreateButton("c", Keys.C);
			CreateButton("d", Keys.D);
			CreateButton("e", Keys.E);
			CreateButton("f", Keys.F);
			CreateButton("g", Keys.G);
			CreateButton("h", Keys.H);
			CreateButton("i", Keys.I);
			CreateButton("j", Keys.J);
			CreateButton("k", Keys.K);
			CreateButton("l", Keys.L);
			CreateButton("m", Keys.M);
			CreateButton("n", Keys.N);
			CreateButton("o", Keys.O);
			CreateButton("p", Keys.P);
			CreateButton("q", Keys.Q);
			CreateButton("r", Keys.R);
			CreateButton("s", Keys.S);
			CreateButton("t", Keys.T);
			CreateButton("u", Keys.U);
			CreateButton("v", Keys.V);
			CreateButton("w", Keys.W);
			CreateButton("x", Keys.X);
			CreateButton("y", Keys.Y);
			CreateButton("z", Keys.Z);

			CreateButton("up", Keys.Up);
			CreateButton("down", Keys.Down);
			CreateButton("left", Keys.Left);
			CreateButton("right", Keys.Right);

			CreateButton("escape", Keys.Escape);
			CreateButton("space", Keys.Space);
			CreateButton("back", Keys.Back);
			CreateButton("enter", Keys.Enter);
			CreateButton("ctrl", Keys.LeftControl);
			CreateButton("ctrl", Keys.RightControl);
			CreateButton("shift", Keys.LeftShift);
			CreateButton("shift", Keys.RightShift);
			CreateButton("delete", Keys.Delete);
			CreateButton("shift", Keys.LeftShift);

			CreateButton("f1", Keys.F1);
			CreateButton("f2", Keys.F2);
			CreateButton("f3", Keys.F3);
			CreateButton("f4", Keys.F4);
			CreateButton("f5", Keys.F5);
			CreateButton("f6", Keys.F6);
			CreateButton("f7", Keys.F7);
			CreateButton("f8", Keys.F8);
			CreateButton("f9", Keys.F9);
			CreateButton("f10", Keys.F10);
			CreateButton("f11", Keys.F11);
			CreateButton("f12", Keys.F12);

			CreateButton("plus", Keys.OemPlus);
			CreateButton("minus", Keys.OemMinus);
		}

		public static void CreateButton(string name, Keys button)
		{
			if (!Exist)
				return;
			Instance.AddInputButton(new InputButton(name.ToLower(), button));
		}

		public static void CreateButton(string name, MouseButton button)
		{
			if (!Exist)
				return;
			Instance.AddInputButton(new InputButton(name.ToLower(), button));
		}

		public static void CreateButton(string name, GamepadButton button)
		{
			if (!Exist)
				return;
			Instance.AddInputButton(new InputButton(name.ToLower(), button));
		}

		public static void CreateAxis(string name, Keys positive, Keys negative)
		{
			if (!Exist)
				return;
			Instance.AddInputButton(new InputButton(name.ToLower(), new Keys[] { positive }, new Keys[] { negative }));
		}

		public static void CreateAxis(string name, MouseButton positive, MouseButton negative)
		{
			if (!Exist)
				return;
			Instance.AddInputButton(new InputButton(name.ToLower(), new MouseButton[] { positive }, new MouseButton[] { negative }));
		}

		public static void CreateAxis(string name, GamepadButton positive, GamepadButton negative)
		{
			if (!Exist)
				return;
			Instance.AddInputButton(new InputButton(name.ToLower(), new GamepadButton[] { positive }, new GamepadButton[] { negative }));
		}

		private void AddInputButton(InputButton inputButton)
		{
			if (inputButtons.ContainsKey(inputButton.Name))
			{
				inputButtons[inputButton.Name] += inputButton;
			}
			else
			{
				inputButtons.Add(inputButton.Name, inputButton);
			}
		}

		/// <summary>
		/// Returns the <see langword="value"/> of the virtual axis identified by <paramref name="axis"/>.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public static float GetAxis(string axis)
		{
			if (!ActiveAndReady)
				return 0f;

			axis = axis.ToLower();
			if (inputButtons.ContainsKey(axis) && inputButtons[axis].IsAxis)
			{
				InputButton input = inputButtons[axis];
				float value = input.GetAxis(InputCondition.Held);
				return value;
			}
			//Input Button doesn't exist or is not an axis - Invoke an error.
			return 0f;
		}

		/// <summary>
		/// Returns the <see langword="value"/> of the virtual axis identified by <paramref name="axis"/> with no smoothing filtering applied.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public static float GetAxisRaw(string axis)
		{
			if (!ActiveAndReady)
				return 0f;

			axis = axis.ToLower();
			if (inputButtons.ContainsKey(axis) && inputButtons[axis].IsAxis)
			{
				InputButton input = inputButtons[axis];
				float value = input.GetAxis(InputCondition.Held);
				return value > 0 ? 1.0f : value < 0 ? -1.0f : 0.0f;
			}
			//Input Button doesn't exist or is not an axis - Invoke an error.
			return 0f;
		}

		/// <summary>
		/// Returns <see langword="true"/> while the virtual button identified by <paramref name="button"/> is held down.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static bool GetButton(string button)
		{
			if (!ActiveAndReady)
				return false;

			button = button.ToLower();
			if (inputButtons.ContainsKey(button) && !inputButtons[button].IsAxis)
			{
				InputButton input = inputButtons[button];
				return input.GetPositiveButton(InputCondition.Held);
			}
			//Input Button doesn't exist or is not a button - Invoke an error.
			return false;
		}

		/// <summary>
		/// Returns <see langword="true"/> during the frame the user pressed down the virtual button identified by <paramref name="button"/>.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static bool GetButtonDown(string button)
		{
			if (!ActiveAndReady)
				return false;

			button = button.ToLower();
			if (inputButtons.ContainsKey(button) && !inputButtons[button].IsAxis)
			{
				InputButton input = inputButtons[button];
				return input.GetPositiveButton(InputCondition.Pressed);
			}
			//Input Button doesn't exist or is not a button - Invoke an error.
			return false;
		}

		/// <summary>
		/// Returns <see langword="true"/> during the frame the user releases the virtual button identified by <paramref name="button"/>.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static bool GetButtonUp(string button)
		{
			if (!ActiveAndReady)
				return false;

			button = button.ToLower();
			if (inputButtons.ContainsKey(button) && !inputButtons[button].IsAxis)
			{
				InputButton input = inputButtons[button];
				return input.GetPositiveButton(InputCondition.Released);
			}
			//Input Button doesn't exist or is not a button - Invoke an error.
			return false;
		}

		/// <summary>
		/// Returns whether the given <paramref name="mouseButton"/> is held down.
		/// </summary>
		/// <param name="mouseButton"></param>
		/// <returns></returns>
		public static bool GetMouseButton(int mouseButton)
		{
			if (!ActiveAndReady)
				return false;
			return mouseButton switch
			{
				0 => InputState.Held(MouseButton.Left),
				1 => InputState.Held(MouseButton.Right),
				2 => InputState.Held(MouseButton.Middle),
				3 => InputState.Held(MouseButton.ThumbButton1),
				4 => InputState.Held(MouseButton.ThumbButton2),
				_ => false,
			};
		}
		/// <summary>
		/// Returns <see langword="true"/> during the frame the user pressed the given mouse <paramref name="mouseButton"/>.
		/// </summary>
		/// <param name="mouseButton"></param>
		/// <returns></returns>
		public static bool GetMouseButtonDown(int mouseButton)
		{
			if (!ActiveAndReady)
				return false;
			return mouseButton switch
			{
				0 => InputState.Pressed(MouseButton.Left),
				1 => InputState.Pressed(MouseButton.Right),
				2 => InputState.Pressed(MouseButton.Middle),
				3 => InputState.Pressed(MouseButton.ThumbButton1),
				4 => InputState.Pressed(MouseButton.ThumbButton2),
				_ => false,
			};
		}
		/// <summary>
		/// Returns <see langword="true"/> during the frame the user releases the given <paramref name="mouseButton"/>.
		/// </summary>
		/// <param name="mouseButton"></param>
		/// <returns></returns>
		public static bool GetMouseButtonUp(int mouseButton)
		{
			if (!ActiveAndReady)
				return false;
			return mouseButton switch
			{
				0 => InputState.Released(MouseButton.Left),
				1 => InputState.Released(MouseButton.Right),
				2 => InputState.Released(MouseButton.Middle),
				3 => InputState.Released(MouseButton.ThumbButton1),
				4 => InputState.Released(MouseButton.ThumbButton2),
				_ => false,
			};
		}
		/// <summary>
		/// Returns true while the user holds down the <paramref name="key"/> identified by the <see cref="CosmosEngine.InputModule.Keys"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool GetKey(Keys key)
		{
			if (!ActiveAndReady)
				return false;
			return InputState.Held(key);
		}

		/// <summary>
		/// Returns true during the frame the user starts pressing down the <paramref name="key"/> identified by the <see cref="CosmosEngine.InputModule.Keys"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool GetKeyDown(Keys key)
		{
			if (!ActiveAndReady)
				return false;
			return InputState.Pressed(key);
		}

		/// <summary>
		/// eturns true during the frame the user releases the <paramref name="key"/> identified by the <see cref="CosmosEngine.InputModule.Keys"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool GetKeyUp(Keys key)
		{
			if (!ActiveAndReady)
				return false;
			return InputState.Released(key);
		}
	}
}