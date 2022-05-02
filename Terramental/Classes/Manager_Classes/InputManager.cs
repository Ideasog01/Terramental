using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Terramental
{
    public class InputManager
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
        private MouseState _currentMouseState;
        private GamePadState _currentGamepadState;
        private GamePadState _oldGamepadState;

        private bool isTap = false;
        private double time1;
        private double time2;
        private int doubleTapKeyCooldown = 500;
        private int sameKeyDownCount = 0;
        private double lastKeyPressTime;

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

        public bool IsGamePadConnected()
        {
            return _currentGamepadState.IsConnected;
        }

        private void KeyboardMouseInput(GameTime gameTime)
        {
            oldKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            MouseState oldMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _oldGamepadState = _currentGamepadState;
            _currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if(oldKeyboardState.IsKeyDown(Keys.C) && _currentKeyboardState.IsKeyUp(Keys.C))
            {
                SpawnManager.SpawnCannonObstacle(_gameManager.playerCharacter.SpritePosition, 1);
            }
            
            if(!_currentGamepadState.IsConnected)
            {
                if (_currentKeyboardState.IsKeyUp(Keys.Escape) && oldKeyboardState.IsKeyDown(Keys.Escape) && GameManager.currentGameState == GameManager.GameState.Level)
                {
                    GameManager.PauseGame();
                    _gameManager.IsMouseVisible = true;
                }

                if(GameManager.currentGameState == GameManager.GameState.StartScreen)
                {
                    if (_currentKeyboardState.GetPressedKeys().Length > 0)
                    {
                        GameManager.currentGameState = GameManager.GameState.MainMenu;
                        _gameManager.menuManager.ChangeSelectedButton(0, true);
                    }
                }
                
            }
            else
            {
                if (GameManager.currentGameState == GameManager.GameState.StartScreen)
                {
                    if (_currentGamepadState.Buttons.A == ButtonState.Pressed && _oldGamepadState.Buttons.A == ButtonState.Released)
                    {
                        GameManager.currentGameState = GameManager.GameState.MainMenu;
                        _gameManager.menuManager.ChangeSelectedButton(0, true);
                    }
                }

                if (_currentGamepadState.Buttons.Start == ButtonState.Pressed && _oldGamepadState.Buttons.Start == ButtonState.Released && GameManager.currentGameState == GameManager.GameState.Level)
                {
                    GameManager.PauseGame();
                    _gameManager.IsMouseVisible = true;
                }

                if(_currentGamepadState.Buttons.A == ButtonState.Pressed && _oldGamepadState.Buttons.A == ButtonState.Released)
                {
                    _menuManager.InteractSelectedButton();
                }

                Vector2 leftStick = _currentGamepadState.ThumbSticks.Left;

                if (leftStick.X > 0)
                {
                    _menuManager.ChangeSelectedButton(1, false);
                }
                else if (leftStick.X < 0)
                {
                    _menuManager.ChangeSelectedButton(-1, false);
                }

                if(leftStick.Y > 0)
                {
                    _menuManager.ChangeSelectedButton(1, true);
                }
                else if(leftStick.Y < 0)
                {
                    _menuManager.ChangeSelectedButton(-1, true);
                }
            }
            
            

            if(oldMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if(GameManager.currentGameState != GameManager.GameState.Level && GameManager.currentGameState != GameManager.GameState.LevelPause)
                {
                    _menuManager.MouseClick(_playerCam.ScreenToWorldSpace(new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y)));
                }
                else
                {
                    _menuManager.MouseClick(_playerCam.CameraCentre + new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y));
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

            if(!_currentGamepadState.IsConnected)
            {
                if (_currentKeyboardState.IsKeyUp(Keys.Q) && oldKeyboardState.IsKeyDown(Keys.Q))
                {
                    _playerCharacter.ActivateUltimate();
                }

                if (_currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                {
                    _playerCharacter.PrimaryUltimateAttack();
                }

                if (_currentKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                {
                    _playerCharacter.PlayerJump();
                }
            }
            else
            {
                if(_currentGamepadState.IsButtonDown(Buttons.Y) && _oldGamepadState.IsButtonUp(Buttons.Y))
                {
                    _playerCharacter.ActivateUltimate();
                }

                if(_currentGamepadState.IsButtonDown(Buttons.RightTrigger) && _oldGamepadState.IsButtonUp(Buttons.RightTrigger))
                {
                    _playerCharacter.PrimaryUltimateAttack();
                }

                if (_currentGamepadState.Buttons.A == ButtonState.Pressed && _oldGamepadState.Buttons.A == ButtonState.Released)
                {
                    _playerCharacter.PlayerJump();
                }
            }
        }

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
            if(!_currentGamepadState.IsConnected)
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
            else
            {
                Vector2 leftStick = _currentGamepadState.ThumbSticks.Left;

                if(leftStick.X > 0)
                {
                    _playerCharacter.HorizontalAxisRaw = 1;
                    _playerCharacter.LastNonZeroHAR = _playerCharacter.HorizontalAxisRaw;
                    _playerCharacter.PlayerMovement(1, gameTime);
                }
                else if(leftStick.X < 0)
                {
                    _playerCharacter.HorizontalAxisRaw = -1;
                    _playerCharacter.LastNonZeroHAR = _playerCharacter.HorizontalAxisRaw;
                    _playerCharacter.PlayerMovement(-1, gameTime);
                }
                else if(leftStick.X == 0)
                {
                    _playerCharacter.HorizontalAxisRaw = 0;
                    _playerCharacter.PlayerMovement(0, gameTime);
                }
            }
        }

        private void PlayerDashInput(GameTime gameTime)
        {
            if(!_currentGamepadState.IsConnected)
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
            else
            {
                Vector2 leftStick = _currentGamepadState.ThumbSticks.Left;
                Vector2 oldLeftStick = _oldGamepadState.ThumbSticks.Left;

                if(leftStick.X > 0 && oldLeftStick.X == 0)
                {
                    _playerCharacter.dashDir = PlayerCharacter.DashDirections.Right;
                    _playerCharacter.DashStateMachine();
                }

                if(leftStick.X < 0 && oldLeftStick.X == 0)
                {
                    _playerCharacter.dashDir = PlayerCharacter.DashDirections.Left;
                    _playerCharacter.DashStateMachine();
                }

                if(leftStick.Y > 0 && oldLeftStick.Y == 0)
                {
                    _playerCharacter.dashDir = PlayerCharacter.DashDirections.Up;
                    _playerCharacter.DashStateMachine();
                }

                if (leftStick.Y < 0 && oldLeftStick.Y == 0)
                {
                    _playerCharacter.VerticalAxisRaw = -1;
                    _playerCharacter.LastNonZeroVAR = _playerCharacter.VerticalAxisRaw;
                }

                if(leftStick.Y == 0)
                {
                    _playerCharacter.VerticalAxisRaw = 0;
                }
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
