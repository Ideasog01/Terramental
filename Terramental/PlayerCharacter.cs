using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Terramental
{
    class PlayerCharacter : BaseCharacter
    {
        private Texture2D _playerTexture;

        private Rectangle _playerRectangle;

        private int _playerMovementSpeed;

        public void Initialize(Texture2D playerTexture, Rectangle playerRectangle, int playerMovementSpeed)
        {
            _playerTexture = playerTexture;
            _playerRectangle = playerRectangle;
            _playerMovementSpeed = playerMovementSpeed;
        }

        public void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _playerRectangle.X += _playerMovementSpeed;
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _playerRectangle.X -= _playerMovementSpeed;
            }

            //_playerRectangle.Y += Game1.gravity;

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_playerTexture, _playerRectangle, Color.White);
        }
    }
}
