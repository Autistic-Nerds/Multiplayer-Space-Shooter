
namespace CosmosEngine
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Animator : Component
	{
		private int index;
		private float elsaped;
		private Animation currentAnimation;
		private SpriteRenderer spriteRenderer;
	}
}