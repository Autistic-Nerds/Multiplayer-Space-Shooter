
using CosmosEngine.EventSystems;
using CosmosEngine.UI;

namespace CosmosEngine.CoreModule
{
	public abstract class UIComponent : Component, IUIComponent, IPointerHandler
	{
		public RectTransform RectTransform => (Transform is RectTransform rectTransform) ? rectTransform : GameObject.ReplaceTransform<RectTransform>(true);

		public bool BlockRaycast { get; set; } = true;
		public WorldSpace WorldSpace => WorldSpace.Screen;
		public Vector2 HandlerPosition => RectTransform.AnchouredPosition - (RectTransform.SizeDelta * (RectTransform.Anchour - 0.5f));
		public Vector2 HandlerSize => RectTransform.SizeDelta;

		internal override void AssignGameObject(GameObject gameObject)
		{
			base.AssignGameObject(gameObject);
			if (!(Transform is RectTransform))
			{
				GameObject.ReplaceTransform<RectTransform>(true);
			}
		}
	}
}