
#nullable enable
using CosmosEngine.Modules;
using CosmosEngine.InputModule;
using System;
using System.Collections.Generic;

namespace CosmosEngine
{
	/// <summary>
	/// <see cref="CosmosEngine.InputSystem"/> is a modular input system, which allow user defined input action that will invoke methods when an input is received. Requires some setup time, but has much more fine control and can return custom types on input through the use of <see cref="CosmosEngine.InputModule.CallbackContext"/>.
	/// </summary>
	public sealed class InputSystem : GameModule<InputSystem>, IUpdateModule
	{
		private readonly List<InputAction> inputActions = new List<InputAction>();

		public void AddInputAction(string name, Action<CallbackContext>? started, Action<CallbackContext>? performed, Action<CallbackContext>? canceled, params InputControl[] control)
		{
			InputAction input = new InputAction(name, control);
			input.OnStarted.Add(started);
			input.OnPerformed.Add(performed);
			input.OnCanceled.Add(canceled);
		}

		public void AddInputAction(string name, Action? started, Action? performed, Action? canceled, params InputControl[] control)
		{

		}

		void IUpdateModule.Update()
		{
		}
	}
}