using CosmosEngine;

namespace Cosmos.Entity
{
	public class SpriteRendererSystem : EntitySystem
	{
		private struct Component
		{
			public Transform transform;
			public SpriteRenderer spriteRenderer;
		}

		protected override void Render()
		{
			foreach(var e in GetEntities<Component>())
			{
				CosmosEngine.CoreModule.Core.SpriteBatch.Draw(
					texture: e.spriteRenderer.sprite,
					position: e.transform.position,
					sourceRectangle: null,
					color: Colour.White,
					rotation: e.transform.rotation,
					origin: new Vector2(e.spriteRenderer.sprite.Width / 2, e.spriteRenderer.sprite.Height / 2),
					scale: e.transform.scale,
					effects: Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
					layerDepth: 0);
			}
		}
	}
}