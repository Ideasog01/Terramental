using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Terramental
{
    public static class AudioManager
    {
        public static List<AudioClip> audioLibrary = new List<AudioClip>();

        public static float musicVolume = 0.1f;
        public static float sfxVolume = 0.5f;

        public static bool muteMusic;
        public static bool muteSfx;

        public static void SetVolumes()
        {
            SoundEffect.MasterVolume = sfxVolume;
            MediaPlayer.Volume = musicVolume;
        }

        public static void AdjustMusicVolume(bool increase)
        {
            if (increase)
            {
                if (musicVolume < 0.9f)
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
                if(sfxVolume < 0.9f)
                {
                    sfxVolume += 0.1f;
                    PlaySound("BeepTone_SFX");
                }
            }
            else
            {
                if(sfxVolume > 0.1f)
                {
                    sfxVolume -= 0.1f;
                    PlaySound("BeepTone_SFX");
                }
            }

            SoundEffect.MasterVolume = sfxVolume;
            
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

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
