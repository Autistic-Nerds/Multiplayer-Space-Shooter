namespace CosmosEngine
{
	public static class DefaultGeometry
	{
		private static Sprite square;
		private static Sprite circle;
		private static Sprite pixel;

		public static Sprite Square => square ??= new Sprite("DefaultGeometry/spr_square");
		public static Sprite Circle => circle ??= new Sprite("DefaultGeometry/spr_circle");
		public static Sprite Pixel => pixel ??= new Sprite("DefaultGeometry/spr_pixel");
	}
}