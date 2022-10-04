
namespace CosmosEngine.InputModule
{
	public enum InputActionPhase
	{
		/// <summary>
		/// The action is not enabled. (Called every frame)
		/// </summary>
		Disabled,
		/// <summary>
		/// The action is enabled and waiting for input on its associated controls. (Called every frame)
		/// </summary>
		Waiting,
		/// <summary>
		/// The action has started performing. (Called once) [pressed]
		/// </summary>
		Started,
		/// <summary>
		/// The action is being performed. (Called every frame) [hold]
		/// </summary>
		Performed,
		/// <summary>
		/// The action has stopped performing. (Called once) [release]
		/// </summary>
		Canceled,
	}
}