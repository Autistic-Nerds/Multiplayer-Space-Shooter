
using CosmosEngine.CoreModule;
using CosmosEngine.Rendering;

namespace CosmosEngine.UI
{
	public class TextField : UIRenderer
	{
		private string text;
		private Font font;
		private int fontSize;
		private VerticalAlignment verticalAlignment;
		private HorizontalAlignment horizontalAlignment;

		public string Text 
		{
			get => text;
			set
			{
				text = value;
			}
		}
		public Font Font { get => font; set => font = value; }
		public int FontSize { get => fontSize; set => fontSize = value; }
		public VerticalAlignment VerticalAlignment { get => verticalAlignment; set => verticalAlignment = value; }
		public HorizontalAlignment HorizontalAlignment { get => horizontalAlignment; set => horizontalAlignment = value; }

		public TextField()
		{
			this.text = "";
			this.font = Font.Verdana;
			this.fontSize = 12;
			verticalAlignment = VerticalAlignment.Middle;
			horizontalAlignment = HorizontalAlignment.Center;
		}

		public override void UI()
		{
			//This is very heavy performance requirements and should be moved into an update method, to then be displayed.
			string[] vs = Text.Split('\n');
			Vector2 textSize = MeasureString();
			Vector2 position = RectTransform.AnchouredPosition - RectTransform.SizeDelta * (RectTransform.Anchour - 0.5f);
			Vector2 origin = position;
			foreach(string v in vs)
			{
				position.X = origin.X;
				Vector2 measurement = Font.MeasureString(v, FontSize);
				position.X += HorizontalAlignment switch
				{
					HorizontalAlignment.Left => -RectTransform.SizeDelta.X / 2,
					HorizontalAlignment.Center => -measurement.X / 2,
					HorizontalAlignment.Right => RectTransform.SizeDelta.X / 2 - measurement.X,
				};
				position.Y = VerticalAlignment switch
				{
					VerticalAlignment.Top => -RectTransform.SizeDelta.Y / 2,
					VerticalAlignment.Middle => -textSize.Y / 2,
					VerticalAlignment.Bottom => RectTransform.SizeDelta.Y / 2 - measurement.Y,
				};
				position.Y += origin.Y;
				Draw.Text(v, Font, FontSize, position, Colour, SortingValue);
				origin.Y += measurement.Y;
			}
			//Draw.Text(Text, Font, FontSize, originPosition, Vector2.Zero, 0.0f, Colour, SortingValue);
		}

		public override void SetNativeSize()
		{
			RectTransform.SizeDelta = Font.MeasureString(text, FontSize);
		}

		public Vector2 MeasureString() => Font.MeasureString(Text, FontSize);
		public int GetHeight() => (int)Font.FontHeight(FontSize);

		protected override void OnDrawGizmos()
		{
			return;
			Gizmos.Colour = Colour.Silver;
			Gizmos.DrawWireBox(RectTransform.AnchouredPosition - RectTransform.SizeDelta * (RectTransform.Anchour - 0.5f), RectTransform.SizeDelta, 1);
		}
	}
}