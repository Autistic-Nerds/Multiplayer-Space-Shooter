
using Vector2Xna = Microsoft.Xna.Framework.Vector2;
using Matrix = Microsoft.Xna.Framework.Matrix;
using System;

namespace CosmosEngine
{
	public class Camera : Component
	{
		#region Fields

		private float min;
		private float max;
		private  Vector2 origin;
		private Vector2 offset;
		private static Camera mainCamera;
		public static Camera Main => mainCamera ??= new GameObject("Main Camera").AddComponent<Camera>();

		private Vector2 previousPosition;
		private float previousRotation;
		private Matrix viewMatrix;
		private Rect boundingBox;
		private float orthographicSize;
		private event Action onCameraChangeEvent = delegate { };

		/// <summary>
		/// The minimum orthogaphic size of the <see cref="CosmosEngine.Camera"/>.
		/// </summary>
		public float Min 
		{ 
			get => min;
			set
			{
				min = value;
				if (OrthographicSize < min)
					OrthographicSize = min;
			}
		}
		/// <summary>
		/// The maximum orthogaphic size of the <see cref="CosmosEngine.Camera"/>.
		/// </summary>
		public float Max 
		{ 
			get => max;
			set
			{
				max = value;
				if(OrthographicSize > max)
					OrthographicSize = max;
			}
		}
		private Vector2 Origin => origin;
		/// <summary>
		/// The position of the <see cref="CosmosEngine.Transform"/> belonging to the <see cref="CosmosEngine.Camera"/>.
		/// </summary>
		public Vector2 Position { get => Transform.Position; set => Transform.Position = value; }
		public float OrthographicSize
		{
			get => orthographicSize;
			set
			{
				float scale = (value - orthographicSize);
				if (scale != 0)
				{
					orthographicSize = Mathf.Max(Min, Mathf.Min(Max, value));
				}
			}
		}
		private float lastOrthographicSize;
		internal float OrthographicScale => ((float)Screen.Height / 200f) / OrthographicSize;
		public Matrix ViewMatrix => viewMatrix;
		#endregion

		public Camera()
		{
			min = 0.3f;
			max = 100.0f;
			orthographicSize = 5f;
			origin = Screen.Centre;
			viewMatrix =
				Matrix.CreateTranslation(0, 0, 0) * //Position
				Matrix.CreateTranslation(Origin.X, Origin.Y, 0) * //Offset
				Matrix.CreateScale(OrthographicScale, OrthographicScale, 1) * //Scale
				Matrix.CreateRotationZ(0); //Rotation
		}

		#region Methods

		protected override void Awake()
		{
			UpdateCamera();
			offset = ScreenToWorld(Origin);
			UpdateCamera();
		}

		protected override void OnEnable()
		{
			onCameraChangeEvent += UpdateCamera;
			Screen.OnScreenSizeChanged += ScreenSizeChanged;
		}

		protected override void OnDisable()
		{
			onCameraChangeEvent -= UpdateCamera;
			Screen.OnScreenSizeChanged -= ScreenSizeChanged;
		}

		protected override void Update()
		{
			//UpdateCamera();
			if (Transform.Position != previousPosition)
			{
				UpdateCamera();
				previousPosition = Transform.Position;
			}
			if (Transform.Rotation != previousRotation)
			{
				UpdateCamera();
				previousRotation = Transform.Rotation;
			}
            if (orthographicSize != lastOrthographicSize)
			{
				UpdateCamera();
				lastOrthographicSize = orthographicSize;
				UpdateCamera();
			}

			Debug.QuickLog(boundingBox);
		}

		public void UpdateCamera()
		{
			Vector2Xna position = Transform.Position.ToXna();
			viewMatrix =
				Matrix.CreateTranslation((int)Mathf.Round(-position.X), (int)Mathf.Round(-position.Y), 0) * //Position
				Matrix.CreateTranslation(boundingBox.Width / 2.0f, boundingBox.Height / 2.0f, 0) * //Offset
				Matrix.CreateScale(OrthographicScale, OrthographicScale, 1) * //Scale
				Matrix.CreateRotationZ(Transform.Rotation * Mathf.Deg2Rad); //Rotation

			boundingBox.X = (int)(viewMatrix.Translation.X * (1f / viewMatrix.Right.X)) * -1;
			boundingBox.Y = (int)(viewMatrix.Translation.Y * (1f / viewMatrix.Up.Y)) * -1;
			boundingBox.Width = (int)(1f / viewMatrix.Right.X * Screen.Width);
			boundingBox.Height = (int)(1f / viewMatrix.Up.Y * Screen.Height);
		}

		private void ScreenSizeChanged()
		{
			UpdateCamera();
		}

		/// <summary>
		/// Transforms a point from screen space into world space.
		/// </summary>
		/// <param name="screenPosition"></param>
		/// <returns></returns>
		public Vector2 ScreenToWorld(Vector2 screenPosition)
		{
			return Vector2.Transform(screenPosition, Matrix.Invert(ViewMatrix));
		}

		/// <summary>
		/// Transforms position from world space into screen space.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public Vector2 WorldToScreen(Vector2 worldPosition)
		{
			return (Vector2)Vector2Xna.Transform(worldPosition.ToXna(), ViewMatrix);
		}

		public bool InsideViewFrustrum(Vector2 point)
		{
			return boundingBox.Contains(point);
		}

		public bool InsideViewFrustrum(float x, float y) => boundingBox.Contains(x, y);

		public bool InsideViewFrustrum(Rect rect)
		{
			return boundingBox.Intersects(rect);
		}

		public bool InsideViewFrustrum(float x, float y, float width, float height)
		{
			return PhysicsModule.PhysicsIntersection.BoxBox(
				boundingBox.X, boundingBox.Y, boundingBox.Width, boundingBox.Height,
				x, y, width, height);
		}

		public bool InsideViewFrustrum(Vector2 pointA, Vector2 pointB)
		{
			float xMin = Mathf.Min(pointA.X, pointB.X);
			float yMin = Mathf.Min(pointA.Y, pointB.Y);
			float xMax = Mathf.Max(pointA.X, pointB.X);
			float yMax = Mathf.Max(pointA.Y, pointB.Y);
			return InsideViewFrustrum(xMin, yMin, xMax - xMin, yMax - yMin);
		}

		/// <summary>
		/// Returns true if the rect is contained or intersects the cameras culling bounding box.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		//public bool InsideViewFrustrum(Rectangle rect)
		//{
		//	return boundingBox.Intersects(rect);
		//}

		//[Obsolete("Does not work, yet...", false)]
		//public bool InsideViewFrustrum(Vector2Xna topLeft, Vector2Xna topRight, Vector2Xna bottomLeft, Vector2Xna bottomRight)
		//{
		//	return boundingBox.Contains(topLeft) || boundingBox.Contains(topRight) || boundingBox.Contains(bottomLeft) || boundingBox.Contains(bottomRight);
		//}

		protected override void OnDestroy()
		{
			if(mainCamera == this)
			{
				mainCamera = null;
			}
		}

		#endregion
	}
}