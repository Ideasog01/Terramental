using Microsoft.Xna.Framework;

namespace Terramental
{
    public class Pickup : Sprite
    {
        private PlayerCharacter _playerCharacter;

        public PlayerCharacter Player
        {
            get { return _playerCharacter; }
            set { _playerCharacter = value; }
        }

        public void ResetPickup(Vector2 position)
        {
            SpritePosition = position;
            IsActive = true;
        }
    }
}
