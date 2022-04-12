using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class Button : MenuComponent
    {
        private GameManager.ButtonName _buttonName;
        private GameManager.LevelButton _levelButtonName;
        private GameManager.GameData _gameData;
        

        private MenuManager _menuManager;

        public Button(GameManager.ButtonName buttonName, MenuManager menuManager)
        {
            _buttonName = buttonName;
            _menuManager = menuManager;
            ComponentColor = Color.White;
        }

        public Button(GameManager.LevelButton buttonName, MenuManager menuManager)
        {
            _levelButtonName = buttonName;
            _menuManager = menuManager;
            ComponentColor = Color.White;
        }

        public Button(GameManager.GameData gameData, MenuManager menuManager)
        {
            _gameData = gameData;
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

        public void CheckInteractionLevel(Vector2 mousePos)
        {
            if (ComponentRectangle.Contains(mousePos))
            {
                _menuManager.LevelSelectButtonInteraction(_levelButtonName);
            }
        }

        public void CheckInteractionLoad(Vector2 mousePos)
        {
            if (ComponentRectangle.Contains(mousePos))
            {
                SaveManager.LoadGame(_gameData);
            }
        }
    }
}
