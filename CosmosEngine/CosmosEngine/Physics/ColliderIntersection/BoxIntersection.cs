using Microsoft.Xna.Framework;

namespace CosmosEngine.PhysicsModule
{
	public static partial class PhysicsIntersection
	{

		#region Rectangle / Rectangle
		/// <summary>
		/// Calculate collision between two rectangles.
		/// </summary>
		/// <param name="a">First BoxCollider.</param>
		/// <param name="b">Second BoxCollider.</param>
		/// <returns></returns>
		public static bool BoxBox(BoxCollider a, BoxCollider b) => BoxBox(a.Position, a.ScaledSize, b.Position, b.ScaledSize);
		/// <summary>
		/// <inheritdoc cref="BoxBox(BoxCollider, BoxCollider)"/>
		/// </summary>
		/// <param name="a">The first rectangle.</param>
		/// <param name="b">The second rectangle</param>
		/// <returns></returns>
		public static bool BoxBox(Rectangle a, Rectangle b) => a.Intersects(b);
		/// <summary>
		/// <inheritdoc cref="BoxBox(BoxCollider, BoxCollider)"/>
		/// </summary>
		/// <param name="posA">First box center position as a <see cref="CosmosEngine.Vector2"/></param>
		/// <param name="sizeA">First box size as a <see cref="CosmosEngine.Vector2"/></param>
		/// <param name="posB">Second box center position as a <see cref="CosmosEngine.Vector2"/></param>
		/// <param name="sizeB">Second box size as a <see cref="CosmosEngine.Vector2"/></param>
		/// <returns></returns>
		public static bool BoxBox(Vector2 posA, Vector2 sizeA, Vector2 posB, Vector2 sizeB) => BoxBox(posA.X - sizeA.X / 2, posA.Y - sizeA.Y / 2, sizeA.X, sizeA.Y, posB.X - sizeB.X / 2, posB.Y - sizeB.Y / 2, sizeB.X, sizeB.Y);

		/// <summary>
		/// <inheritdoc cref="BoxBox(BoxCollider, BoxCollider)"/>
		/// </summary>
		/// <param name="rAx">First box top left corner X position.</param>
		/// <param name="rAy">First box top left corner Y position.</param>
		/// <param name="rAw">First box width.</param>
		/// <param name="rAh">First box height.</param>
		/// <param name="rBx">Seocnd box top left corner X position.</param>
		/// <param name="rBy">Second box top left corner Y position.</param>
		/// <param name="rBw">Second box width.</param>
		/// <param name="rBh">Second box height.</param>
		/// <returns></returns>
		public static bool BoxBox(float rAx, float rAy, float rAw, float rAh, float rBx, float rBy, float rBw, float rBh)
		{
			//Debug.FastLog($"A: x:{rAx:F2} y:{rAy:F2} w:{rAw:F2} h:{rAh} --- B: x:{rBx:F2} y:{rBy:F2} w:{rBw:F2} h:{rBh}");
			//Debug.FastLog($"Rect A right edge / B left: {rAx + rAw} >= {rBx} == {(rAx + rAw >= rBx)}");
			//Debug.FastLog($"Rect A left edge /  B right: {rAx} <= {rBx + rBx} == {(rAx <= rBx + rBw)}");
			//Debug.FastLog($"Rect A top edge / B bottom: {rAy + rAh} >= {rBy} == {(rAy + rAh >= rBy)}");
			//Debug.FastLog($"Rect A bottom edge / B top: {rAy} <= {rBy + rBh} == {(rAy <= rBy + rBh)}");
			if (rAx + rAw >= rBx &&		// rect A right edge past rect B left
				rAx <= rBx + rBw &&		// rect A left edge past rect B right
				rAy + rAh >= rBy &&		// rect A top edge past rect B bottom
				rAy <= rBy + rBh)       // rect A bottom edge past rect B top
				return true;
			return false;
		}

		#endregion

		#region Rectangle / Point

		#endregion
	}
}