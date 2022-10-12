using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmosEngine;
using CosmosEngine.InputModule;
using CosmosEngine.Netcode;

namespace SpaceBattle
{
    internal class Move : NetcodeBehaviour
    {
        private float speed = 2.7f;
        private List<Transform> movement = new List<Transform>();
        protected override void Update()
        {
            foreach (Transform m in movement)
            {
                m.Transform.Translate(Vector2.Up * Time.DeltaTime);
            }

            if (!IsConnected)
                return;

            if (InputManager.GetKey(Keys.W))
            {

            }
        }

        [ClientRPC]
        private void Movement()
        {
            Rpc(nameof(OnPlayerMove));
            OnPlayerMove();
        }

        [ServerRPC]
        private void OnPlayerMove(CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>().Normalized;
            Transform.Translate(movement * (Unit.Interceptor.Speed * Time.UnscaledDeltaTime), Space.Self);
        }
    }
}
