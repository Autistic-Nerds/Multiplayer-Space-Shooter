using CosmosEngine.Variables;

namespace CosmosEngine
{
	public partial class Assets
	{
		private static Sprite abattoir_blue;
		public static Sprite Abattoir_Blue => abattoir_blue ??= (Sprite)new ContentSprite("Abattoir_Blue");
	}
}