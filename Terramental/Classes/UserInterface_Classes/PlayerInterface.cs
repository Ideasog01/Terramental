using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Terramental
{
    public class PlayerInterface
    {

        public static bool interfaceActive;

        public static List<InterfaceComponent> playerInterfaceElements = new List<InterfaceComponent>();

        private GameManager _gameManager;

        private InterfaceComponent _currentElement;
        private InterfaceComponent _firstElement;
        private InterfaceComponent _secondElement;

        private InterfaceComponent _dashAbility;
        private InterfaceComponent _ultimateAbility;

        private InterfaceComponent _firstLife;
        private InterfaceComponent _secondLife;
        private InterfaceComponent _thirdLife;
        private InterfaceComponent _playerScoreComponent;

        private InterfaceComponent _ultimateSlider;
        private InterfaceComponent _ultimateSliderFill;

        private SpriteFont _defaultFont;

        public PlayerInterface(GameManager gameManager)
        {
            _gameManager = gameManager;

            LoadPlayerInterface();
        }

        public void HidePlayerInterface(bool isActive)
        {
            if(isActive)
            {
                //Display
            }
            else
            {
                //Hide
            }
        }

        public void UpdatePlayerInterface()
        {
            if (_gameManager.playerCharacter.ultimateActive)
            {
                _ultimateSliderFill.ComponentScale = new Vector2(40 * _gameManager.playerCharacter.ultimateActiveTimer, 40);

                switch (_gameManager.playerCharacter.ElementIndex)
                {
                    case 0:
                        _ultimateSliderFill.ComponentColor = Color.IndianRed;
                        break;
                    case 1:
                        _ultimateSliderFill.ComponentColor = Color.DeepSkyBlue;
                        break;
                    case 2:
                        _ultimateSliderFill.ComponentColor = Color.AliceBlue;
                        break;
                }
            }
        }

        public void DrawInterface(SpriteBatch spriteBatch)
        {
            foreach(InterfaceComponent component in playerInterfaceElements)
            {
                component.DrawComponent(spriteBatch);
            }

            if(_gameManager.playerCharacter.ultimateActive)
            {
                _ultimateSlider.DrawComponent(spriteBatch);
                _ultimateSliderFill.DrawComponent(spriteBatch);
            }
        }

        public void UpdatePlayerLives(int livesAmount)
        {
            switch(livesAmount)
            {
                case 0:
                    _firstLife.ComponentColor = Color.Gray;
                    _secondLife.ComponentColor = Color.Gray;
                    _thirdLife.ComponentColor = Color.Gray;
                    break;
                case 1:
                    _firstLife.ComponentColor = Color.White;
                    _secondLife.ComponentColor = Color.Gray;
                    _thirdLife.ComponentColor = Color.Gray;
                    break;
                case 2:
                    _firstLife.ComponentColor = Color.White;
                    _secondLife.ComponentColor = Color.White;
                    _thirdLife.ComponentColor = Color.Gray;
                    break;
                case 3:
                    _firstLife.ComponentColor = Color.White;
                    _secondLife.ComponentColor = Color.White;
                    _thirdLife.ComponentColor = Color.White;
                    break;
            }
        }

        public void UpdateElementDisplay()
        {
            if(_gameManager.playerCharacter.ElementIndex == 0)
            {
                playerInterfaceElements[0].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element"), new Vector2(400, -200), new Vector2(64, 64));
                playerInterfaceElements[1].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Water_Element"), new Vector2(412, -240), new Vector2(42, 42));
                playerInterfaceElements[2].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Snow_Element"), new Vector2(465, -220), new Vector2(42, 42));
            }
            else if(_gameManager.playerCharacter.ElementIndex == 1)
            {
                playerInterfaceElements[0].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Water_Element"), new Vector2(400, -200), new Vector2(64, 64));
                playerInterfaceElements[1].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Snow_Element"), new Vector2(412, -240), new Vector2(42, 42));
                playerInterfaceElements[2].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element"), new Vector2(465, -220), new Vector2(42, 42));
            }
            else if(_gameManager.playerCharacter.ElementIndex == 2)
            {
                playerInterfaceElements[0].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Snow_Element"), new Vector2(400, -200), new Vector2(64, 64));
                playerInterfaceElements[1].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element"), new Vector2(412, -240), new Vector2(42, 42));
                playerInterfaceElements[2].SetProperties(_gameManager.GetTexture("UserInterface/PlayerInterface/Water_Element"), new Vector2(465, -220), new Vector2(42, 42));
            }
        }

        public void DrawCooldownTexts(SpriteBatch spriteBatch)
        {
            if(interfaceActive)
            {
                if(_gameManager.playerCharacter.dashCooldown > 0)
                {
                    spriteBatch.DrawString(_defaultFont, _gameManager.playerCharacter.dashCooldown.ToString("F0"), _dashAbility.ComponentPosition + new Vector2(15, 14), Color.Black);
                }

                if(_gameManager.playerCharacter.ultimateCooldown > 0)
                {
                    spriteBatch.DrawString(_defaultFont, _gameManager.playerCharacter.ultimateCooldown.ToString("F0"), _ultimateAbility.ComponentPosition + new Vector2(15, 14), Color.Black);
                }

                spriteBatch.DrawString(_defaultFont, _gameManager.playerCharacter.PlayerScore.ToString(), _playerScoreComponent.ComponentPosition + new Vector2(40, 5), Color.Black);
                
                
            }
        }

        private void LoadPlayerInterface()
        {
            _currentElement = new InterfaceComponent(new Vector2(846, 26), new Vector2(64, 64), _gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element"));
            playerInterfaceElements.Add(_currentElement);

            _firstElement = new InterfaceComponent(new Vector2(910, 8), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Snow_Element"));
            playerInterfaceElements.Add(_firstElement);

            _secondElement = new InterfaceComponent(new Vector2(910, 46), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Water_Element"));
            playerInterfaceElements.Add(_secondElement);

            _dashAbility = new InterfaceComponent(new Vector2(906, 84), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Dash_Element"));
            playerInterfaceElements.Add(_dashAbility);

            _ultimateAbility = new InterfaceComponent(new Vector2(860, 92), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Special_PowerUp"));
            playerInterfaceElements.Add(_ultimateAbility);

            _firstLife = new InterfaceComponent(new Vector2(5, 5), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Health"));
            playerInterfaceElements.Add(_firstLife);

            _secondLife = new InterfaceComponent(new Vector2(49, 5), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Health"));
            playerInterfaceElements.Add(_secondLife);

            _thirdLife = new InterfaceComponent(new Vector2(92, 5), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Health"));
            playerInterfaceElements.Add(_thirdLife);

            _playerScoreComponent = new InterfaceComponent(new Vector2(10, 56), new Vector2(32, 32), _gameManager.GetTexture("Sprites/Pickups/Collectible"));
            playerInterfaceElements.Add(_playerScoreComponent);

            _ultimateSlider = new InterfaceComponent(new Vector2((GameManager.screenWidth / 2) - 200, (GameManager.screenHeight / 2) - 240), new Vector2(400, 40), _gameManager.GetTexture("UserInterface/Sliders/UltimateSliderBorder"));

            _ultimateSliderFill = new InterfaceComponent(_ultimateSlider.ComponentPosition, new Vector2((GameManager.screenWidth / 2) - 200, (GameManager.screenHeight / 2) - 240), _gameManager.GetTexture("UserInterface/Sliders/BlankSliderFill"));

            _defaultFont = _gameManager.Content.Load<SpriteFont>("SpriteFont/DefaultFont");

            interfaceActive = true;
        }
    }
}
