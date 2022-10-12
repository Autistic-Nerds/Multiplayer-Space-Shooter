using CosmosEngine;

namespace SpaceBattle
{
	internal class Solider : GameBehaviour
	{
		private float health;

		public Solider()
		{

		}

		public Solider(float health)
		{
			this.health = health;
		}

		public void Initialize(float health)
		{
			this.health = health;
		}

		protected override void Update()
		{
			if(InputManager.GetKeyDown(CosmosEngine.InputModule.Keys.M))
			{
				health -= 10f;
				Debug.Log($"My health: {health}");
			}
		}
	}
}