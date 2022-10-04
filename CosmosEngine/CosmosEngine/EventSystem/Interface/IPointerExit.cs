
namespace CosmosEngine.EventSystems
{
	/// <summary>
	/// Detects whenever the mouse exits the handler's area.
	/// </summary>
	public interface IPointerExit : IPointerHandler
	{
		void OnPointerExit(PointerEventData pointerEventData);
	}
}