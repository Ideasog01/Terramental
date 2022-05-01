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

                    if (GameManager.currentGameState == GameManager.GameState.LevelComplete || GameManager.currentGameState == GameManager.GameState.LevelPause)
                    {
                        _menuManager.ActivateLoadingScreen(2, GameManager.GameState.MainMenu);
                        GameManager.gameLoaded = false;
                    }
                    
                    if(GameManager.currentGameState == GameManager.GameState.Options)
                    {
                        if(GameManager.gameLoaded)
                        {
                            GameManager.currentGameState = GameManager.GameState.LevelPause;
                        }
                        else
                        {
                            GameManager.currentGameState = GameManager.GameState.MainMenu;
                        }
                    }

                    if(GameManager.currentGameState == GameManager.GameState.Credits)
                    {
                        GameManager.currentGameState = GameManager.GameState.MainMenu;
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
