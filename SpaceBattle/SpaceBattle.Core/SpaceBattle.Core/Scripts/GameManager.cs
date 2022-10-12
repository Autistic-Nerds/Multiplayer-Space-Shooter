using CosmosEngine;
using CosmosEngine.Netcode;

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
			GameObject serverObject = new GameObject();
			NetcodeServer server = serverObject.AddComponent<NetcodeServer>();
			//server.SimulateLatency(100, 0.0f);

			GameObject obj = new GameObject("Net Object");
			obj.AddComponent<NetcodeIdentity>();
			obj.AddComponent<NetcodeTransform>();
			obj.AddComponent<TestPlayer>();
			obj.AddComponent<SpriteRenderer>().Sprite = ArtContent.InterceptorEnemy;

			
			//GameObject playerobj = new GameObject("Player");
			//playerobj.AddComponent<Unit>();
			//playerobj.AddComponent<ControlInput>();
			//playerobj.AddComponent<SpriteRenderer>().Sprite = ArtContent.Interceptor;

			//GameObject go = new GameObject("Chat", typeof(NetworkChat));
		}

		public override void Update()
		{
		}

	}
}