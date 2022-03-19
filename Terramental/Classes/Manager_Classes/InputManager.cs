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
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.D))
            {
                _playerCharacter.PlayerMovement(1, gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                _playerCharacter.PlayerMovement(-1, gameTime);
            }
            else if(keyboardState.IsKeyUp(Keys.D) && keyboardState.IsKeyUp(Keys.A))
            {
                _playerCharacter.PlayerMovement(0, gameTime);
            }

            if(keyboardState.IsKeyDown(Keys.Q))
            {
                _playerCharacter.ActivateUltimate();
            }

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                _playerCharacter.PrimaryAttack();
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _playerCharacter.PlayerJump();
            }
        }

    }
}
