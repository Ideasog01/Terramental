using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Terramental
{
    class SpriteManager
    {
        private static List<Sprite> _spriteList = new List<Sprite>();

        public static List<Sprite> SpriteList
        {
            get { return _spriteList; }
        }

        public void Update(GameTime gameTime)
        {
            foreach(Sprite sprite in _spriteList)
            {
                sprite.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            foreach(Sprite sprite in _spriteList)
            {
                sprite.Draw(gameTime, _spriteBatch);
            }
        }

        public void DestroySprite(Sprite sprite)
        {
            _spriteList.Remove(sprite);
        }
    }
}
