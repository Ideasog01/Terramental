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

        private PlayerCharacter _playerCharacter;
        private KeyboardState _currentKeyboardState = Keyboard.GetState();
        public InputManager(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
        }

        public void Update(GameTime gameTime)
        {
            if(_playerCharacter != null)
            {
                KeyboardMouseInput(gameTime);
            }
        }

        private void KeyboardMouseInput(GameTime gameTime)
        {
            KeyboardState oldState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            MouseState mouseState = Mouse.GetState();

            if (_currentKeyboardState.IsKeyDown(Keys.D))
            {
                _playerCharacter.PlayerMovement(1, gameTime);
            }
            else if (_currentKeyboardState.IsKeyDown(Keys.A))
            {
                _playerCharacter.PlayerMovement(-1, gameTime);
            }
            else if(_currentKeyboardState.IsKeyUp(Keys.D) && _currentKeyboardState.IsKeyUp(Keys.A))
            {
                _playerCharacter.PlayerMovement(0, gameTime);
            }

            if(_currentKeyboardState.IsKeyDown(Keys.Q))
            {
                _playerCharacter.ActivateUltimate();
            }

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                _playerCharacter.PrimaryAttack();
            }

            if (_currentKeyboardState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            {
                _playerCharacter.PlayerJump();
            }
        }

    }
}
