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
			server.SimulateLatency(100, 0.0f);

			GameObject obj = new GameObject("Net Object");
			obj.AddComponent<NetcodeIdentity>();
			obj.AddComponent<NetcodeTransform>();
			obj.AddComponent<Shooter>();
			obj.AddComponent<SpriteRenderer>().Sprite = ArtContent.InterceptorEnemy;

			GameObject go = new GameObject("Chat", typeof(NetcodeIdentity), typeof(NetworkChat));
		}

		public override void Update()
		{
		}

	}
}