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

			if (NetcodeHandler.IsConnected)
			{
				if (NetcodeHandler.IsClient)
				{
					if (InputManager.GetMouseButtonDown(0))
					{
						Vector2 pos = Camera.Main.ScreenToWorld(InputManager.MousePosition);
						elapsed = Time.ElapsedTime - elapsed;
						//Rpc(nameof(TestMethodServerRpc), pos, elapsed);
						Rpc(nameof(Shoot), null, pos);
					}
				}
			}
		}

		[ClientRPC]
		private void Teleport(Vector2 newPosition)
		{
			Transform.Position = newPosition;
		}

		[ClientRPC]
		private void Shoot(Vector2 pos)
		{
			Colour rndColour = Colour.Random;
			PlaceObject(pos, rndColour);
			Rpc(nameof(PlaceObject), null, pos, rndColour);
		}

		[ServerRPC]
		private void PlaceObject(Vector2 pos, Colour colour)
		{
			GameObject obj = new GameObject("Projectile");
			obj.AddComponent<SpriteRenderer>().Sprite = DefaultGeometry.Circle;
			obj.GetComponent<SpriteRenderer>().Colour = colour;
			obj.Transform.LocalScale = new Vector2(0.25f, 0.25f);
			obj.Transform.Position = pos;
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