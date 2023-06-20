
using CosmosEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using CosmosEngine.Async;
using CosmosEngine.Modules;

namespace CosmosEngine
{
	/// <summary>
	/// A coroutine is a function that can suspend its execution (yield) until the given <see cref="CosmosEngine.Async.YieldInstruction"/> finishes. Yielding of any type, including null, results in the execution coming back on a later frame, unless the coroutine is stopped or has complete.
	/// </summary>
	public sealed class Coroutine
	{
		#region Fields
		private Stack<IEnumerator> routines = new Stack<IEnumerator>();
		private readonly Event<Coroutine> onRoutineCompleteEvent = new Event<Coroutine>();
		private string name;
		private CoroutineState coroutineState;

		/// <summary>
		/// Represents whether the <see cref="CosmosEngine.Coroutine"/> is running (not complete) or not.
		/// </summary>
		internal bool IsAlive => !(State == CoroutineState.Stopped || State == CoroutineState.Interrupted);
		/// <summary>
		/// The name is reference when starting the <see cref="CosmosEngine.Coroutine"/> using a string, this way it can be stopped through the use a string as well.
		/// </summary>
		internal string Name { get => name; set => name = value; }
		/// <summary>
		/// An <see cref="EventSystems.Event"/> that is invoked once the <see cref="CosmosEngine.Coroutine"/> has completed running successfully. If the coroutine is stopped through a StopCoroutine this method will not be invoked, since it was interrupted doing its execution.
		/// </summary>
		public Event<Coroutine> OnRoutineComplete => onRoutineCompleteEvent;
		/// <summary>
		/// The current <see cref="CoroutineState"/> of this <see cref="Coroutine"/>.
		/// </summary>
		public CoroutineState State { get => coroutineState; private set => coroutineState = value; }
		#endregion

		#region Public Methods
		internal void Initialise(IEnumerator routine)
		{
			onRoutineCompleteEvent.RemoveAllListeners();
			routines.Clear();
			routines.Push(routine);
			State = CoroutineState.Running;
		}
		/// <summary>
		/// Pauses the <see cref="CosmosEngine.Coroutine"/> all functionality is stopped until <see cref="CosmosEngine.Coroutine.Resume"/> is invoked. <see cref="CosmosEngine.Async.YieldInstruction"/> will also wait until the coroutine is running.
		/// </summary>
		public void Pause() => State = CoroutineState.Paused;
		/// <summary>
		/// Resumes the <see cref="CosmosEngine.Coroutine"/> if it has been paused using <see cref="CosmosEngine.Coroutine.Pause"/>.
		/// </summary>
		public void Resume()
		{
			if (State == CoroutineState.Paused)
				State = CoroutineState.Running;
		}
		internal void Stop() => State = CoroutineState.Interrupted;

		internal void Update()
		{
			if (routines.Count > 0)
			{
				if (routines.Peek().Current is YieldInstruction yieldInstruction)
				{
					if (yieldInstruction.KeepWaiting)
					{
						//CoreModule.EngineCore.Debug.Add("Waiting: " + yieldInstruction.ToString());
						State = CoroutineState.Waiting;
						return;
					}
					else
					{
						yieldInstruction.Complete();
					}
				}
			}
			//CoreModule.EngineCore.Debug.Add("Running");

			if (routines.Peek().MoveNext() == false)
			{
				routines.Pop();

				if (routines.Count == 0)
				{
					State = CoroutineState.Stopped;
					OnRoutineComplete.Invoke(this);
					return; //We are done.
				}
			}
			else if(routines.Peek().Current is IEnumerator)
			{
				routines.Push((IEnumerator)routines.Peek().Current);
			}
			else
			{
				State = CoroutineState.Running;
			}
		}

		#endregion

		#region Static Methods

		public static Coroutine Start(IEnumerator routine)
		{
			return CoroutineManager.StartCoroutine(routine);
		}

		#endregion
	}
}