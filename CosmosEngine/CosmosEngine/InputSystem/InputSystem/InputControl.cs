
namespace CosmosEngine.InputModule
{
	/// <summary>
	/// Holds a key, mouse or controller input action.
	/// An InputControl can hold a unique object result that can be read through the CallbackContext.
	/// An InputAction can contain multiple InputControl.
	/// </summary>
	public struct InputControl
	{
		private Keys keyInput;
		private MouseButton mouseInput;
		private GamepadButton buttonInput;
		private Interaction interaction;
		private object result;

		internal Keys Key => keyInput;
		internal MouseButton Mouse => mouseInput;
		internal GamepadButton Button => buttonInput;
		internal bool KeyControlled { get; private set; }
		internal bool MouseControlled { get; private set; }
		internal bool ButtonControlled { get; private set; }
		internal Interaction Interaction { get => interaction; set => interaction = value; }
		internal object Result => result;

		/// <summary>
		/// InputAction can return custom results, accessed within the CallbackContext.ReadValue.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="interaction">What kind of interactions should this InputControl use, changing interaction will change what Actions are called. Default is PressAndRelease</param>
		/// <param name="result">Generic variables as int, float or Vector2 will be added together if more than one InputControl exists.</param>
		public InputControl(Keys input, Interaction interaction = Interaction.All, object result = null)
		{
			this.interaction = interaction;
			this.result = result;
			keyInput = input;
			mouseInput = default;
			buttonInput = default;

			KeyControlled = true;
			MouseControlled = false;
			ButtonControlled = false;
		}

		public InputControl(MouseButton input, Interaction interaction = Interaction.All, object result = null)
		{
			this.interaction = interaction;
			this.result = result;
			keyInput = default;
			mouseInput = input;
			buttonInput = default;

			KeyControlled = false;
			MouseControlled = true;
			ButtonControlled = false;
		}

		public InputControl(GamepadButton input, Interaction interaction = Interaction.All, object result = null)
		{
			this.interaction = interaction;
			this.result = result;
			keyInput = default;
			mouseInput = default;
			buttonInput = input;

			KeyControlled = false;
			MouseControlled = false;
			ButtonControlled = true;
		}
	}
}
