using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Terramental
{
    public static class AudioManager
    {
        public static List<AudioClip> audioLibrary = new List<AudioClip>();

        public static float musicVolume = 1.0f;
        public static float sfxVolume = 1.0f;

        public static bool muteMusic;
        public static bool muteSfx;

        public static void AdjustMusicVolume(bool increase)
        {
            if (increase)
            {
                if (musicVolume < 1.0f)
                {
                    musicVolume += 0.1f;
                }
            }
            else
            {
                if (musicVolume > 0.1f)
                {
                    musicVolume -= 0.1f;
                }
            }

            MediaPlayer.Volume = musicVolume;
        }

        public static void AdjustSFXVolume(bool increase)
        {
            if(increase)
            {
                if(sfxVolume < 1.0f)
                {
                    sfxVolume += 0.1f;
                }
            }
            else
            {
                if(sfxVolume > 0.1f)
                {
                    sfxVolume -= 0.1f;
                }
            }

            SoundEffect.MasterVolume = sfxVolume;
            PlaySound("BeepTone_SFX");
        }

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
