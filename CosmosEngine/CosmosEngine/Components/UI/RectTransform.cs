
using CosmosEngine.CoreModule;

namespace CosmosEngine.UI
{
	public class RectTransform : Transform, IUIComponent
	{
		#region Fields
		private Vector2 sizeDelta;
		private Vector2 anchour;
		private Vector2 pivot;
		private Vector2 anchourPosition;

		/// <summary>
		/// The size of this <see cref="CosmosEngine.UI.RectTransform"/> relative to the distances between the anchors.
		/// </summary>
		public Vector2 SizeDelta
		{
			get => sizeDelta;
			set
			{
				sizeDelta = value;
				Dirty();
			}
		}

		/// <summary>
		/// The position of the pivot of this <see cref="CosmosEngine.UI.RectTransform"/> relative to the anchor reference point.
		/// </summary>
		public Vector2 Anchour
		{
			get => anchour;
			set
			{
				anchour = value; 
				Dirty();
			}
		}

		[System.Obsolete("Pivot for the RectTransform is obsolete and should not be relied on, since it will be removed in a later version.", false)]
		/// <summary>
		/// The normalized position in this <see cref="CosmosEngine.UI.RectTransform"/> that it rotates around.
		/// </summary>
		public Vector2 Pivot
		{
			get => pivot;
			set
			{
				pivot = value;
				Dirty();
			}
		}

		/// <summary>
		/// The position of the pivot of this <see cref="CosmosEngine.UI.RectTransform"/> relative to the anchor reference point.
		/// </summary>
		public Vector2 AnchouredPosition => anchourPosition;
		public float Width { get => SizeDelta.X; set => SizeDelta = new Vector2(value, SizeDelta.Y); }
		public float Height { get => SizeDelta.Y; set => SizeDelta = new Vector2(SizeDelta.X, value); }
		#endregion

		public RectTransform()
		{
			anchour = new Vector2(0.5f, 0.5f);
			pivot = new Vector2(0.5f, 0.5f);
			sizeDelta = new Vector2(100, 100);
		}
		
		#region Private Methods
		private void UpdateTransformAnchour()
		{
			if (Parent == null || !(Parent is RectTransform))
				anchourPosition = new Vector2(Screen.Width * Anchour.X + Position.X, Screen.Height * Anchour.Y + Position.Y);
			else
				anchourPosition = ((RectTransform)Parent).AnchouredPosition + LocalPosition;
		}

		#endregion

		#region Public Methods
		protected override void Dirty()
		{
			UpdateTransformAnchour();
			TransformUpdateEvent.Invoke();
		}

		protected override void OnEnable()
		{
			Dirty();
			Screen.OnScreenSizeChanged += Dirty;
		}

		protected override void OnDisable()
		{
			Screen.OnScreenSizeChanged -= Dirty;
		}

		public override void Reset()
		{
			sizeDelta = new Vector2(100, 100);
			pivot = new Vector2(0.5f, 0.5f);
			anchour = new Vector2(0.5f, 0.5f);
			base.Reset();
		}

		public override void Copy(Transform transformToCopy)
		{
			base.Copy(transformToCopy);
			if(transformToCopy is RectTransform rt)
			{
				this.SizeDelta = rt.SizeDelta;
				this.Anchour = rt.Anchour;
				this.Pivot = rt.Pivot;
			}
		}

		protected override void OnDrawGizmos()
		{
			Vector2 size = (SizeDelta / 100);
			Vector2 point = (AnchouredPosition / 100) + size * 0.5f;
			Gizmos.Colour = Colour.LightGrey;
			Gizmos.DrawWireBox(point, size, 1);
		}

		public override string ToString()
		{
			return $"Position: {LocalPosition} - Size: {SizeDelta} - Pivot: {Pivot} - Anchour: {Anchour} - AnchourPos: {AnchouredPosition} - Parent: {(Parent == null ? "null" : Parent.Name)}";
		}
		#endregion
	}
}