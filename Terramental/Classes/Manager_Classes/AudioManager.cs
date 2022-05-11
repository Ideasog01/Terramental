using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Terramental
{
    public static class AudioManager
    {
        public static List<AudioClip> audioLibrary = new List<AudioClip>(); //The list of audio containing all instantiated AudioClips

        public static float musicVolume = 0.2f; //The default music volume
        public static float sfxVolume = 0.5f; //The default sound effects volume

        public static void SetVolumes() //Sets the initial volumes for sound effects and music
        {
            SoundEffect.MasterVolume = sfxVolume;
            MediaPlayer.Volume = musicVolume;
        }

        public static void AdjustMusicVolume(bool increase) //Adjusts the volume of music based on the increase boolean. True == Increase, False == Decrease. 
        {
            if (increase)
            {
                if (musicVolume < 0.9f) //Avoids the musicVolume variable to be larger than the maximum volume of 1
                {
                    musicVolume += 0.1f;
                }
            }
            else
            {
                if (musicVolume != 0f) //Avoids the musicVolume variable to be smaller than the minimum volume of 0
                {
                    musicVolume -= 0.1f;
                }
            }

            if(musicVolume >= 0 && musicVolume <= 1)
            {
                MediaPlayer.Volume = musicVolume;
            }

        }

        public static void AdjustSFXVolume(bool increase) //Adjusts the volume of sound efffects based on the increase boolean. True == Increase, False == Decrease.
        {
            if(increase)
            {
                if(sfxVolume < 0.9f) //Avoids the sfxVolume variable to be larger than the maximum volume of 1
                {
                    sfxVolume += 0.1f;
                    PlaySound("BeepTone_SFX");
                }
            }
            else
            {
                if(sfxVolume != 0f) //Avoids the sfxVolume variable to be smaller than the minimum volume of 0
                {
                    sfxVolume -= 0.1f;
                    PlaySound("BeepTone_SFX");
                }
            }

            if(sfxVolume >= 0 && sfxVolume <= 1)
            {
                SoundEffect.MasterVolume = sfxVolume;
            }
        }

        public static void AddSound(AudioClip audioClip) //Adds the AudioClip to the audioLibrary list
        {
            audioLibrary.Add(audioClip);
        }

        public static void PlaySound(string soundName) //Plays a sound based on the string variable contents. 
        {
            foreach(AudioClip audioClip in audioLibrary)
            {
                if(soundName == audioClip.SoundName) //If the string does not match any audioClip.SoundName, no sound will play.
                {
                    if(audioClip.IsMusic && audioClip.SongAudio != null) //If the audio clip is music, use the MediaPlayer class to play the identified audio
                    {
                        MediaPlayer.Play(audioClip.SongAudio);
                        MediaPlayer.IsRepeating = true;
                    }
                    else
                    {
                        audioClip.EffectAudio.Play(); //Play the identified sound effect
                    }

                    break;
                }
                
            }
        }

        public static void StopMusic() //Stops the music completely. The music will start at the beginning if the MediaPlayer.Play() is called. If this is not the desired effect, use MediaPlayer.Resume()
        {
            MediaPlayer.Stop();
        }
    }
}
