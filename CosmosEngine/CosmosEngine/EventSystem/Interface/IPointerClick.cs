
namespace CosmosEngine.EventSystems
{
	/// <summary>
	/// Detects whenever the mouse is down over the handler.
	/// </summary>
	public interface IPointerClick : IPointerHandler
	{
		void OnPointerClick(PointerEventData pointerEventData);
	}
}