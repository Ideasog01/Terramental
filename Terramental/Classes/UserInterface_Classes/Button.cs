using Microsoft.Xna.Framework;
using System;

namespace Terramental
{
    public class Button : MenuComponent
    {
        private GameManager.ButtonName _buttonName;
        private GameManager.LevelButton _levelButtonName;

        private bool _buttonActive = true;

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

        public bool ButtonActive
        {
            get { return _buttonActive; }
            set { _buttonActive = value; }
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
            if(_buttonActive)
            {
                if (ComponentRectangle.Contains(mousePos))
                {
                    _menuManager.ButtonInteraction(_buttonName);

                    if(_buttonName != GameManager.ButtonName.SFXVolumeUp && _buttonName != GameManager.ButtonName.SFXVolumeDown)
                    {
                        AudioManager.PlaySound("MenuButton_SFX");
                    }

                    if (_buttonName == GameManager.ButtonName.ReturnMainMenu)
                    {
                        if (GameManager.currentGameState == GameManager.GameState.LevelPause)
                        {
                            SpawnManager._gameManager.mapManager.UnloadLevel();
                        }

                        if (GameManager.currentGameState == GameManager.GameState.LevelComplete || GameManager.currentGameState == GameManager.GameState.LevelPause)
                        {
                            _menuManager.ActivateLoadingScreen(2, GameManager.GameState.MainMenu);
                            AudioManager.PlaySound("Level_Music");
                            GameManager.levelLoaded = false;
                            _menuManager.ChangeSelectedButton(0, true);
                        }

                        if(GameManager.currentGameState == GameManager.GameState.HelpMenu)
                        {
                            if(GameManager.levelLoaded)
                            {
                                GameManager.currentGameState = GameManager.GameState.LevelPause;
                                _menuManager.ChangeSelectedButton(0, true);
                            }
                            else
                            {
                                GameManager.currentGameState = GameManager.GameState.MainMenu;
                                _menuManager.ChangeSelectedButton(0, true);
                            }
                        }

                        if (GameManager.currentGameState == GameManager.GameState.Options)
                        {
                            if (GameManager.levelLoaded)
                            {
                                GameManager.currentGameState = GameManager.GameState.LevelPause;
                                _menuManager.ChangeSelectedButton(0, true);
                            }
                            else
                            {
                                GameManager.currentGameState = GameManager.GameState.MainMenu;
                                _menuManager.ChangeSelectedButton(0, true);
                            }
                        }

                        if (GameManager.currentGameState == GameManager.GameState.Credits || GameManager.currentGameState == GameManager.GameState.LevelSelect)
                        {
                            GameManager.currentGameState = GameManager.GameState.MainMenu;
                            _menuManager.ChangeSelectedButton(0, true);
                        }
                    }
                }
            }
        }

        public void CheckInteractionLevel(Vector2 mousePos)
        {
            if(_buttonActive)
            {
                if (ComponentRectangle.Contains(mousePos))
                {
                    _menuManager.LevelSelectButtonInteraction(_levelButtonName);
                }
            }
        }
    }
}
