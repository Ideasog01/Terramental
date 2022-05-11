using Microsoft.Xna.Framework;

namespace Terramental
{
    public class Fragment : Pickup
    {
        private MenuManager _menuManager;

        private float _collisionTimer;

        public Fragment(MenuManager menuManager, PlayerCharacter player) // Fragment constructor
        {
            _menuManager = menuManager;
            Player = player;
        }

        public void CheckFragmentCollision(GameTime gameTime)
        {
            if(_collisionTimer > 0) // Checks to see that the collision timer is greater than 0
            {
                _collisionTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(OnCollision(Player.SpriteRectangle) && IsActive && _collisionTimer <= 0) // Checks for a collision between this and the player
            {
                AudioManager.PlaySound("Positive_SFX");
                _menuManager.EndLevel();
                IsActive = false; // Removes fragment object
                _collisionTimer = 2; // Resets collision timer
            }
        }
    }
}