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
        private int _playerMovementSpeed = 5;

        public void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.D))
            {
                SpritePosition = new Vector2(SpritePosition.X + _playerMovementSpeed, SpritePosition.Y);
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.A))
            {
                SpritePosition = new Vector2(SpritePosition.X - _playerMovementSpeed, SpritePosition.Y);
            }

            //_playerRectangle.Y += Game1.gravity;
        }
    }
}
