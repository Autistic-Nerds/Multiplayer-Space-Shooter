using CosmosEngine;

namespace CosmosEngine.NetCode
{
	public class NetCodeTransform : NetCodeBehaviour
	{
		[SyncVar(hook = nameof(UpdatePosition))] private Vector2 position;
		[SyncVar(hook = nameof(UpdateRotation))] private float rotation;

		private Vector2 desiredPosition;
		private float desiredRotation;

		protected override void Update()
		{
			if (FindObjectOfType<NetCodeServer>().IsServerConnection)
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
