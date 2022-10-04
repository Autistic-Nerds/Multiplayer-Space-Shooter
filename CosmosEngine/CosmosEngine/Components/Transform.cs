
using System;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace CosmosEngine
{
	public class Transform : Component
	{
		#region Fields
		private Vector2 localPosition;
		private float localRotation;
		private Vector2 localScale;
		private Transform parent;

		/// <summary>
		/// When the <see cref="CosmosEngine.Transform"/> values changes, it should be marked using Dirty() for UpdateEvent to be invoked.
		/// </summary>
		private event Action transformUpdateEvent;

		/// <summary>
		/// The world space position of the <see cref="CosmosEngine.Transform"/>.
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return Parent == null ? localPosition : Parent.Position + localPosition;
			}
			set
			{
				if (Parent == null)
				{
					localPosition = value;
				}
				else
				{
					localPosition = Parent.Position + value;
				}
				Dirty();
			}
		}
		/// <summary>
		/// The position of the <see cref="CosmosEngine.Transform"/> relative to the <see cref="CosmosEngine.Transform.Parent"/>.
		/// </summary>
		public Vector2 LocalPosition 
		{ 
			get => localPosition;
			set
			{
				localPosition = value;
				Dirty();
			}
		}
		/// <summary>
		/// The world space rotation in degree of the <see cref="CosmosEngine.Transform"/>.
		/// </summary>
		public float Rotation
		{
			get
			{
				return Parent == null ? localRotation : Parent.Rotation + localRotation;
			}
			set
			{
				if (Parent == null)
				{
					localRotation = value;
				}
				else
				{
					localRotation = Parent.Rotation + value;
				}
				Dirty();
			}
		}
		/// <summary>
		/// The rotation of the <see cref="CosmosEngine.Transform"/> relative to the <see cref="CosmosEngine.Transform.Parent"/>.
		/// </summary>
		public float LocalRotation 
		{ 
			get => localRotation;
			set
			{
				localRotation = value;
				Dirty();
			}
		}
		/// <summary>
		/// The global scale of the <see cref="CosmosEngine.Transform"/>.
		/// </summary>
		public Vector2 Scale
		{
			get
			{
				return Parent == null ? localScale : Parent.Scale * localScale;
			}
		}
		/// <summary>
		/// The scale of the <see cref="CosmosEngine.Transform"/> relative to the <see cref="CosmosEngine.Transform.Parent"/>.
		/// </summary>
		public Vector2 LocalScale
		{
			get => localScale;
			set
			{
				localScale = value;
				Dirty();
			}
		}
		/// <summary>
		/// The up direction the transform in world space.
		/// </summary>
		public Vector2 Up => Vector2.Up.Transform(Matrix.CreateRotationZ(Rotation * Mathf.Deg2Rad)).Normalized;
		/// <summary>
		/// The down direction the transform in world space.
		/// </summary>
		public Vector2 Down => Vector2.Down.Transform(Matrix.CreateRotationZ(Rotation * Mathf.Deg2Rad)).Normalized;
		/// <summary>
		/// The right direction the transform in world space.
		/// </summary>
		public Vector2 Right => Vector2.Right.Transform(Matrix.CreateRotationZ(Rotation * Mathf.Deg2Rad)).Normalized;
		/// <summary>
		/// The left direction the transform in world space.
		/// </summary>
		public Vector2 Left => Vector2.Left.Transform(Matrix.CreateRotationZ(Rotation * Mathf.Deg2Rad)).Normalized;

		/// <summary>
		/// The parent of the <see cref="Cosmos.Transform"/>.
		/// </summary>
		public Transform Parent { get => parent; set => SetParent(value); }
		/// <summary>
		/// Event invoked when the <see cref="CosmosEngine.Transform"/> position, rotation or scale changes.
		/// </summary>
		internal Action TransformUpdateEvent { get => transformUpdateEvent; set => transformUpdateEvent = value; }

		#endregion

		public Transform()
		{
			localPosition = Vector2.Zero;
			localRotation = 0.0f;
			localScale = Vector2.One;
			if (transformUpdateEvent == null)
				transformUpdateEvent = delegate { };
		}

		#region Public Methods
		internal override void AssignGameObject(GameObject gameObject)
		{
			base.AssignGameObject(gameObject);
		}

		/// <summary>
		/// Marks the <see cref="CosmosEngine.Transform"/> as dirty which will invoke <see cref="CosmosEngine.Transform.TransformUpdateEvent"/>.
		/// </summary>
		protected virtual void Dirty() => TransformUpdateEvent.Invoke();//dirty = true;

		/// <summary>
		/// Resets the <see cref="CosmosEngine.Transform"/> position, rotation and scale to default values.
		/// </summary>
		public virtual void Reset()
		{
			localPosition = Vector2.Zero;
			localRotation = 0.0f;
			localScale = Vector2.One;
			Dirty();
		}

		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Transform.SetParent(Transform, bool)"/>
		/// </summary>
		/// <param name="newParent">The parent Transform to use.</param>
		public void SetParent(Transform newParent) => SetParent(newParent, true);
		/// <summary>
		/// Set the parent of the transform. It is the same as setting the Parent property except that it also lets the <see cref="CosmosEngine.Transform"/> keep its local orientation rather than its global orientation. This means for example, if the <see cref="CosmosEngine.GameObject"/> was previously next to its parent, setting <paramref name="worldPositionStays"/> to false will move the <see cref="CosmosEngine.GameObject"/> to be positioned next to its new parent in the same way. The default value of <paramref name="worldPositionStays"/> argument is <see langword="true"/>.
		/// </summary>
		/// <param name="newParent">The parent Transform to use.</param>
		/// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
		public void SetParent(Transform newParent, bool worldPositionStays)
		{
			Vector2 previousLocalPosition = Vector2.Zero;
			Vector2 previousLocalScale = LocalScale;
			float previousLocalRotation = 0f;
			if(parent != null)
			{
				parent.TransformUpdateEvent -= TransformUpdateEvent;
				if(newParent == null)
				{
					localPosition = Position;
				}
				if(!worldPositionStays)
				{
					previousLocalPosition = LocalPosition;
					previousLocalScale = LocalScale;
					previousLocalRotation = LocalRotation;
				}
			}
			if(newParent != null)
			{
				if(worldPositionStays)
				{
					float delta = Vector2.Distance(Position, newParent.Position);
					Vector2 direction = (Position - newParent.Position).Normalized;
					localPosition = direction * delta;
				}
				else
				{
					LocalPosition = previousLocalPosition;
					LocalRotation = previousLocalRotation;
					LocalScale = previousLocalScale;
				}
				newParent.TransformUpdateEvent += TransformUpdateEvent;
			}
			parent = newParent;
			Dirty();
		}

		//public override void Update()
		//{
		//	if(dirty)
		//	{
				
		//		dirty = false;
		//	}
		//}

		/// <summary>
		/// Moves the transform in the direction and distance of <paramref name="translation"/>.
		/// </summary>
		/// <param name="translation">The translation applied to the transform's position.</param>
		public void Translate(Vector2 translation) => Translate(translation, Space.World);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Transform.Translate(Vector2)"/>
		/// </summary>
		/// <param name="translation">The translation applied to the transform's position.</param>
		/// <param name="relativeTo">If <paramref name="relativeTo"/> is set to <see cref="CosmosEngine.Space.Self"/> the movement is applied relative to the transform's local axes. If <paramref name="relativeTo"/> is set to <see cref="CosmosEngine.Space.World"/> the movement is applied relative to the world coordinate system. </param>
		public void Translate(Vector2 translation, Space relativeTo)
		{	
			if(relativeTo == Space.World)
			{
				Position += translation;
			}
			else if(relativeTo == Space.Self)
			{
				Position += translation.Transform(Rotation) * 100f;
			}
		}
		/// <summary>
		/// Moves the transform by <paramref name="x"/> along the x axis and by <paramref name="y"/> along the y axis.
		/// </summary>
		/// <param name="x">The translation applied to the transform's x axis.</param>
		/// <param name="y">The translation applied to the transform's y axis.</param>
		public void Translate(float x, float y) => Translate(x, y, Space.World);
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Transform.Translate(float, float)"/>
		/// </summary>
		/// <param name="x">The translation applied to the transform's x axis.</param>
		/// <param name="y">The translation applied to the transform's y axis.</param>
		/// <param name="relativeTo">If <paramref name="relativeTo"/> is set to <see cref="CosmosEngine.Space.Self"/> the movement is applied relative to the transform's local axes. If <paramref name="relativeTo"/> is set to <see cref="CosmosEngine.Space.World"/> the movement is applied relative to the world coordinate system. </param>
		public void Translate(float x, float y, Space relativeTo) => Translate(new Vector2(x, y), relativeTo);

		/// <summary>
		/// Rotates the transform so the up vector points towards the <paramref name="worldPosition"/>.
		/// </summary>
		/// <param name="worldPosition">Point to look at.</param>
		public void LookAt(Vector2 worldPosition)
		{
			Vector2 direction = worldPosition - Position;
			direction.Normalize();

			if (direction == Vector2.Zero)
			{
				return;
			}

			float angle = Vector2.SignedAngle(Up, direction);
			Rotation += angle;
		}

		/// <summary>
		/// Rotates the transform the given <paramref name="angle"/> in degrees.
		/// </summary>
		/// <param name="angle"></param>
		public void Rotate(float angle)
		{
			Rotation += angle;
		}

		/// <summary>
		/// Rotates the transform so the up vector points towards the <paramref name="point"/> over an amount of time scaled on <paramref name="t"/>.
		/// </summary>
		/// <param name="point">Point to look at.</param>
		/// <param name="t">Amount of degrees per second.</param>
		/// <returns>Returns <see langword="true"/> if the <see cref="CosmosEngine.Transform"/> is pointed towards <paramref name="point"/> within a narrow margin of error.</returns>
		public bool RotateTowards(Vector2 point, float t)
		{
			Vector2 direction = (point - Position).Normalized;
			if (direction == Vector2.Zero)
			{
				return true;
			}

			float angle = Vector2.SignedAngle(Up, direction);
			if (Mathf.Abs(angle) <= 1E-04f)
				return true;

			if (Mathf.Abs(angle) < t * 2f * Time.DeltaTime)
				t = t * Mathf.Clamp01(Mathf.Abs(angle), 0.0f, t * 2f * Time.DeltaTime);
			if (angle > 0.0f)
				angle = 1.0f;
			else if (angle < -0.0f)
				angle = -1.0f;
			Rotate(angle * t * Time.DeltaTime);
			return false;
		}

		/// <summary>
		/// Copies the values of <paramref name="transformToCopy"/> onto the transform.
		/// </summary>
		/// <param name="transformToCopy"></param>
		public virtual void Copy(Transform transformToCopy)
		{
			this.Position = transformToCopy.Position;
			this.Rotation = transformToCopy.Rotation;
			this.LocalScale = transformToCopy.LocalScale;
			this.Parent = transformToCopy.Parent;
		}

		/// <summary>
		///Trying to mark a Transform for destruction is not allowed. The only way to destroy a Transform component is by using DestroyImmediate. Transform should never need to be destroyed, destroying the Transform will in most scenarios reset it as any Behaviour requires a Transform to function properly.
		/// </summary>
		/// <param name="t"></param>
		internal override void MarkForDestruction(float t = 0)
		{
		}

		protected override void OnDestroy()
		{
			TransformUpdateEvent = null;
		}

		public override string ToString()
		{
			return base.ToString() + $" Position: {Position} - Rotation {Rotation:F2} Scale {Scale}";
		}

		#endregion
	}
}