using CosmosEngine.CoreModule;

namespace CosmosEngine
{
	public class Rigidbody : Component, IPhysicsComponent
	{
		private Collider[] colliders;
		private bool isKinematic;
		private bool gameObjectModified;

		public bool IsKinematic { get => isKinematic; set => isKinematic = value; }
		public Collider[] Colliders
		{
			get
			{
				if(gameObjectModified)
				{
					if (colliders != null)
					{
						foreach (Collider c in colliders)
							c.IsRigidbodyCollider = false;
					}
					colliders = GameObject.GetComponents<Collider>();
					foreach (Collider c in colliders)
						c.IsRigidbodyCollider = true;
					gameObjectModified = false;
				}
				return colliders;
			}
		}

		internal override void AssignGameObject(GameObject gameObject)
		{
			base.AssignGameObject(gameObject);
			gameObject.ModifiedEvent.Add(GameObjectModified);
			gameObjectModified = true;
		}

		private void GameObjectModified(GameObjectChange change) => gameObjectModified = true;
	}
}