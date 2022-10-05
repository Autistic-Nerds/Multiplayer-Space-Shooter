using CosmosEngine;
using CosmosEngine.NetCode;

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
			AudioManager.Play();
			GameObject serverObject = new GameObject();
			NetCodeServer server = serverObject.AddComponent<NetCodeServer>();

			GameObject gameObject = new GameObject();
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			gameObject.AddComponent<Unit>("Interceptor", "Fast and agile unit with low firepower but great evasive abilities.");
			gameObject.AddComponent<NetCodeTransform>();
			spriteRenderer.Sprite = ArtContent.Interceptor;
			gameObject.AddComponent<ControlInput>();

			//GameObject obj = new GameObject("Mover");
			//obj.AddComponent<NetCodeIdentity>();
			//obj.AddComponent<NetCodeTransform>();
			//obj.AddComponent<NetworkMover>();
			//obj.AddComponent<SpriteRenderer>().Sprite = ArtContent.InterceptorEnemy;
		}

		public override void Update()
		{
		}

	}
}