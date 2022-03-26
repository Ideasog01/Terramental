using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    class Button : Sprite
    {
        private int _buttonIndex;
        private MenuManager _menuManager;

        public Button(int buttonIndex, MenuManager menuManager)
        {
            _buttonIndex = buttonIndex;
            _menuManager = menuManager;
        }

        public void CheckInteraction(Vector2 mousePos)
        {
            if(SpriteRectangle.Contains(mousePos))
            {
                _menuManager.ButtonInteraction(_buttonIndex);
            }
        }
    }
}
