
using CosmosEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace CosmosEngine
{
	public class Gizmos
	{
		public static Colour Colour { get; set; } = Colour.White;
		public static Matrix Matrix { get; set; } = Matrix.Identity;
		public static int SortingOrder { get; set; } = short.MaxValue - 2;
		private static short SortingValue => (short)Mathf.Min(SortingOrder, short.MaxValue);

		public static bool IsValidOperation()
		{
			StackTrace trace = new StackTrace(2);
			var frames = trace.GetFrames();
			bool validOperation = false;
			Type type;
			foreach (var frame in frames)
			{
				type = frame.GetMethod().DeclaringType;
				if (frame.GetMethod().Name == "OnDrawGizmos")
				{
					return true;
				}
			}
			return validOperation;
		}
		private static Vector2 Position(Vector2 original) => Vector2.Transform(original, Matrix);

		public static void DrawBox(Vector2 position, Vector2 size) => DrawBox(position, size, Gizmos.Colour);
		[Conditional("EDITOR")]
		public static void DrawBox(Vector2 position, Vector2 size, Colour colour)
		{
			if(IsValidOperation())
			{
				Draw.Box(position, size, colour, SortingValue);
			}
		}
		public static void DrawBox(Rect rect) => DrawBox(rect.Center / 100, rect.Size / 100);

		public static void DrawWireBox(Vector2 position, Vector2 size) => DrawWireBox(position, size, 2);
		public static void DrawWireBox(Vector2 position, Vector2 size, int thickness) => DrawWireBox(position, size, thickness, Gizmos.Colour);
		public static void DrawWireBox(Vector2 position, Vector2 size, Colour colour) => DrawWireBox(position, size, 2, colour);
		[Conditional("EDITOR")]
		public static void DrawWireBox(Vector2 position, Vector2 size, int thickness, Colour colour)
		{
			if (IsValidOperation())
			{
				Draw.WireBox(position, size, thickness, colour, SortingValue);
			}
		}

		public static void DrawCircle(Vector2 position, float radius) => DrawCircle(position, radius, Gizmos.Colour);
		[Conditional("EDITOR")]
		public static void DrawCircle(Vector2 position, float radius, Colour colour)
		{
			if(IsValidOperation())
			{
				Draw.Circle(position, radius, colour, SortingValue);
			}
		}

		public static void DrawWireCircle(Vector2 position, float radius) => DrawWireCircle(position, radius, 2);
		public static void DrawWireCircle(Vector2 position, float radius, int thickness) => DrawWireCircle(position, radius, thickness, Gizmos.Colour);
		public static void DrawWireCircle(Vector2 position, float radius, Colour colour) => DrawWireCircle(position, radius, 2, colour);
		[Conditional("EDITOR")]
		public static void DrawWireCircle(Vector2 position, float radius, int thickness, Colour colour)
		{
			if (IsValidOperation())
			{
				Draw.WireCircle(position, radius,  thickness, colour, SortingValue);
			}
		}

		public static void DrawLine(Vector2 pointA, Vector2 pointB) => DrawLine(pointA, pointB, Gizmos.Colour);
		public static void DrawLine(Vector2 pointA, Vector2 pointB, int thickness) => DrawLine(pointA, pointB, thickness, Gizmos.Colour);
		public static void DrawLine(Vector2 pointA, Vector2 pointB, Colour colour) => DrawLine(pointA, pointB, 2, colour);
		[Conditional("EDITOR")]
		public static void DrawLine(Vector2 pointA, Vector2 pointB, int thickness, Colour colour)
		{
			if (IsValidOperation())
			{
				Draw.Line(pointA, pointB, colour, thickness, SortingValue);
			}
		}

		public static void DrawRay(Vector2 origin, Vector2 direction) => DrawRay(origin, direction, Gizmos.Colour);
		public static void DrawRay(Vector2 origin, Vector2 direction, int thickness) => DrawRay(origin, direction, thickness, Gizmos.Colour);
		public static void DrawRay(Vector2 origin, Vector2 direction, Colour colour) => DrawRay(origin, direction, 2, colour);
		[Conditional("EDITOR")]
		public static void DrawRay(Vector2 origin, Vector2 direction, int thickness, Colour colour)
		{
			if (IsValidOperation())
			{
				Draw.Ray(origin, direction, colour, thickness, SortingValue);
			}
		}

		public static void DrawTexture(Sprite texture, Vector2 position) => DrawTexture(texture, position, Gizmos.Colour);
		[Conditional("EDITOR")]
		public static void DrawTexture(Sprite texture, Vector2 position, Colour colour)
		{
			if(IsValidOperation())
			{
				Draw.Sprite(texture, position, colour, SortingValue);
			}
		}

		public static void DrawText(string text, Vector2 position) => DrawText(text, position, Gizmos.Colour);
		public static void DrawText(string text, Vector2 position, Colour colour) => DrawText(text, position, Font.Inter, 14, colour);
		public static void DrawText(string text, Vector2 position, int size) => DrawText(text, position, Font.Inter, size, Gizmos.Colour);
		public static void DrawText(string text, Vector2 position, Font font, int size) => DrawText(text, position, font, size, Gizmos.Colour);
		[Conditional("EDITOR")]
		public static void DrawText(string text, Vector2 position, Font font, int size, Colour colour)
		{
			if (IsValidOperation())
			{
				Vector2 measure = font.MeasureString(text) / 2;
				Draw.Text(text.Trim(), font, size, (position * 100).Round() - measure.Round(), colour, (short)(SortingValue + 1));
			}
		}
	}	
}