using CosmosEngine;
using CosmosEngine.Netcode;

namespace SpaceBattle
{
	internal class NetworkChat : NetcodeBehaviour
	{
		string chatMsg;
		private KeyboardInput keyboard;

		protected override void Start()
		{
			keyboard = new KeyboardInput();
		}

		protected override void Update()
		{
			keyboard.ReadNext();
			chatMsg = keyboard.Read;
			Debug.QuickLog($"Chat: {chatMsg}");
			if(InputState.Pressed(CosmosEngine.InputModule.Keys.Enter))
			{
				keyboard.Clear();
			}
		}
	}
}