using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    class Sprite
    {
        private Vector2 _spritePosition;
        private Texture2D _spriteTexture;
        private Rectangle _spriteRectangle;

        public void Initialise(Vector2 startPosition, Texture2D texture)
        {
            _spritePosition = startPosition;
            _spriteTexture = texture;
        }

        public Vector2 SpritePosition
        {
            get { return _spritePosition; }
            set { _spritePosition = value; }
        }

        public Rectangle SpriteRectangle
        {
            get { return _spriteRectangle; }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _spriteRectangle = new Rectangle((int)_spritePosition.X, (int)_spritePosition.Y, 64, 64);
            spriteBatch.Draw(_spriteTexture, _spriteRectangle, Color.White);
        }
    }
}
