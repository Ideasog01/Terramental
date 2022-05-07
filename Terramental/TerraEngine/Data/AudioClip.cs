using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Terramental;

namespace TerraEngine.Data
{
    public class AudioClip
    {
        private string _soundName;

        private SoundEffect _soundEffect;

        private Song _songEffect;

        private bool _isMusic;

        public AudioClip(string soundName, string soundFilePath, bool isMusic, GameManager gameManager)
        {
            if(isMusic)
            {
                _songEffect = gameManager.Content.Load<Song>(soundFilePath);
            }
            else
            {
                _soundEffect = gameManager.Content.Load<SoundEffect>(soundFilePath);
            }

            _isMusic = isMusic;
            _soundName = soundName;
        }

        public string SoundName
        {
            get { return _soundName; }
        }

        public SoundEffect EffectAudio
        {
            get { return _soundEffect; }
        }

        public Song SongAudio
        {
            get { return _songEffect; }
        }

        public bool IsMusic
        {
            get { return _isMusic; }
        }
    }
}
