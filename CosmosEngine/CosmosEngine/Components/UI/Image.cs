
using CosmosEngine.CoreModule;
using CosmosEngine.Rendering;

namespace CosmosEngine.UI
{
	public class Image : UIRenderer
	{
		private Vector2 size;
		private Vector2 renderPosition;
		private Sprite sprite;
		private Colour colourMultiplier;
		private Colour activeColour;
		private FillDirection fillDirection;
		private float fillAmount;

		private Sprite Texture { get => sprite ?? DefaultGeometry.Square; set => sprite = value; }
		public Sprite Sprite 
		{ 
			get => sprite;
			set
			{
				sprite = value;
				SetNativeSize();
				sourceRect = new Rect(Offset, (sprite != null) ? sprite.Size : RectTransform.SizeDelta);
			}
		}
		public override Colour Colour 
		{ 
			get => base.Colour;
			set
			{
				base.Colour = value;
				UpdateActiveColour();
			}
		}
		public Colour ColourMultiplier
		{
			get => colourMultiplier;
			set
			{
				colourMultiplier = value;
				UpdateActiveColour();
			}
		}

		public FillDirection FillDirection 
		{ 
			get => fillDirection;
			set
			{
				fillDirection = value;
				AdjustFillPercentage();
			}
		}
		public float FillAmount
		{
			get => fillAmount;
			set
			{
				fillAmount = Mathf.Clamp01(value);
				AdjustFillPercentage();
			}
		}

		public Vector2 Size => size;

		private void AdjustFillPercentage()
		{
			Rect source = new Rect(0, 0, Texture.Width, Texture.Height);
			switch (FillDirection)
			{
				case FillDirection.Left:
					source.Width = Mathf.Lerp(0, Texture.Width, FillAmount);
					break;
				case FillDirection.Right:
					source.X = Mathf.Lerp(0, Texture.Width, 1f - FillAmount);
					break;
				case FillDirection.Top:
					source.Y = Mathf.Lerp(0, Texture.Width, FillAmount);
					break;
				case FillDirection.Bottom:
					source.Height = Mathf.Lerp(0, Texture.Height, 1f - FillAmount);
					break;
			}
			sourceRect = source;
		}

		public Image()
		{
			fillAmount = 1.0f;
			colourMultiplier = Colour.White;
			UpdateActiveColour();
		}

		protected override void OnInstantiated()
		{
			sourceRect = new Rect(Vector2.Zero, RectTransform.SizeDelta);
		}

		private void UpdateActiveColour()
		{
			Colour.Deconstruct(out float r, out float g, out float b, out float a);
			colourMultiplier.Deconstruct(out float _r, out float _g, out float _b);
			activeColour = new Colour((float)(r * _r), (float)(g * _g), (float)(b * _b), (float)a);
		}

		private void RectTransformChanged()
		{
			float width = (RectTransform.SizeDelta.X * RectTransform.Scale.X) / Texture.Width;
			float height = (RectTransform.SizeDelta.Y * RectTransform.Scale.Y) / Texture.Height;
			size = new Vector2(width, height) * (100 / Texture.PixelsPerUnit);
			renderPosition = RectTransform.AnchouredPosition;
		}

		public override void SetNativeSize()
		{
			if(Sprite != null)
			{
				RectTransform.SizeDelta = new Vector2(Sprite.Width, Sprite.Height);
				sourceRect = new Rect(Vector2.Zero, Sprite.Size);
			}
			else
			{
				RectTransform.SizeDelta = new Vector2(100, 100);
				sourceRect = new Rect(Vector2.Zero, RectTransform.SizeDelta);
			}
			RectTransformChanged();
		}

		protected override void Update()
		{
			RectTransformChanged(); //Why is this invoked every frame? Is dirty flags not working correctly?
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			Transform.TransformUpdateEvent += RectTransformChanged;
			RectTransformChanged();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			Transform.TransformUpdateEvent -= RectTransformChanged;
		}

		public override void UI()
		{
			base.UI();
			Draw.Sprite(Texture, renderPosition, RectTransform.Rotation, Size, sourceRect, RectTransform.Anchour, activeColour, SortingValue);
		}
	}
}