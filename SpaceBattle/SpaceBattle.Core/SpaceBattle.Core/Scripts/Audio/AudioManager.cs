using CosmosEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;


namespace SpaceBattle
{
    internal class AudioManager
    {
        public static void Play()
        {
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(AudioContent.Adventure);
            //MediaPlayer.Play(AudioContent.Epic);
            MediaPlayer.IsRepeating = true;
        }

        public static void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(AudioContent.Adventure);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

        }


    }
}
