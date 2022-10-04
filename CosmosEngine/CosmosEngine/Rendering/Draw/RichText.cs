
namespace CosmosEngine.Rendering
{
	public static class RichText
	{
		public const string Italic = "i";
		public const string Bold = "b";
		public const string Size = "size";
		public const string Colour = "colour";

		[System.Obsolete("Incomplete", false)]
		public static void Text(string text, Vector2 position, Font font, int fontSize, Colour colour)
		{
			Vector2 measure = font.MeasureString(text, fontSize);
			Vector2 point = position - measure;
			string f = "<colour> something </colour>";
		}
	}
}