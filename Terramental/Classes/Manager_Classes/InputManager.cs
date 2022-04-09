using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Terramental
{
    class InputManager
    {
        /// <summary>
        /// InputManager controls all essential player input
        /// </summary>

        private MenuManager _menuManager;
        private CameraController _playerCam;
        private GameManager _gameManager;
        private PlayerCharacter _playerCharacter;

        public KeyboardState _currentKeyboardState;
        public KeyboardState oldKeyboardState;
        private MouseState _currentMouseState = Mouse.GetState();

        private bool isTap = false;
        private double time1;
        private double time2;
        private int doubleTapKeyCooldown = 500;
        private int sameKeyDownCount = 0;
        private double lastKeyPressTime;

        // private int dashCheck = 0;

        public InputManager(CameraController playerCam, MenuManager menuManager, GameManager gameManager)
        {
            _playerCam = playerCam;
            _menuManager = menuManager;
            _gameManager = gameManager;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardMouseInput(gameTime);
        }

        private void KeyboardMouseInput(GameTime gameTime)
        {
            oldKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            MouseState oldMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            

            if(_currentMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                _menuManager.MouseClick(_playerCam.CameraCentre + new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y));

                if(DialogueManager.dialogueActive)
                {
                    if(_gameManager.dialogueManager != null)
                    {
                        _gameManager.dialogueManager.CheckButtonInteraction(_playerCam.CameraCentre + new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y));             
                    }
                }
            }

            if (_playerCharacter == null)
            {
                if (_gameManager.playerCharacter != null)
                {
                    _playerCharacter = _gameManager.playerCharacter;
                }
                else
                {
                    return;
                }
            }

            PlayerMovementInput(gameTime);
            PlayerDashInput(gameTime);

            if (_currentKeyboardState.IsKeyUp(Keys.Q) && oldKeyboardState.IsKeyDown(Keys.Q))
            {
                _playerCharacter.ActivateUltimate();
            }

            if (_currentMouseState.LeftButton == ButtonState.Pressed)
            {
                _playerCharacter.PrimaryAttack();
            }

            if (_currentKeyboardState.IsKeyUp(Keys.Space) && oldKeyboardState.IsKeyDown(Keys.Space))
            {
                _playerCharacter.PlayerJump();
            }
        }

        
        // public Keys lastSuccessfulKeyPress;
        public bool IsKeyPressed(Keys key)
        {
            if (_currentKeyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key))
            {
                // lastSuccessfulKeyPress = key;
                // lastSuccessfulKeyPressTime = gameTime.ElapsedGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }

        private void PlayerMovementInput(GameTime gameTime)
        {
            if (_currentKeyboardState.IsKeyDown(Keys.D))
            {
                _playerCharacter.HorizontalAxisRaw = 1;
                _playerCharacter.LastNonZeroHAR = _playerCharacter.HorizontalAxisRaw;
                _playerCharacter.PlayerMovement(1, gameTime);
            }

            if (_currentKeyboardState.IsKeyDown(Keys.A))
            {
                _playerCharacter.HorizontalAxisRaw = -1;
                _playerCharacter.LastNonZeroHAR = _playerCharacter.HorizontalAxisRaw;
                _playerCharacter.PlayerMovement(-1, gameTime);
            }

            if (_currentKeyboardState.IsKeyUp(Keys.D) && _currentKeyboardState.IsKeyUp(Keys.A))
            {
                _playerCharacter.HorizontalAxisRaw = 0;
                _playerCharacter.PlayerMovement(0, gameTime);
            }
        }

        private void PlayerDashInput(GameTime gameTime)
        {
            if (IsKeyPressed(Keys.W))
            {
                _playerCharacter.dashDir = PlayerCharacter.DashDirections.Up;
                _playerCharacter.DashStateMachine();
            }

            if (IsKeyPressed(Keys.A))
            {
                _playerCharacter.dashDir = PlayerCharacter.DashDirections.Left;
                _playerCharacter.DashStateMachine();
            }

            if (IsKeyPressed(Keys.D))
            {
                _playerCharacter.dashDir = PlayerCharacter.DashDirections.Right;
                _playerCharacter.DashStateMachine();
            }

            if (_currentKeyboardState.IsKeyDown(Keys.S))
            {
                _playerCharacter.VerticalAxisRaw = -1;
                _playerCharacter.LastNonZeroVAR = _playerCharacter.VerticalAxisRaw;
            }

            if (!_currentKeyboardState.IsKeyDown(Keys.W) && !_currentKeyboardState.IsKeyDown(Keys.S))
            {
                _playerCharacter.VerticalAxisRaw = 0;
            }
        }

        /*

        
        double clickTimer;
        public bool IsKeyDoublePressed(Keys key, GameTime gameTime)
        {
            double doublePressTime = 500;
            // clickTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            Debug.WriteLine("current" + key);
            Debug.WriteLine("last " + lastSuccessfulKeyPress);
            if (IsKeyPressed(key, gameTime) && key == lastSuccessfulKeyPress) // && lastKeyPressed == key)
            {
                if (clickTimer < doublePressTime) // double click
                {
                    return true;
                }
                else // normal click
                {

                }
                clickTimer = 0;
            }
            return false;    
        }
        */
    }

}
