
using System;
using System.Linq;

namespace CosmosEngine.InputModule
{
	public class InputButton
	{
		#region Fields
		private string name;
		private float value;
		private float sensitivity = 10.0f;
		private Keys[] positiveKeyInputs;
		private Keys[] negativeKeyInputs;
		private MouseButton[] positiveMouseInputs;
		private MouseButton[] negativeMouseInputs;
		private GamepadButton[] positiveGamepadInputs;
		private GamepadButton[] negativeGamepadInputs;
		private bool isAxis;

		public string Name { get => name; set => name = value; }
		public float Value { get => value; set => this.value = value; }
		public float Sensitivity { get => sensitivity; set => this.sensitivity = value; }
		public Keys[] PositiveKey => positiveKeyInputs;
		public Keys[] NegativeKey => negativeKeyInputs;
		public MouseButton[] PositiveMouse => positiveMouseInputs;
		public MouseButton[] NegativeMouse => negativeMouseInputs;
		public GamepadButton[] PositiveGamepad => positiveGamepadInputs;
		public GamepadButton[] NegativeGamepad => negativeGamepadInputs;
		internal bool KeyControlled => positiveKeyInputs != null;
		internal bool MouseControlled => positiveMouseInputs != null;
		internal bool GamepadControlled => positiveGamepadInputs != null;
		internal bool IsAxis => isAxis;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new <see cref="CosmosEngine.InputModule.InputButton"/> controlled by the keyboard.
		/// </summary>
		public InputButton(string name, params Keys[] input)
		{
			this.name = name;
			this.positiveKeyInputs = input;
			this.isAxis = false;
		}
		/// <summary>
		/// Creates a new <see cref="CosmosEngine.InputModule.InputButton"/> for boolean control.
		/// </summary>
		public InputButton(string name, Keys[] positiveInput, Keys[] negativeInput)
		{
			this.name = name;
			this.positiveKeyInputs = positiveInput;
			this.negativeKeyInputs = negativeInput;
			this.isAxis = true;
		}
		public InputButton(string name, params MouseButton[] input)
		{
			this.name = name;
			this.positiveMouseInputs = input;
			this.isAxis = false;
		}
		public InputButton(string name, MouseButton[] positiveInput, MouseButton[] negativeInput)
		{
			this.name = name;
			this.positiveMouseInputs = positiveInput;
			this.negativeMouseInputs = negativeInput;
			this.isAxis = true;
		}
		public InputButton(string name, params GamepadButton[] input)
		{
			this.name = name;
			this.positiveGamepadInputs = input;
			this.isAxis = false;
		}
		public InputButton(string name, GamepadButton[] positiveInput, GamepadButton[] negativeInput)
		{
			this.name = name;
			this.positiveGamepadInputs = positiveInput;
			this.negativeGamepadInputs = negativeInput;
			this.isAxis = true;
		}
		#endregion

		#region Public Methods

		public bool GetPositiveButton(InputCondition condition)
		{
			bool confirmedInput;
			if (positiveKeyInputs != null)
			{
				foreach (Keys input in positiveKeyInputs)
				{
					confirmedInput = condition switch
					{
						InputCondition.Pressed => InputState.Pressed(input),
						InputCondition.Held => InputState.Held(input),
						InputCondition.Released => InputState.Released(input),
					};
					if (confirmedInput)
						return true;
				}
			}
			if (positiveMouseInputs != null)
			{
				foreach (MouseButton input in positiveMouseInputs)
				{
					confirmedInput = condition switch
					{
						InputCondition.Pressed => InputState.Pressed(input),
						InputCondition.Held => InputState.Held(input),
						InputCondition.Released => InputState.Released(input),
					};
					if (confirmedInput)
						return true;
				}
			}
			if(positiveGamepadInputs != null)
			{
				foreach (GamepadButton input in positiveGamepadInputs)
				{
					confirmedInput = condition switch
					{
						InputCondition.Pressed => InputState.Pressed(input),
						InputCondition.Held => InputState.Held(input),
						InputCondition.Released => InputState.Released(input),
					};
					if (confirmedInput)
						return true;
				}
			}
			return false;
		}
		public bool GetNegativeButton(InputCondition condition)
		{
			bool confirmedInput;
			if (negativeKeyInputs != null)
			{
				foreach (Keys input in negativeKeyInputs)
				{
					confirmedInput = condition switch
					{
						InputCondition.Pressed => InputState.Pressed(input),
						InputCondition.Held => InputState.Held(input),
						InputCondition.Released => InputState.Released(input),
					};
					if (confirmedInput)
						return true;
				}
			}
			if (negativeMouseInputs != null)
			{
				foreach (MouseButton input in negativeMouseInputs)
				{
					confirmedInput = condition switch
					{
						InputCondition.Pressed => InputState.Pressed(input),
						InputCondition.Held => InputState.Held(input),
						InputCondition.Released => InputState.Released(input),
					};
					if (confirmedInput)
						return true;
				}
			}
			if (negativeGamepadInputs != null)
			{
				foreach (GamepadButton input in negativeGamepadInputs)
				{
					confirmedInput = condition switch
					{
						InputCondition.Pressed => InputState.Pressed(input),
						InputCondition.Held => InputState.Held(input),
						InputCondition.Released => InputState.Released(input),
					};
					if (confirmedInput)
						return true;
				}
			}
			return false;
		}
		public float GetAxis(InputCondition condition)
		{
			bool positive = GetPositiveButton(condition);
			bool negative = GetNegativeButton(condition);
			Debug.QuickLog($"{Name}: Positive: {positive} - Negative: {negative}");
			if(positive && negative)
			{
				return 0.0f;
				value = Mathf.MoveTowards(value, 0.0f, Sensitivity * Time.DeltaTime);
			}
			else if(positive)
			{
				return 1.0f;
				value = Mathf.MoveTowards(value, 1.0f, Sensitivity * Time.DeltaTime);
			}
			else if (negative)
			{
				return -1.0f;
				value = Mathf.MoveTowards(value, -1.0f, Sensitivity * Time.DeltaTime);
			}
			return 0.0f;
		}

		public void AddInputControls(Keys[] positiveInput)
		{
			if (positiveKeyInputs == null)
				positiveKeyInputs = Array.Empty<Keys>();
			positiveKeyInputs = positiveKeyInputs.Concat(positiveInput).ToArray();
		}
		public void AddInputControls(Keys[] positiveInput, Keys[] negativeInput)
		{
			AddInputControls(positiveInput);
			if (negativeKeyInputs == null)
				negativeKeyInputs = Array.Empty<Keys>();
			negativeKeyInputs = negativeKeyInputs.Concat(negativeInput).ToArray();
		}
		public void AddInputControls(MouseButton[] positiveInput)
		{
			if (positiveMouseInputs == null)
				positiveMouseInputs = Array.Empty<MouseButton>();
			positiveMouseInputs = positiveMouseInputs.Concat(positiveInput).ToArray();
		}
		public void AddInputControls(MouseButton[] positiveInput, MouseButton[] negativeInput)
		{
			AddInputControls(positiveInput);
			if (negativeMouseInputs == null)
				negativeMouseInputs = Array.Empty<MouseButton>();
			negativeMouseInputs = negativeMouseInputs.Concat(negativeInput).ToArray();
		}
		public void AddInputControls(GamepadButton[] positiveInput)
		{
			if (positiveGamepadInputs == null)
				positiveGamepadInputs = Array.Empty<GamepadButton>();
			positiveGamepadInputs = positiveGamepadInputs.Concat(positiveInput).ToArray();
		}
		public void AddInputControls(GamepadButton[] positiveInput, GamepadButton[] negativeInput)
		{
			AddInputControls(positiveInput);
			if (negativeGamepadInputs == null)
				negativeGamepadInputs = Array.Empty<GamepadButton>();
			negativeGamepadInputs = negativeGamepadInputs.Concat(negativeInput).ToArray();
		}
		#endregion

		#region Operators
		public static InputButton operator +(InputButton origin, InputButton addition)
		{
			if(origin.IsAxis && addition.IsAxis)
			{
				if (addition.KeyControlled)
					origin.AddInputControls(addition.PositiveKey, addition.NegativeKey);
				if(addition.MouseControlled)
					origin.AddInputControls(addition.PositiveMouse, addition.NegativeMouse);
				if (addition.GamepadControlled)
					origin.AddInputControls(addition.PositiveGamepad, addition.NegativeGamepad);

			}
			else if(!origin.isAxis && !addition.IsAxis)
			{
				if (addition.KeyControlled)
					origin.AddInputControls(addition.PositiveKey);
				if (addition.MouseControlled)
					origin.AddInputControls(addition.PositiveMouse);
				if (addition.GamepadControlled)
					origin.AddInputControls(addition.PositiveGamepad);
			}
			else
			{
				//Error - Can't add an input button and input axis together. 
			}
			return origin;
		}
		#endregion
	}
}