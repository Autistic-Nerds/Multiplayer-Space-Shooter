using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CosmosEngine.PhysicsModule
{
	public static partial class PhysicsIntersection
	{
		#region Point / Point

		/// <summary>
		/// Calculates the collision between two points.
		/// </summary>
		public static bool PointPoint(Vector2 a, Vector2 b) => PointPoint(a.X, a.Y, b.X, b.Y);
		/// <summary>
		/// <inheritdoc cref="PointPoint(Vector2, Vector2)"/>
		/// </summary>
		public static bool PointPoint(Vector2 a, Vector2 b, float epsilon) => PointPoint(a.X, a.Y, b.X, b.Y, epsilon);
		/// <summary>
		/// <inheritdoc cref="PointPoint(Vector2, Vector2)"/>
		/// </summary>
		public static bool PointPoint(float xA, float yA, float xB, float yB) => PointPoint(xA, yA, xB, yB, 0.1f);
		/// <summary>
		/// <inheritdoc cref="PointPoint(Vector2, Vector2)"/>
		/// </summary>
		public static bool PointPoint(float xA, float yA, float xB, float yB, float epsilon)
		{
			if (Mathf.Abs(xA - xB) < epsilon && Mathf.Abs(yA - yB) < epsilon)
				return true;
			return false;
		}
		#endregion

		#region Point / Circle



		#endregion

		#region Point / Box
		/// <summary>
		/// Calculates whether a point is intersecting with a box.
		/// </summary>
		/// <param name="point">The point position.</param>
		/// <param name="box">The box represented as a <see cref="Microsoft.Xna.Framework.Rectangle"/>.</param>
		/// <returns></returns>
		public static bool PointBox(Vector2 point, Rectangle box) => PointBox(point.X, point.Y, box.X, box.Y, box.Width, box.Height);
		/// <summary>
		/// <inheritdoc cref="PointBox(Vector2, Rectangle)"/>
		/// </summary>
		/// <param name="point">The point position.</param>
		/// <param name="boxPosition">The box centre position.</param>
		/// <param name="boxSize">The box size.</param>
		/// <returns></returns>
		public static bool PointBox(Vector2 point, Vector2 boxPosition, Vector2 boxSize) => PointBox(point, boxPosition, boxSize, new Vector2(0.5f, 0.5f));
		/// <summary>
		/// <inheritdoc cref="PointBox(Vector2, Rectangle)"/>
		/// </summary>
		/// <param name="point">The point position.</param>
		/// <param name="boxPosition">The box position.</param>
		/// <param name="boxSize">The box size.</param>
		/// <param name="boxOrigin">The box normalised centre origin. A origin of 0.5f would mean the position represents the centre of the box.</param>
		/// <returns></returns>
		public static bool PointBox(Vector2 point, Vector2 boxPosition, Vector2 boxSize, Vector2 boxOrigin)
		{
			float originX = boxPosition.X - boxSize.X * boxOrigin.X;
			float originY = boxPosition.Y - boxSize.Y * boxOrigin.Y;
			return PointBox(point.X, point.Y, originX, originY, boxSize.X, boxSize.Y);

		}
		/// <summary>
		/// <inheritdoc cref="PointBox(Vector2, Rectangle)"/>
		/// </summary>
		/// <param name="px">The point x position.</param>
		/// <param name="py">The point y position.</param>
		/// <param name="rx">The box top left corner X position.</param>
		/// <param name="ry">The box top left corner Y position.</param>
		/// <param name="rw">The box width.</param>
		/// <param name="rh">The box height.</param>
		/// <returns></returns>
		public static bool PointBox(float px, float py, float rx, float ry, float rw, float rh)
		{
			if (px >= rx &&			// right of the left edge AND
				px <= rx + rw &&	// left of the right edge AND
				py >= ry &&			// below the top AND
				py <= ry + rh)		// above the bottom
			{					
				return true;
			}
			return false;
		}

		#endregion
	}
}