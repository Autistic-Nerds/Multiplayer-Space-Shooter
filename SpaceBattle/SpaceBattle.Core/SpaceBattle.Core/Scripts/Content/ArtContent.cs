using CosmosEngine;

namespace SpaceBattle
{
    internal class ArtContent
    {
        private static Sprite interceptor;
        private static Sprite interceptorEnemy;
        private static Sprite projectilePlayer;
        private static Sprite projectileEnemy;

        public static Sprite Interceptor => interceptor ??= new Sprite("Art/interceptor");
        public static Sprite InterceptorEnemy => interceptorEnemy ??= new Sprite("Art/interceptorEnemy");
        public static Sprite ProjectilePlayer => projectilePlayer ??= new Sprite("Art/projectilePlayer");
        public static Sprite ProjectileEnemy => projectileEnemy ??= new Sprite("Art/projectileEnemy");
    }
}
