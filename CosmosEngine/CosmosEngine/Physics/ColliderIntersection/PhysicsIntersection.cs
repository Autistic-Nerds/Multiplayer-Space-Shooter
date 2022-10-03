//Written by Philip Wittusen
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
	}
}