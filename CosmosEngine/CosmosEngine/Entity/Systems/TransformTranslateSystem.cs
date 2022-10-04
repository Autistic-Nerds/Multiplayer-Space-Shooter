namespace Cosmos.Entity
{
	public class TransformTranslateSystem : EntitySystem
	{
		private struct Components
		{
			public Transform transform;
			public Translate translate;
		}

		protected override void Update()
		{
			foreach (var e in GetEntities<Components>())
			{
				e.transform.position += e.translate.translation;
			}
		}
	}
}