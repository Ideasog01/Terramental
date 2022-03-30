using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Terramental
{
    public class AudioClip
    {
        private string _soundName;

        private SoundEffect _soundEffect;

        private bool _isMusic;

        public AudioClip(string soundName, string soundFilePath, bool isMusic, GameManager gameManager)
        {
            _soundEffect = gameManager.Content.Load<SoundEffect>(soundFilePath);

            _isMusic = isMusic;
            _soundName = soundName;
        }

        public string SoundName
        {
            get { return _soundName; }
        }

        public SoundEffect Audio
        {
            get { return _soundEffect; }
        }

        public bool IsMusic
        {
            get { return _isMusic; }
        }
    }
}
