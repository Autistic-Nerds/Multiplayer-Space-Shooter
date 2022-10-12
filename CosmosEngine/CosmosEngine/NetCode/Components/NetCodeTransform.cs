using CosmosEngine;

namespace CosmosEngine.Netcode
{
	public class NetcodeTransform : NetcodeBehaviour
	{
		[SyncVar(hook = nameof(UpdatePosition))] private Vector2 position;
		[SyncVar(hook = nameof(UpdateRotation))] private float rotation;

		private Vector2 desiredPosition;
		private float desiredRotation;

		protected override void Update()
		{
			if (FindObjectOfType<NetcodeServer>().IsServerConnection)
			{
				position = Transform.Position;
				rotation = Transform.Rotation;
			}
			else
			{
				float dist = Vector2.Distance(Transform.Position, desiredPosition);
				Transform.Position = Vector2.MoveTowards(Transform.Position, desiredPosition, 3f * dist * Time.DeltaTime);
				Transform.Rotation = Mathf.MoveTowardsAngle(Transform.Rotation, desiredRotation, 360f * Time.DeltaTime);
			}
		}

		private void UpdatePosition()
		{
			desiredPosition = position;
		}

		private void UpdateRotation()
		{
			desiredRotation = rotation;
		}
	}
}
