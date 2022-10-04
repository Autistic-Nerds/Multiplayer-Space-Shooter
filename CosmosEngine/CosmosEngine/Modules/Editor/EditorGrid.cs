
using CosmosEngine.Rendering;

namespace CosmosEngine.Modules
{
	public sealed class EditorGrid : GameModule<EditorGrid>, IRenderModule
	{
		public void RenderUI()
		{
			//Draw.Box(new Vector2(Screen.Width / 200, Screen.Height / 200), Vector2.One, Colour.White, short.MaxValue);
		}

		public void RenderWorld()
		{
			return;
			if (Camera.Main == null)
				return;

			return;

			int scale = (int)(Camera.Main.OrthographicSize / 10) + 1;

			Vector2 offset =  new Vector2(-0.5f, -0.5f);
			Vector2 centre = Camera.Main.ScreenToWorld(new Vector2(Screen.Width, Screen.Height) / 2) + offset;
			centre -= Camera.Main.Transform.Position;
			float ratio = (float)Screen.Width / Screen.Height;
			int v = Mathf.RoundToInt(Camera.Main.OrthographicSize + 1);
			int h = Mathf.RoundToInt(v * ratio + 1);
			int xO = Mathf.RoundToInt(Camera.Main.Transform.Position.X);
			int yO = Mathf.RoundToInt(Camera.Main.Transform.Position.Y);
			Colour colour = new Colour(100, 100, 100, 100);
			for (int x = -h + xO; x < h + xO; x++)
			{
				for (int y = -v + yO; y <= v + yO; y++)
				{
					Draw.WireBox(centre + new Vector2(x, y), Vector2.One, 1, colour, short.MinValue);
				}
			}

			//for (int x = -h + xO; x < h + xO; x++)
			//{
			//	for (int y = -v + yO; y <= v + yO; y++)
			//	{
			//		Draw.WireBox(centre + new Vector2(x, y), Vector2.One, 1, colour, short.MinValue);
			//	}
			//}
		}
	}
}