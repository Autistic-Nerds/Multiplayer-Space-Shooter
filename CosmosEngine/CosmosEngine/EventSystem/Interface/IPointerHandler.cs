
namespace CosmosEngine.EventSystems
{
	public interface IPointerHandler : IEventHandler
	{
		/// <summary>
		/// Whether the event handler should block raycast and mouse inputs. If <see langword="true"/> <see cref="CosmosEngine.EventSystems.Pointer.IsOverObject"/> will also return true.
		/// </summary>
		bool BlockRaycast { get; }
		/// <summary>
		/// 
		/// </summary>
		WorldSpace WorldSpace { get; }
		/// <summary>
		/// The position of the event handler.
		/// </summary>
		Vector2 HandlerPosition { get; }
		/// <summary>
		/// The size of the event handler.
		/// </summary>
		Vector2 HandlerSize { get; }
	}
}