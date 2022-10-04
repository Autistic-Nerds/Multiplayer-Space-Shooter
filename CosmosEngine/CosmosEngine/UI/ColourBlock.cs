
namespace CosmosEngine.UI
{
	public struct ColourBlock
	{
		private Colour normalColour;
		private Colour pressedColour;
		private Colour highlightColour;
		private Colour disabledColour;
		private float fadeDuration;
		private float colourMultiplier;
		private ColourChangeMode colourChangeMode;

		public static readonly ColourBlock DefaultColourBlock =
			new ColourBlock()
			{
				normalColour = Colour.White,
				highlightColour = new Colour(167, 208, 215),
				pressedColour = new Colour(98, 152, 175),
				disabledColour = new Colour(140, 90, 86),
				fadeDuration = 0.15f,
				colourChangeMode = ColourChangeMode.Multiplicative,
				colourMultiplier = 1f,
			};

		/// <summary>
		/// Colour when the button is idle.
		/// </summary>
		public Colour NormalColour { get => normalColour; set => normalColour = value; }
		/// <summary>
		/// Colour when the button is pressed.
		/// </summary>
		public Colour PressedColour { get => pressedColour; set => pressedColour = value; }
		/// <summary>
		/// Colour while the button is hovered.
		/// </summary>
		public Colour HighlightColour { get => highlightColour; set => highlightColour = value; }
		/// <summary>
		///	Colour when the button is not interactive.
		/// </summary>
		public Colour DisabledColour { get => disabledColour; set => disabledColour = value; }
		/// <summary>
		/// How long a colour transition should take.
		/// </summary>
		public float FadeDuration { get => fadeDuration; set => fadeDuration = value; }
		/// <summary>
		/// How strong the colour multiplier or addition is to the image.
		/// </summary>
		public float ColourMultiplier { get => colourMultiplier; set => colourMultiplier = value; }
		/// <summary>
		/// How the colours are changed for the image.
		/// </summary>
		public ColourChangeMode ColourChangeMode { get => colourChangeMode; set => colourChangeMode = value; }

		public ColourBlock()
		{
			normalColour = Colour.White;
			highlightColour = Colour.LightGrey;
			pressedColour = Colour.DarkGrey;
			disabledColour = Colour.Grey;
			fadeDuration = 0.2f;
			colourChangeMode = ColourChangeMode.Multiplicative;
			colourMultiplier = 1f;
		}

		public ColourBlock(Colour normalColour, Colour pressedColour, Colour highlightColour, Colour disabledColour, float fadeDuration) : this()
		{
			this.normalColour = normalColour;
			this.pressedColour = pressedColour;
			this.highlightColour = highlightColour;
			this.disabledColour = disabledColour;
			this.fadeDuration = fadeDuration;
			this.colourMultiplier = 1.0f;
			this.colourChangeMode = ColourChangeMode.Multiplicative;
		}

		public ColourBlock(Colour normalColour, Colour pressedColour, Colour highlightColour, Colour disabledColour, float fadeDuration, float colourMultiplier, ColourChangeMode colourChangeMode) : this()
		{
			this.normalColour = normalColour;
			this.pressedColour = pressedColour;
			this.highlightColour = highlightColour;
			this.disabledColour = disabledColour;
			this.fadeDuration = fadeDuration;
			this.colourMultiplier = colourMultiplier;
			this.colourChangeMode = colourChangeMode;
		}
	}
}