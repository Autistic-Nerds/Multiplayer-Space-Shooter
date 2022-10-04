namespace CosmosEngine.PhysicsModule
{
	public static partial class PhysicsIntersection
	{
		#region Circle / Circle
		/// <summary>
		/// Calculate collision between two circles.
		/// </summary>
		/// <param name="a">First <see cref="CosmosEngine.CircleCollider"/></param>
		/// <param name="b">Second <see cref="CosmosEngine.CircleCollider"/></param>
		/// <returns></returns>
		public static bool CircleCircle(CircleCollider a, CircleCollider b) => CircleCircle(a.Position, a.Radius, b.Position, b.Radius);
		/// <summary>
		/// <inheritdoc cref="CircleCircle(CircleCollider, CircleCollider)"/>
		/// </summary>
		/// <param name="pA">Position of the first circle.</param>
		/// <param name="rA">Radius of the first cricle.</param>
		/// <param name="pB">Position of the first circle.</param>
		/// <param name="rB">Radius of the first cricle.</param>
		/// <returns></returns>
		public static bool CircleCircle(Vector2 pA, float rA, Vector2 pB, float rB) => CircleCircle(pA.X, pA.Y, rA, pB.X, pB.Y, rB);
		/// <summary>
		/// <inheritdoc cref="CircleCircle(CircleCollider, CircleCollider)"/>
		/// </summary>
		/// <param name="x1">First circle centre X position.</param>
		/// <param name="y1">First circle centre Y position.</param>
		/// <param name="r1">First circle radius.</param>
		/// <param name="x2">Second circle centre X position.</param>
		/// <param name="y2">Second cricle centre Y position.</param>
		/// <param name="r2">Second cricle radius.</param>
		/// <returns></returns>
		public static bool CircleCircle(float x1, float y1, float r1, float x2, float y2, float r2)
		{
			//Find distance between each circle and check if radisu of both circles are less than the distance 
			float distX = x1 - x2;
			float distY = y1 - y2;
			float distSqrt = (distX * distX) + (distY * distY);
			//if the distance is less than the sum of the circle's radii, the circles are touching!
			if (distSqrt <= (r1 + r2) * (r1 + r2))
			{
				return true;
			}
			return false;
		}

		#endregion
	}
}