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
        private Vector2 position;
        private List<Transform> movement = new List<Transform>();
        protected override void Update()
        {
            foreach (Transform m in movement)
            {
                m.Transform.Translate(Vector2.Up * Time.DeltaTime);
            }

            if (!IsConnected)
                return;

            if (InputManager.GetButton("W"))
            {
                position += Vector2.Up;
            }
        }
    }
}
