using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
                    if(audioClip.IsMusic)
                    {
                     
                    }
                    else
                    {
                        audioClip.Audio.Play();
                    }
                }
                
            }
        }
    }
}
