using CosmosEngine.Netcode.Serialization;
using System.Collections.Generic;

namespace CosmosEngine.Netcode
{
	public class TestPlayer : NetcodeBehaviour
	{
		private float value;
		private int power;
		private Vector2 position;
		private float elapsed;

		protected override void Start()
		{
			elapsed = Time.ElapsedTime;
		}

		protected override void Update()
		{
			if(InputManager.GetButtonDown("space"))
			{
				value = Random.Range(0.0f, 1.0f);
				power = Random.Range(1, 100);
				position = Random.InsideUnitCircle();
			}

			if (FindObjectOfType<NetcodeServer>().IsServerConnection)
			{
			}
			if (InputManager.GetMouseButtonDown(0))
			{
				Vector2 pos = Camera.Main.ScreenToWorld(InputManager.MousePosition);
				elapsed = Time.ElapsedTime - elapsed;
				Rpc(nameof(TestMethodServerRpc), pos, elapsed);
			}
		}

		[ServerRPC]
		private void TestMethodServerRpc()
		{
			Debug.Log($"SERVER RPC RECIEVED");
		}

		[ServerRPC]
		private void TestMethodServerRpc(Vector2 newPosition)
		{
			Debug.Log($"SERVER RPC RECIEVED: {newPosition}");
		}

		[ServerRPC]
		private void TestMethodServerRpc(Vector2 newPosition, float timeSinceLast)
		{
			Debug.Log($"SERVER RPC RECIEVED: {newPosition} - {timeSinceLast:F2}");
		}

		[ClientRPC]
		private void TestExecuteClientRpc(Vector2 newPosition, float timeSinceLast)
		{
			Debug.Log($"CLIENT RPC RECIEVED: {newPosition} - {timeSinceLast:F2}");
		}

		public override void Serialize(ref NetcodeWriter stream)
		{
			stream.Write(value);
			stream.Write(power);
			stream.WriteVector2(position);
		}

		public override void Deserialize(ref NetcodeReader stream)
		{
			this.value = stream.Read<float>();
			this.power = stream.Read<int>();
			this.position = stream.Read<Vector2>();
		}
	}
}