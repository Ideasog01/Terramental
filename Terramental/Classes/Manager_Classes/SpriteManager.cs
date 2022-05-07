using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TerraEngine.Graphics;

namespace Terramental
{
    class SpriteManager
    {
        /// <summary>
        /// Controls all loaded sprites
        /// </summary>

        private static List<Sprite> _spriteList = new List<Sprite>();

        public static List<Sprite> SpriteList
        {
            get { return _spriteList; }
        }

        public void Update(GameTime gameTime)
        {
            foreach(Sprite sprite in _spriteList)
            {
                if(sprite.IsVisible && sprite.IsActive)
                {
                    sprite.UpdateSprite(gameTime);
                }

                sprite.IsVisible = CameraController.ObjectIsVisible(sprite.SpritePosition);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            foreach(Sprite sprite in _spriteList)
            {
                if(sprite.IsVisible && sprite.IsActive)
                {
                    if (sprite.LayerOrder == 0)
                    {
                        sprite.Draw(gameTime, _spriteBatch);
                    }
                }
            }

            foreach (Sprite sprite in _spriteList)
            {
                if (sprite.IsVisible && sprite.IsActive)
                {
                    if (sprite.LayerOrder == -1)
                    {
                        sprite.Draw(gameTime, _spriteBatch);
                    }
                }
            }

            foreach (Sprite sprite in _spriteList)
            {
                if (sprite.IsVisible && sprite.IsActive)
                {
                    if (sprite.LayerOrder == -2)
                    {
                        sprite.Draw(gameTime, _spriteBatch);
                    }
                }
            }
        }

        public void DestroySprite(Sprite sprite)
        {
            _spriteList.Remove(sprite);
        }
    }
}
