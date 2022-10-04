
namespace CosmosEngine.InputModule
{
	[System.Flags]
	public enum Interaction
	{
		None = 0,
		/// <summary>
		/// Invokes Started
		/// </summary>
		Press = 1 << 0,
		/// <summary>
		/// Invokes Canceled
		/// </summary>
		Release = 1 << 1,
		/// <summary>
		/// Invokes Performed
		/// </summary>
		Hold = 1 << 2,
		/// <summary>
		/// Invokes Started and Canceled
		/// </summary>
		PressAndRelease = Interaction.Press | Interaction.Release,
		/// <summary>
		/// Invokes Started, Performed and Canceled
		/// </summary>
		All = Interaction.Press | Interaction.Release | Interaction.Hold,
	}
}
