using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Terramental
{
    class InputManager
    {
        private PlayerCharacter _playerCharacter;

        public InputManager(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
        }

        public void Update(GameTime gameTime)
        {
            if(_playerCharacter != null)
            {
                KeyboardMouseInput();
            }
        }

        private void KeyboardMouseInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.D))
            {
                _playerCharacter.PlayerMovement(1);
            }
            else if(keyboardState.IsKeyDown(Keys.A))
            {
                _playerCharacter.PlayerMovement(-1);
            }

            if(keyboardState.IsKeyDown(Keys.Q))
            {
                _playerCharacter.ActivateUltimate();
            }
        }
    }
}
