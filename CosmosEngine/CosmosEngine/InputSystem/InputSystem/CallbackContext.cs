
namespace CosmosEngine.InputModule
{
#nullable enable
	// <summary>
	// CallbackContext contains information about what InputAction triggered and a string value of what key was pressed.
	// For simple InputActions the CallbackContext may be ignored since it's unused.
	// What makes a CallbackContext powerful is the ability to read custom data from it. 
	// When assigning a new InputAction each InputControl can contain a primitives and some structs such as [string/int/float/Vector2] - it can even contain custom data and types.
	// When that specific InputControl is activated it will deliver a CallbackContext that contains that custom data.
	// Best example is using the CallbackContext for movement, you can assign W, A, S and D as InputControls, each can
	// have a direction attachted to them. Such as W = Vector2(0, 1) and D = Vector2(1, 0).
	// When the action is triggered you can use CallbackContext.ReadValue<T> to return that value.
	// </summary>

	public struct CallbackContext
	{
		private InputAction inputAction;
		private string key;
		private object? value;
		public InputActionPhase InputActionPhase => inputAction.Phase;
		public InputAction InputAction => inputAction; //consider deleting
		public string Key => key;

		public CallbackContext(InputAction inputAction)
		{
			this.inputAction = inputAction;
			this.key = "";
			this.value = null;
		}

		public CallbackContext(InputAction inputAction, string key, object value)
		{
			this.inputAction = inputAction;
			this.key = key;
			this.value = value;
		}

		/// <summary>
		/// Read the custom value of the preformed <see cref="CosmosEngine.InputModule.InputControl"/> of the <see cref="CosmosEngine.InputModule.InputAction"/>.
		/// </summary>
		public T? ReadValue<T>()
		{
			if (value != null)
			{
				return (T)value;
			}
			return default;
		}

		public override string ToString()
		{
			if(inputAction == null)
			{
				return value == null ? $"key:{Key} => null" : $"key:{Key} => {value} : {value.GetType()}";
			}
			return value == null ? $"input:{inputAction.Name} => null" : $"input:{inputAction.Name} => {value} : {value.GetType()}";
		}
	}
}
