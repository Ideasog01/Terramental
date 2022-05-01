using Microsoft.Xna.Framework;
using System;

namespace Terramental
{
    public class Button : MenuComponent
    {
        private GameManager.ButtonName _buttonName;
        private GameManager.LevelButton _levelButtonName;

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

        public GameManager.ButtonName ButtonName
        {
            get { return _buttonName; }
        }

        public GameManager.LevelButton LevelButtonName
        {
            get { return _levelButtonName; }
        }


        public void CheckInteraction(Vector2 mousePos)
        {
            if(ComponentRectangle.Contains(mousePos))
            {
                _menuManager.ButtonInteraction(_buttonName);

                if(_buttonName == GameManager.ButtonName.ReturnMainMenu)
                {
                    if (GameManager.currentGameState == GameManager.GameState.LevelPause)
                    {
                        SpawnManager._gameManager.mapManager.UnloadLevel();
                    }

                    if (!GameManager.gameLoaded || GameManager.currentGameState == GameManager.GameState.LevelPause)
                    {
                        _menuManager.DisplayMainMenu(true);
                        GameManager.gameLoaded = false;
                    }
                    else
                    {
                        GameManager.currentGameState = GameManager.GameState.LevelPause;
                    }
                }
            }
        }

        public void CheckInteractionLevel(Vector2 mousePos)
        {
            if (ComponentRectangle.Contains(mousePos))
            {
                _menuManager.LevelSelectButtonInteraction(_levelButtonName);
            }
        }
    }
}
