
namespace CosmosEngine.EventSystems
{
	/// <summary>
	/// Detects whenever the mouse is held down over the handler.
	/// </summary>
	public interface IPointerDown : IPointerHandler
	{
		void OnPointerDown(PointerEventData pointerEventData);
	}
}