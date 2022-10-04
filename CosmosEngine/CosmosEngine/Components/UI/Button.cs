
using CosmosEngine.CoreModule;
using CosmosEngine.EventSystems;

namespace CosmosEngine.UI
{
	public class Button : UIComponent, IPointerClick, IPointerUp, IPointerEnter, IPointerExit
	{
		private Event onClickEvent;
		private SpriteBlock spriteBlock;
		private ColourBlock colourBlock;
		private Colour desiredColour;
		private Colour currentColour;
		private float colourProgression;
		private ButtonTransition transition;

		private Image attachedImage;
		private bool pointerHover;
		private bool pointerPressed;
		/// <summary>
		/// The event invoked when the mouse button is pressed when over this <see cref="CosmosEngine.UI.Button"/>.
		/// </summary>
		public Event OnClick => onClickEvent;
		/// <summary>
		/// 
		/// </summary>
		public ColourBlock ColourBlock { get => colourBlock; set => colourBlock = value; }
		public SpriteBlock SpriteBlock { get => spriteBlock; set => spriteBlock = value; }
		public ButtonTransition Transition 
		{ 
			get => transition;
			set
			{
				transition = value;
				switch (value)
				{
					case ButtonTransition.ColourTint:
						desiredColour = colourBlock.NormalColour;
						break;
					case ButtonTransition.SpriteSwap:
						if (AffectedImage != null)
							AffectedImage.ColourMultiplier = Colour.White;
						break;
					case ButtonTransition.None:
						if (AffectedImage != null)
							AffectedImage.ColourMultiplier = Colour.White;
						break;
				}
			}
		}
		public Image AffectedImage { get => attachedImage; set => attachedImage = value; }

		public Button()
		{
			transition = ButtonTransition.ColourTint;
			colourBlock = ColourBlock.DefaultColourBlock;
			desiredColour = colourBlock.NormalColour;
			onClickEvent = new Event();
		}

		protected override void OnEnable()
		{
			pointerPressed = false;
		}
		internal override void AssignGameObject(GameObject gameObject)
		{
			base.AssignGameObject(gameObject);
			GameObject.ModifiedEvent.Add(GameObjectModified);
		}

		private void GameObjectModified(GameObjectChange change)
		{
			if (attachedImage == null)
				attachedImage = GameObject.GetComponent<Image>();
		}

		protected override void OnDestroy()
		{
			GameObject.ModifiedEvent.Remove(GameObjectModified);
		}

		protected override void Update()
		{
			switch(Transition)
			{
				case ButtonTransition.ColourTint:
					ColourChangeEffect();
					break;
				case ButtonTransition.SpriteSwap:
					break;
			}
		}

		private void InvokeButtonEvent()
		{
			onClickEvent.Invoke();
		}

		private void InvokeColourChange(Colour colour)
		{
			//Debug.Log("Innvoking colour change");
			//if(colourProgression < 1f)
			//{
			//	nextColour = colour;
			//	return;
			//}
			desiredColour = colour;
			colourProgression = 0f;
		}

		private void ColourChangeEffect()
		{
			if(AffectedImage != null)
			{
				if(colourProgression < 1f)
				{
					colourProgression = Mathf.MoveTowards(colourProgression, 1f, 1f / colourBlock.FadeDuration * Time.UnscaledDeltaTime);
					currentColour = Colour.Lerp(currentColour, desiredColour, colourProgression);
				}
				else
				{
					//if(nextColour != desiredColour)
					//{
					//	InvokeColourChange(nextColour);
					//}
				}
				AffectedImage.ColourMultiplier = currentColour;
			}
		}

		void IPointerClick.OnPointerClick(PointerEventData pointerEventData)
		{
			pointerPressed = pointerHover;
			if (pointerPressed)
				InvokeColourChange(ColourBlock.PressedColour);
		}

		void IPointerEnter.OnPointerEnter(PointerEventData pointerEventData)
		{
			pointerHover = true;
			InvokeColourChange(ColourBlock.HighlightColour);
		}

		void IPointerExit.OnPointerExit(PointerEventData pointerEventData)
		{
			pointerHover = false;
			InvokeColourChange(ColourBlock.NormalColour);
		}

		void IPointerUp.OnPointerUp(PointerEventData pointerEventData)
		{
			if(pointerPressed && pointerHover)
			{
				InvokeButtonEvent();
				InvokeColourChange(pointerHover ? ColourBlock.HighlightColour : ColourBlock.NormalColour);
			}
		}
	}
}