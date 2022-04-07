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

        public PlayerCharacter playerCharacter;
        private MenuManager _menuManager;
        private CameraController _playerCam;

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

        public InputManager(CameraController playerCam, MenuManager menuManager)
        {
            _playerCam = playerCam;
            _menuManager = menuManager;
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
                _menuManager.MouseClick(_playerCam.ScreenToWorldSpace(new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y)));
            }

            if(playerCharacter != null)
            {
                if (_currentKeyboardState.IsKeyDown(Keys.D))
                {
                    playerCharacter.HorizontalAxisRaw = 1;
                    playerCharacter.LastNonZeroHAR = playerCharacter.HorizontalAxisRaw;
                    playerCharacter.PlayerMovement(playerCharacter.HorizontalAxisRaw, gameTime);
                }
                if (_currentKeyboardState.IsKeyDown(Keys.A))
                {
                    playerCharacter.HorizontalAxisRaw = -1;
                    playerCharacter.LastNonZeroHAR = playerCharacter.HorizontalAxisRaw;
                    playerCharacter.PlayerMovement(playerCharacter.HorizontalAxisRaw, gameTime);
                }
                if (_currentKeyboardState.IsKeyUp(Keys.D) && _currentKeyboardState.IsKeyUp(Keys.A))
                {
                    playerCharacter.HorizontalAxisRaw = 0;
                    playerCharacter.PlayerMovement(playerCharacter.HorizontalAxisRaw, gameTime);
                }

                
                if (IsKeyPressed(Keys.W, gameTime))
                {
                    playerCharacter.dashDir = PlayerCharacter.DashDirections.Up;
                    playerCharacter.DashStateMachine();
                }
                if (IsKeyPressed(Keys.A, gameTime))
                {
                    playerCharacter.dashDir = PlayerCharacter.DashDirections.Left;
                    playerCharacter.DashStateMachine();
                }
                if (IsKeyPressed(Keys.D, gameTime))
                {
                    playerCharacter.dashDir = PlayerCharacter.DashDirections.Right;
                    playerCharacter.DashStateMachine();
                }


                if (_currentKeyboardState.IsKeyDown(Keys.S))
                {
                    playerCharacter.VerticalAxisRaw = -1;
                    playerCharacter.LastNonZeroVAR = playerCharacter.VerticalAxisRaw;
                }

                if (!_currentKeyboardState.IsKeyDown(Keys.W) && !_currentKeyboardState.IsKeyDown(Keys.S))
                {
                    playerCharacter.VerticalAxisRaw = 0;

                }

                if (_currentKeyboardState.IsKeyUp(Keys.Q) && oldKeyboardState.IsKeyDown(Keys.Q))
                {
                    playerCharacter.ActivateUltimate();
                }

                if (_currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    playerCharacter.PrimaryAttack();
                }

                if (_currentKeyboardState.IsKeyUp(Keys.Space) && oldKeyboardState.IsKeyDown(Keys.Space))
                {
                    playerCharacter.PlayerJump();
                }
            }
        }

        
        // public Keys lastSuccessfulKeyPress;
        public bool IsKeyPressed(Keys key, GameTime gameTime)
        {
            if (_currentKeyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key))
            {
                // lastSuccessfulKeyPress = key;
                // lastSuccessfulKeyPressTime = gameTime.ElapsedGameTime.TotalMilliseconds;
                return true;
            }
            return false;
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
