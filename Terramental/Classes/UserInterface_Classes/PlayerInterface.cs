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

        private SpriteFont _dashCooldown;
        private SpriteFont _ultimateCooldown;
        private SpriteFont _playerScore;

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
            foreach(InterfaceComponent component in playerInterfaceElements)
            {
                component.FollowCamera();
            }
        }

        public void DrawInterface(SpriteBatch spriteBatch)
        {
            foreach(InterfaceComponent component in playerInterfaceElements)
            {
                component.DrawComponent(spriteBatch);
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

        public void DrawCooldownTexts(SpriteBatch spriteBatch)
        {
            if(interfaceActive)
            {
                if(_gameManager.playerCharacter.dashCooldown > 0)
                {
                    spriteBatch.DrawString(_dashCooldown, _gameManager.playerCharacter.dashCooldown.ToString("F0"), _dashAbility.ComponentPosition + new Vector2(15, 14), Color.Black);
                }

                if(_gameManager.playerCharacter.ultimateCooldown > 0)
                {
                    spriteBatch.DrawString(_ultimateCooldown, _gameManager.playerCharacter.ultimateCooldown.ToString("F0"), _ultimateAbility.ComponentPosition + new Vector2(15, 14), Color.Black);
                }

                spriteBatch.DrawString(_playerScore, _gameManager.playerCharacter.PlayerScore.ToString(), _playerScoreComponent.ComponentPosition + new Vector2(40, 5), Color.Black);
                
                
            }
        }

        private void LoadPlayerInterface()
        {
            _currentElement = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(400, -200), new Vector2(64, 64), _gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element"));
            playerInterfaceElements.Add(_currentElement);

            _firstElement = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(412, -240), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Snow_Element"));
            playerInterfaceElements.Add(_firstElement);

            _secondElement = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(465, -220), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Water_Element"));
            playerInterfaceElements.Add(_secondElement);

            _dashAbility = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(465, -170), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Dash_Element"));
            playerInterfaceElements.Add(_dashAbility);

            _ultimateAbility = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(412, -138), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Special_PowerUp"));
            playerInterfaceElements.Add(_ultimateAbility);

            _firstLife = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(-440, -230), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Health"));
            playerInterfaceElements.Add(_firstLife);

            _secondLife = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(-400, -230), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Health"));
            playerInterfaceElements.Add(_secondLife);

            _thirdLife = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(-360, -230), new Vector2(42, 42), _gameManager.GetTexture("UserInterface/PlayerInterface/Health"));
            playerInterfaceElements.Add(_thirdLife);

            _playerScoreComponent = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(-435, -180), new Vector2(32, 32), _gameManager.GetTexture("Sprites/Pickups/Collectible"));
            playerInterfaceElements.Add(_playerScoreComponent);

            _dashCooldown = _gameManager.Content.Load<SpriteFont>("SpriteFont/DefaultFont");
            _ultimateCooldown = _gameManager.Content.Load<SpriteFont>("SpriteFont/DefaultFont");
            _playerScore = _gameManager.Content.Load<SpriteFont>("SpriteFont/DefaultFont");

            interfaceActive = true;
        }
    }
}
