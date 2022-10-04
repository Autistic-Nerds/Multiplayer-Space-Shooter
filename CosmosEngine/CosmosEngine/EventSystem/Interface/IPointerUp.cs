
namespace CosmosEngine.EventSystems
{
	/// <summary>
	/// Detects whenever the mouse is released over the handler.
	/// </summary>
	public interface IPointerUp : IPointerHandler
	{
		void OnPointerUp(PointerEventData pointerEventData);
	}
}