using Microsoft.Xna.Framework;
using TerraEngine.Graphics;

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

        public void ResetPickup(Vector2 position) // Resets the position of the pickup to the specified position
        {
            SpritePosition = position;
            IsActive = true; // Sets the pickup to active
        }
    }
}
