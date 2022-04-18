using Microsoft.Xna.Framework;

namespace Terramental
{
    public class Fragment : Pickup
    {
        private MenuManager _menuManager;

        private float _collisionTimer;

        public Fragment(MenuManager menuManager, PlayerCharacter player)
        {
            _menuManager = menuManager;
            Player = player;
        }

        public void CheckFragmentCollision(GameTime gameTime)
        {
            if(_collisionTimer > 0)
            {
                _collisionTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(OnCollision(Player.SpriteRectangle) && IsActive && _collisionTimer <= 0)
            {
                _menuManager.EndLevel();
                IsActive = false;
                _collisionTimer = 2;
            }
        }
    }
}