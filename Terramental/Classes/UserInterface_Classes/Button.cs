using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Terramental
{
    public class Button : MenuComponent
    {
        private GameManager.ButtonName _buttonName;
        private GameManager.LevelButton _levelButtonName;

        private bool _buttonActive = true;

        private MenuManager _menuManager;

        private Button positiveNeighbour;
        private Button negativeNeighbour;

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

        public Button PositiveNeighbour
        {
            get { return positiveNeighbour; }
            set { positiveNeighbour = value; }
        }

        public Button NegativeNeighbour
        {
            get { return negativeNeighbour; }
            set { negativeNeighbour = value; }
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

                        if (GameManager.currentGameState == GameManager.GameState.HelpMenu)
                        {
                            if (GameManager.levelLoaded)
                            {
                                GameManager.PauseGame();
                            }
                            else
                            {
                                GameManager.currentGameState = GameManager.GameState.MainMenu;
                            }

                            return;
                        }

                        if (GameManager.currentGameState == GameManager.GameState.Options)
                        {
                            if (GameManager.levelLoaded)
                            {
                                GameManager.PauseGame();
                            }
                            else
                            {
                                GameManager.currentGameState = GameManager.GameState.MainMenu;
                            }

                            return;
                        }

                        if (GameManager.currentGameState == GameManager.GameState.Credits || GameManager.currentGameState == GameManager.GameState.LevelSelect)
                        {
                            GameManager.currentGameState = GameManager.GameState.MainMenu;
                            return;
                        }

                        if (GameManager.currentGameState == GameManager.GameState.LevelComplete || GameManager.currentGameState == GameManager.GameState.LevelPause)
                        {
                            _menuManager.ActivateLoadingScreen(2, GameManager.GameState.MainMenu);
                            AudioManager.PlaySound("Level_Music");
                            GameManager.levelLoaded = false;
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
