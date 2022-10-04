using CosmosEngine;
using System.Drawing.Text;

namespace SpaceBattle
{
	public class GameManager : CosmosEngine.CoreModule.Game
	{
		
		public override void Initialize()
		{
			BackgroundColour = Colour.DesaturatedBlue;
		}

		public override void Start()
		{
			GameObject gameObject = new GameObject();
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			gameObject.AddComponent<Unit>("Interceptor", "Fast and agile unit with low firepower but great evasive abilities.");
			spriteRenderer.Sprite = ArtContent.InterceptorPlayer;
			gameObject.AddComponent<ControlInput>();

		}

		public override void Update()
		{
		}

	}
}