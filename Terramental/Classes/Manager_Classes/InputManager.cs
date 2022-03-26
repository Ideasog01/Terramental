using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        private KeyboardState _currentKeyboardState = Keyboard.GetState();
        private MouseState _currentMouseState = Mouse.GetState();

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
            KeyboardState oldKeyboardState = _currentKeyboardState;
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
                    playerCharacter.PlayerMovement(1, gameTime);
                }
                else if (_currentKeyboardState.IsKeyDown(Keys.A))
                {
                    playerCharacter.PlayerMovement(-1, gameTime);
                }
                else if (_currentKeyboardState.IsKeyUp(Keys.D) && _currentKeyboardState.IsKeyUp(Keys.A))
                {
                    playerCharacter.PlayerMovement(0, gameTime);
                }

                if (_currentKeyboardState.IsKeyDown(Keys.Q))
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

    }
}
