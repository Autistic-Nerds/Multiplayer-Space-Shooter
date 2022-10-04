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
            Input.AddInputAction(100, "Player Movement", started: OnPlayerMove, null, canceled: OnPlayerMove, new InputControl[]
            {
                new InputControl(Keys.W, Interaction.Hold, Vector2.Up),
                new InputControl(Keys.Up, Interaction.Hold, Vector2.Up),
                new InputControl(Keys.S, Interaction.Hold, Vector2.Down),
                new InputControl(Keys.Down, Interaction.Hold, Vector2.Down),
                new InputControl(Keys.A, Interaction.Hold, Vector2.Left),
                new InputControl(Keys.Left, Interaction.Hold, Vector2.Left),
                new InputControl(Keys.D, Interaction.Hold, Vector2.Right),
                new InputControl(Keys.Right, Interaction.Hold, Vector2.Right)
            });
        }

        private void OnPlayerMove(CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            GameObject.Transform.Translate(movement * (Unit.Interceptor.Speed * Time.UnscaledDeltaTime));
        }
    }
}
