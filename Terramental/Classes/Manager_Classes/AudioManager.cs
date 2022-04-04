using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Terramental
{
    public static class AudioManager
    {
        public static List<AudioClip> audioLibrary = new List<AudioClip>();

        public static void AddSound(AudioClip audioClip)
        {
            audioLibrary.Add(audioClip);
        }

        public static void PlaySound(string soundName)
        {
            foreach(AudioClip audioClip in audioLibrary)
            {
                if(soundName == audioClip.SoundName)
                {
                    if(audioClip.IsMusic && audioClip.SongAudio != null)
                    {
                        MediaPlayer.Play(audioClip.SongAudio);
                    }
                    else
                    {
                        audioClip.EffectAudio.Play();
                    }
                }
                
            }
        }
    }
}
