
namespace CosmosEngine.CoreModule
{
	public abstract class UIRenderer : UIComponent, IRenderUI
	{
		private Colour colour;
		private short sortingOrder;
		private Vector2 offset;
		protected Rect sourceRect;

		/// <summary>
		/// Rendering colour for the Sprite graphic.
		/// </summary>
		public virtual Colour Colour { get => colour; set => colour = value; }
		/// <summary>
		/// Renderer's order within a sorting layer. [<see langword="-32768"/>, <see langword="32767"/>].
		/// </summary>
		public int SortingOrder
		{
			get => (int)sortingOrder;
			set => sortingOrder = (short)Mathf.ClampBetween(value, short.MinValue, short.MaxValue);
		}
		protected short SortingValue => sortingOrder;
		public virtual Vector2 Offset
		{
			get => offset;
			set
			{
				offset = value;
				sourceRect.Position = offset;
			}
		}

		public UIRenderer()
		{
			colour = Colour.White;
			sortingOrder = 0;
		}

		public abstract void SetNativeSize();

		public virtual void UI() { }
	}
}