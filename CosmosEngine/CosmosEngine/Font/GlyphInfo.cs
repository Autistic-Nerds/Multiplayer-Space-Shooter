
namespace CosmosEngine.Text
{
	internal struct GlyphInfo
	{
		private int x;
		private int y;
		private int width;
		private int height;
		private int xOffset;
		private int yOffset;
		private int xAdvance;

		public int X => x;
		public int Y => y;
		public int Width => width;
		public int Height => height;
		public int OffsetX => xOffset;
		public int OffsetY => yOffset;
		public int AdvanceX => xAdvance;

		public GlyphInfo(int x, int y, int width, int height, int xOffset, int yOffset, int xAdvance)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.xOffset = xOffset;
			this.yOffset = yOffset;
			this.xAdvance = xAdvance;
		}

	}
}