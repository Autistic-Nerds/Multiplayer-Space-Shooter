using CosmosEngine;
using CosmosEngine.EventSystems;
using CosmosEngine.InputModule;
using System;

namespace SpaceBattle
{
    internal class ControlInput : GameBehaviour
    {
        protected override void Awake()
        {
            Input.AddInputAction(100, "Player Movement", null, performed: OnPlayerMove, null, new InputControl[]
            {
                new InputControl(Keys.W, Interaction.Hold, Vector2.Up),
                new InputControl(Keys.Up, Interaction.Hold, Vector2.Up),
                new InputControl(Keys.S, Interaction.Hold, Vector2.Down / 2),
                new InputControl(Keys.Down, Interaction.Hold, Vector2.Down / 2),
                new InputControl(Keys.A, Interaction.Hold, Vector2.Left / 2),
                new InputControl(Keys.Left, Interaction.Hold, Vector2.Left / 2),
                new InputControl(Keys.D, Interaction.Hold, Vector2.Right / 2),
                new InputControl(Keys.Right, Interaction.Hold, Vector2.Right / 2)
            });
            Input.AddInputAction(101, "Player Shoot", started: OnPlayerShoot, null, null, new InputControl(Keys.Space, Interaction.Press));
            Input.AddInputAction(102, "Spawn Enemy", null, performed: OnEnemySpawn, null, new InputControl(Keys.J, Interaction.Press));
        }

        private void OnPlayerMove(CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>().Normalized;
            Transform.Translate(movement * (Unit.Interceptor.Speed * Time.UnscaledDeltaTime), Space.Self);
        }

        private void OnPlayerShoot(CallbackContext context)
        {
            GameObject projectileObject = new GameObject("Projectile");
            projectileObject.AddComponent<Projectile>();
            SpriteRenderer spriteRenderer = projectileObject.AddComponent<SpriteRenderer>();
            spriteRenderer.Sprite = ArtContent.ProjectilePlayer;
            projectileObject.Transform.Position = Transform.Position;
            projectileObject.Transform.Rotation = Transform.Rotation;
        }

        private void OnEnemySpawn(CallbackContext context)
        {
            GameObject newUnit = new GameObject("EnemyInterceptor");
            newUnit.AddComponent<Unit>();
            SpriteRenderer spriteRenderer = newUnit.AddComponent<SpriteRenderer>();
            spriteRenderer.Sprite = ArtContent.InterceptorEnemy;
            OnPlayerMove();
        }
    }
}
