using Microsoft.Xna.Framework.Media;

namespace SpaceBattle
{
    internal class AudioContent
    {
        private static Song adventure;
        private static Song chill;
        private static Song epic;
        private static Song gloomyCombat;

        public static Song Adventure => adventure ??= GameManager.ContentManager.Load<Song>("Audio/Adventure");
        public static Song Chill => chill ??= GameManager.ContentManager.Load<Song>("Audio/Chill");

    }
}
