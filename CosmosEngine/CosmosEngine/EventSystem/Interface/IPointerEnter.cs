
namespace CosmosEngine.EventSystems
{
	/// <summary>
	/// Detects whenever the mouse enters the handler's area.
	/// </summary>
	public interface IPointerEnter : IPointerHandler
	{
		void OnPointerEnter(PointerEventData pointerEventData);
	}
}