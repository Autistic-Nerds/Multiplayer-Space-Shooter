﻿using CosmosEngine;
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

			GameObject obj = new GameObject("Net Object");
			obj.AddComponent<NetcodeIdentity>();
			obj.AddComponent<NetcodeTransform>();
			obj.AddComponent<TestPlayer>();
			obj.AddComponent<SpriteRenderer>().Sprite = ArtContent.InterceptorEnemy;
		}

		public override void Update()
		{
		}

	}
}