using CosmosEngine;
using System.Drawing;

namespace SpaceBattle
{
    internal class Projectile : GameBehaviour
    {
        private float speed = 8f;
        private Sprite graphics;
        public float Speed { get => speed; set => speed = value; }
        public Sprite Graphics { get => graphics; set => graphics = value; }

        protected override void Update()
        {
            Transform.Translate(Transform.Up * Speed * Time.DeltaTime);
        }
        public static Projectile ProjectilePlayer
        {
            get
            {
                Projectile Projectile = new Projectile()
                {
                    Speed = 8f,
                    Graphics = ArtContent.ProjectilePlayer
                };
            return Projectile;
            }

        }

    }
}
