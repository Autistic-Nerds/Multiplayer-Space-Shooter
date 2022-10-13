using CosmosEngine;
using CosmosEngine.Netcode.Serialization;

namespace CosmosEngine.Netcode
{
	public class NetcodeTransform : NetcodeBehaviour
	{
		[SyncVar] private Vector2 onlinePosition;
		[SyncVar] private float onlineRotation;

		private Vector2 desiredPosition;
		private float desiredRotation;

		protected override void Update()
		{
			if(!HasAuthority)
			{
				float dist = Vector2.Distance(Transform.Position, onlinePosition);
				Transform.Position = Vector2.Lerp(Transform.Position, onlinePosition, 10f * dist * Time.DeltaTime);
				Transform.Rotation = Mathf.LerpAngle(Transform.Rotation, onlineRotation, 360f * Time.DeltaTime);
			}
			else
			{
				onlinePosition = Transform.Position;
				onlineRotation = Transform.Rotation;
			}
		}

		private void UpdatePosition()
		{
			Debug.Log("Update Position");
			desiredPosition = onlinePosition;
		}

		private void UpdateRotation()
		{
			Debug.Log("Update Rotation");
			desiredRotation = onlineRotation;
		}
	}
}
