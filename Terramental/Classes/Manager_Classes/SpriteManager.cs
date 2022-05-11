using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Terramental
{
    public class SpriteManager
    {
        /// <summary>
        /// Controls all loaded sprites
        /// </summary>

        private static List<Sprite> _spriteList = new List<Sprite>(); //Includes all sprites that have been instantiated

        public static List<Sprite> SpriteList
        {
            get { return _spriteList; }
        }

        public void Update(GameTime gameTime) //Updates Sprites for animation purposes
        {
            foreach(Sprite sprite in _spriteList)
            {
                if(sprite.IsVisible && sprite.IsActive)
                {
                    sprite.UpdateSprite(gameTime);
                }

                sprite.IsVisible = CameraController.ObjectIsVisible(sprite.SpritePosition); //Checks if the object is visible by checking if the sprite position is within the world boundaries of the camera
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch) //Draws the sprite according to it's layer order, a lower layer order will mean that the sprite will be drawn on top of other sprites that have a higher layer order value
        {
            foreach(Sprite sprite in _spriteList)
            {
                if(sprite.IsVisible && sprite.IsActive && sprite.IsLoaded)
                {
                    if (sprite.LayerOrder == 0)
                    {
                        sprite.Draw(gameTime, _spriteBatch);
                    }
                }
            }

            foreach (Sprite sprite in _spriteList)
            {
                if (sprite.IsVisible && sprite.IsActive && sprite.IsLoaded)
                {
                    if (sprite.LayerOrder == -1)
                    {
                        sprite.Draw(gameTime, _spriteBatch);
                    }
                }
            }

            foreach (Sprite sprite in _spriteList)
            {
                if (sprite.IsVisible && sprite.IsActive && sprite.IsLoaded)
                {
                    if (sprite.LayerOrder == -2)
                    {
                        sprite.Draw(gameTime, _spriteBatch);
                    }
                }
            }

            foreach (Sprite sprite in _spriteList)
            {
                if (sprite.IsVisible && sprite.IsActive && sprite.IsLoaded)
                {
                    if (sprite.LayerOrder == -3)
                    {
                        sprite.Draw(gameTime, _spriteBatch);
                    }
                }
            }
        }
    }
}
