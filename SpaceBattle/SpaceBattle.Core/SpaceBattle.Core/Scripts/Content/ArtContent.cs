using CosmosEngine;

namespace SpaceBattle
{
    internal class ArtContent
    {
        private static Sprite interceptorPlayer;
        private static Sprite interceptorEnemy;

        public static Sprite InterceptorPlayer => interceptorPlayer ??= new Sprite("Art/interceptor");
        public static Sprite InterceptorEnemy => interceptorEnemy ??= new Sprite("Art/interceptorEnemy");
    }
}
