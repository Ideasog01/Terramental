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
