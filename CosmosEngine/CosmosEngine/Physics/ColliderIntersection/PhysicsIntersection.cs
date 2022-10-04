namespace CosmosEngine.PhysicsModule
{
	public static partial class PhysicsIntersection
	{
		private static float DistanceSqrt(float x1, float y1, float x2, float y2)
		{
			float distX = x1 - x2;
			float distY = y1 - y2;
			return (distX * distX) + (distY * distY);
		}

		public static bool GetCollision(Collider lhs, Collider rhs)
		{
			#region Box
			if (lhs is BoxCollider rA && rhs is BoxCollider rB) 
				return BoxBox(rA, rB);
			#endregion

			#region Circle
			if (lhs is CircleCollider cA && rhs is CircleCollider cB)
				return CircleCircle(cA, cB);
			#endregion

			return false;
		}
	}
}