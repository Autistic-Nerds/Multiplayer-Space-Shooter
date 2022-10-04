
using System.Collections.Generic;

namespace CosmosEngine.InputModule
{
	public class InputAction
	{
		private string name;
		private readonly List<InputControl> inputControls = new List<InputControl>();
		private InputActionPhase actionPhase = InputActionPhase.Disabled;

		private readonly InputEvent onStart = new InputEvent();
		private readonly InputEvent onPerform = new InputEvent();
		private readonly InputEvent onCancel = new InputEvent();

		public string Name { get => name; set => name = value; }
		internal List<InputControl> Controls => inputControls;
		internal InputActionPhase Phase => actionPhase;
		public InputEvent OnStarted => onStart;
		public InputEvent OnPerformed => onPerform; 
		public InputEvent OnCanceled => onCancel; 

		internal InputAction(string name, params InputControl[] control)
		{
			this.name = name;
			AddInputControl(control);
			Enable();
		}

		internal void AddInputControl(params InputControl[] control)
		{
			foreach (InputControl c in control)
			{
				inputControls.Add(c);
			}
		}

		internal void CheckState()
		{
			if (Phase == InputActionPhase.Disabled)
				return;

			string key = string.Empty;
			object contextResult = null;
			InputActionPhase[] actionPhases = null;

			InputActionPhase[] keyPhases;
			object keyResult;
			bool keyConfirmation;

			foreach (InputControl c in Controls)
			{
				keyPhases = null;
				keyResult = null;
				keyConfirmation = false;

				if (c.KeyControlled)
				{
					keyPhases = Input.CheckButton(c.Key, c.Interaction, out keyConfirmation);
					if (keyPhases != null)
					{
						actionPhases = actionPhases ?? keyPhases;
						key = (string.IsNullOrEmpty(key) ? c.Key.ToString() : key);
					}
				}
				else if (c.MouseControlled)
				{
					keyPhases = Input.CheckButton(c.Mouse, c.Interaction, out keyConfirmation);
					if (keyPhases != null)
						actionPhases = actionPhases ?? keyPhases;
					{
						key = (string.IsNullOrEmpty(key) ? c.Mouse.ToString() : key);
					}
				}
				else if (c.ButtonControlled)
				{
					keyPhases = Input.CheckButton(c.Button, c.Interaction, out keyConfirmation);
					if (keyPhases != null)
					{
						actionPhases = actionPhases ?? keyPhases;
						key = (string.IsNullOrEmpty(key) ? c.Button.ToString() : key);
					}
				}

				//We want to loop over all the InputControls if they have any custom return type, other we can skip everything after we got confirmation on one.
				if (actionPhases != null && actionPhases.Length > 0 && keyConfirmation)
				{
					keyResult = c.Result;
					if(keyResult == null)
					{
						break;
					}
					else
					{
						if (keyResult is float)
						{
							if (contextResult == null)
								contextResult = 0.0f;
							if (contextResult is float)
								contextResult = ((float)contextResult) + ((float)keyResult);
						}
						else if (keyResult is int)
						{
							if (contextResult == null)
								contextResult = 0;
							if (contextResult is int)
								contextResult = ((int)contextResult) + ((int)keyResult);
						}
						else if (keyResult is double)
						{
							if (contextResult == null)
								contextResult = 0.0;
							if (contextResult is double)
								contextResult = ((double)contextResult) + ((double)keyResult);
						}
						else if (keyResult is Vector2)
						{
							if (contextResult == null)
								contextResult = Vector2.Zero;
							if (contextResult is Vector2)
								contextResult = (Vector2)contextResult + (Vector2)keyResult;
						}
						else
						{
							if (contextResult == null)
								contextResult = keyResult;
						}
					}
				}
			}

			if(actionPhases != null && actionPhases.Length > 0)
			{
				CallbackContext context = new CallbackContext(this, key, contextResult);
				foreach(InputActionPhase phase in actionPhases)
				{
					//If interaction == All Started and Performed will be called on the same frame, which is not suppose to happen.
					switch(phase)
					{
						case InputActionPhase.Started:
							actionPhase = InputActionPhase.Started;
							Started(context);
							break;
						case InputActionPhase.Performed:
							actionPhase = InputActionPhase.Performed;
							Performed(context);
							break;
						case InputActionPhase.Canceled:
							actionPhase = InputActionPhase.Canceled;
							Canceled(context);
							break;
					}
				}
			}
			else
			{
				actionPhase = InputActionPhase.Waiting;
			}
		}

		public void Enable()
		{
			actionPhase = InputActionPhase.Waiting;
		}
		public void Disable()
		{
			actionPhase = InputActionPhase.Disabled;
		}

		private void Started(CallbackContext context)
		{
			if (OnStarted != null)
			{
				OnStarted.Invoke(context);
			}
		}

		private void Performed(CallbackContext context)
		{
			if (OnPerformed != null)
			{
				OnPerformed.Invoke(context);
			}
		}

		private void Canceled(CallbackContext context)
		{
			if (OnCanceled != null)
			{
				OnCanceled.Invoke(context);
			}
		}
	}
}
