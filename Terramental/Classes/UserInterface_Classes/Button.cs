using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    class Button : MenuComponent
    {
        private GameManager.ButtonName _buttonName;
        private MenuManager _menuManager;

        public Button(GameManager.ButtonName buttonName, MenuManager menuManager)
        {
            _buttonName = buttonName;
            _menuManager = menuManager;
            ComponentColor = Color.White;
        }

        public void CheckInteraction(Vector2 mousePos)
        {
            if(ComponentRectangle.Contains(mousePos))
            {
                _menuManager.ButtonInteraction(_buttonName);
            }
        }
    }
}
