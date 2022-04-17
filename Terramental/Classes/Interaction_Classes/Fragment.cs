using Microsoft.Xna.Framework;

namespace Terramental
{
    public class Fragment : Pickup
    {
        private MenuManager _menuManager;

        public Fragment(MenuManager menuManager, PlayerCharacter player)
        {
            _menuManager = menuManager;
            Player = player;
        }

        public void CheckFragmentCollision()
        {
            if(OnCollision(Player.SpriteRectangle) && IsActive)
            {
                _menuManager.EndLevel();
                IsActive = false;
            }
        }
    }
}