using CosmosEngine;
using CosmosEngine.Netcode.Serialization;

namespace CosmosEngine.Netcode
{
	public class NetcodeTransform : NetcodeBehaviour
	{
		[SyncVar] private Vector2 onlinePosition;
		[SyncVar] private float onlineRotation;

		protected override void Update()
		{
			if(!HasAuthority)
			{
				float dist = Vector2.Distance(Transform.Position, onlinePosition);
				if (dist > 1f)
					Transform.Position = onlinePosition;
				else
					Transform.Position = Vector2.MoveTowards(Transform.Position, onlinePosition, 10f * dist * Time.DeltaTime);

				Transform.Rotation = Mathf.MoveTowardsAngle(Transform.Rotation, onlineRotation, 360f * Time.DeltaTime);
			}
			else
			{
				onlinePosition = Transform.Position;
				onlineRotation = Transform.Rotation;
			}
		}
	}
}
