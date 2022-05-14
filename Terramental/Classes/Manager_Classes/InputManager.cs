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

        public KeyboardState _currentKeyboardState; // Stores current keyboard state
        public KeyboardState oldKeyboardState; // Stores previous keyboard state
        private MouseState _currentMouseState; // Stores current mouse state
        private GamePadState _currentGamepadState; // Stores current controller state
        private GamePadState _oldGamepadState; // Stores previous controller state

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
            
            if(!_currentGamepadState.IsConnected) // Checks to see if there is no controller connected 
            {
                if (_currentKeyboardState.IsKeyUp(Keys.Escape) && oldKeyboardState.IsKeyDown(Keys.Escape) && GameManager.currentGameState == GameManager.GameState.Level) // If player presses escape whilst playing a level
                {
                    GameManager.PauseGame();
                    _gameManager.IsMouseVisible = true; // Sets the mouse visibility to true
                }

                if(GameManager.currentGameState == GameManager.GameState.StartScreen)
                {
                    if (_currentKeyboardState.GetPressedKeys().Length > 0) // Checks to see if keys are being pressed
                    {
                        GameManager.currentGameState = GameManager.GameState.MainMenu;
                        _menuManager.ResetMenu();

                    }
                }
            }
            else
            {

                if (_currentGamepadState.Buttons.Start == ButtonState.Pressed && _oldGamepadState.Buttons.Start == ButtonState.Released && GameManager.currentGameState == GameManager.GameState.Level)
                {
                    GameManager.PauseGame();
                    _menuManager.ResetMenu();
                    _gameManager.IsMouseVisible = true;
                }

                if(GameManager.currentGameState != GameManager.GameState.Level)
                {
                    if (_currentGamepadState.Buttons.A == ButtonState.Pressed && _oldGamepadState.Buttons.A == ButtonState.Released)
                    {
                        _menuManager.ActivateButtonController();
                    }

                    if (GameManager.currentGameState == GameManager.GameState.StartScreen)
                    {
                        if (_currentGamepadState.Buttons.A == ButtonState.Pressed && _oldGamepadState.Buttons.A == ButtonState.Released)
                        {
                            GameManager.currentGameState = GameManager.GameState.MainMenu;
                            _menuManager.ResetMenu();

                        }
                    }
                    else
                    {
                        Vector2 leftStick = _currentGamepadState.ThumbSticks.Left; // Left thumbstick using controller

                        if (leftStick.X > 0) // Checks to see if left stick is moving to the right
                        {
                            _menuManager.ChangeButtonController(true, false);
                        }
                        else if (leftStick.X < 0) // Checks to see if left stick is moving to the left
                        {
                            _menuManager.ChangeButtonController(false, false);
                        }

                        if (leftStick.Y > 0) // Checks to see if left stick is moving up
                        {
                            _menuManager.ChangeButtonController(true, true);
                        }
                        else if (leftStick.Y < 0) // Checks to see if left stick is moving down
                        {
                            _menuManager.ChangeButtonController(false, true);
                        }
                    }
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
                    _menuManager.MouseClick(CameraController.cameraTopLeftAnchor + new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y));
                }
            }

            if (_playerCharacter == null) // Checks to see if there is no player reference
            {
                if (_gameManager.playerCharacter != null) // Checks to see if GameManager has a player reference
                {
                    _playerCharacter = _gameManager.playerCharacter; // Sets the local playerCharacter to the one in GameManager
                }
                else
                {
                    return;
                }
            }

            PlayerMovementInput(gameTime);

            if(GameManager.currentGameState == GameManager.GameState.Level)
            {
                if (!_currentGamepadState.IsConnected) // Checks to see that no controller is connected
                {
                    if (_currentKeyboardState.IsKeyUp(Keys.Q) && oldKeyboardState.IsKeyDown(Keys.Q)) // If Q is pressed
                    {
                        _playerCharacter.ActivateUltimate();
                    }

                    if (_currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        _playerCharacter.PrimaryUltimateAttack();
                    }

                    if (_currentKeyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space)) // If Space is pressed
                    {
                        _playerCharacter.PlayerJump();
                    }
                }
                else // Controller is connected
                {
                    if (_currentGamepadState.IsButtonDown(Buttons.Y) && _oldGamepadState.IsButtonUp(Buttons.Y))
                    {
                        _playerCharacter.ActivateUltimate();
                    }

                    if (_currentGamepadState.IsButtonDown(Buttons.RightTrigger) && _oldGamepadState.IsButtonUp(Buttons.RightTrigger))
                    {
                        _playerCharacter.PrimaryUltimateAttack();
                    }

                    if (_currentGamepadState.Buttons.A == ButtonState.Pressed && _oldGamepadState.Buttons.A == ButtonState.Released)
                    {
                        _playerCharacter.PlayerJump();
                    }
                }

                PlayerDashInput(gameTime);
            }
        }

        public bool IsKeyPressed(Keys key) // Used to check for when a key is pressed and then released
        {
            if (_currentKeyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }

        private void PlayerMovementInput(GameTime gameTime)
        {
            if(!_currentGamepadState.IsConnected) // Checks to see that no controller is connected
            {
                if (_currentKeyboardState.IsKeyDown(Keys.D))
                {
                    _playerCharacter.PlayerMovement(1, gameTime);
                }

                if (_currentKeyboardState.IsKeyDown(Keys.A))
                {
                    _playerCharacter.PlayerMovement(-1, gameTime);
                }
            }
            else
            {
                Vector2 leftStick = _currentGamepadState.ThumbSticks.Left; // Left stick controller input

                if(leftStick.X > 0) // Right movement
                {
                    _playerCharacter.PlayerMovement(1, gameTime);
                }
                else if(leftStick.X < 0) // Left movement
                {
                    _playerCharacter.PlayerMovement(-1, gameTime);
                }
            }
        }

        private void PlayerDashInput(GameTime gameTime)
        {
            if(!_currentGamepadState.IsConnected) // Checks to see that no controller is connected
            {
                if (_gameManager.useDoubleTapDash) // Checks to see which chosen method of dashing the player has selected (shift vs double tap)
                {
                    if (IsKeyPressed(Keys.A))
                    {
                        _playerCharacter.dashDir = PlayerCharacter.DashDirections.Left; // Left Dash
                        _playerCharacter.DashStateMachine();
                    }

                    if (IsKeyPressed(Keys.D))
                    {
                        _playerCharacter.dashDir = PlayerCharacter.DashDirections.Right; // Right Dash
                        _playerCharacter.DashStateMachine();
                    }
                }
                else // Left Shift dash method
                {
                    if (IsKeyPressed(Keys.LeftShift)) // Checks to see if left shift is pressed
                    {
                        if (_playerCharacter.CanDash)
                        {
                            if (_currentKeyboardState.IsKeyDown(Keys.A)) // Left Dash
                            {
                                _playerCharacter.dashDirY = 0;
                                _playerCharacter.dashDirX = -1;
                                _playerCharacter._isDashing = true;
                            }

                            if (_currentKeyboardState.IsKeyDown(Keys.D)) // Right Dash
                            {
                                _playerCharacter.dashDirY = 0;
                                _playerCharacter.dashDirX = 1;
                                _playerCharacter._isDashing = true;
                            }
                        }

                    }
                }
            }
            else
            {
                if(_gameManager.useDoubleTapDash)
                {
                    Vector2 leftStick = _currentGamepadState.ThumbSticks.Left;
                    Vector2 oldLeftStick = _oldGamepadState.ThumbSticks.Left;

                    if (leftStick.X > 0 && oldLeftStick.X == 0) // Right Dash
                    {
                        _playerCharacter.dashDir = PlayerCharacter.DashDirections.Right;
                        _playerCharacter.DashStateMachine();
                    }

                    if (leftStick.X < 0 && oldLeftStick.X == 0) // Left Dash
                    {
                        _playerCharacter.dashDir = PlayerCharacter.DashDirections.Left;
                        _playerCharacter.DashStateMachine();
                    }
                }
                else
                {
                    if(_currentGamepadState.Buttons.B == ButtonState.Pressed && _oldGamepadState.Buttons.B == ButtonState.Released) // Checks to see if controller A button is pressed
                    {
                        Vector2 leftStick = _currentGamepadState.ThumbSticks.Left;

                        if (leftStick.X < 0) // Left Dash
                        {
                            _playerCharacter.dashDirY = 0;
                            _playerCharacter.dashDirX = -1;
                            // _playerCharacter.dashCooldown = 2;
                            _playerCharacter._isDashing = true;
                        }

                        if (leftStick.X > 0) // Right Dash
                        {
                            _playerCharacter.dashDirY = 0;
                            _playerCharacter.dashDirX = 1;
                            // _playerCharacter.dashCooldown = 2;
                            _playerCharacter._isDashing = true;
                        }
                    }
                }
            }
        }
    }
}
