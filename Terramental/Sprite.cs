using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class Sprite
    {
        private Vector2 _spritePosition;
        private Vector2 _spriteScale;
        private Texture2D _spriteTexture;
        private Rectangle _spriteRectangle;
        private Rectangle _spriteSourceRectangle;

        private List<Texture2D> _animationSpriteSheets = new List<Texture2D>();
        private int _animationIndex;
        private float _animationElapsedTime;
        private float _animationFrameDuration = 0;
        private int _animationFrameIndex;
        private int _animationFrameCount = 0;

        public void Initialise(Vector2 startPosition, Texture2D texture, Vector2 scale)
        {
            _spritePosition = startPosition;
            _spriteTexture = texture;
            _spriteScale = scale;

            SpriteManager.SpriteList.Add(this);
        }

        public Rectangle BoxCollision
        {
            get { return new Rectangle((int)_spritePosition.X, (int)_spritePosition.Y, (int)_spriteScale.X, (int)_spriteScale.Y); }
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

        public int AnimationFrameCount
        {
            set { _animationFrameCount = value; }
        }

        public float AnimationFrameDuration
        {
            set { _animationFrameDuration = value; }
        }

        public Rectangle SpriteSourceRectangle
        {
            get { return _spriteSourceRectangle; }
            set { _spriteSourceRectangle = value; }
        }

        public List<Texture2D> Animations
        {
            get { return _animationSpriteSheets; }
        }

        public int AnimationIndex
        {
            get { return _animationIndex; }
            set { _animationIndex = value; }
        }

        public void Update(GameTime gameTime)
        {
            if(_animationSpriteSheets.Count > 0)
            {
                UpdateAnimationFrames(gameTime);
            }
        }

        public void UpdateAnimationFrames(GameTime gameTime)
        {
            _animationElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(_animationElapsedTime >= _animationFrameDuration)
            {
                if(_animationFrameIndex >= _animationFrameCount - 1)
                {
                    _animationFrameIndex = 0;
                }
                else
                {
                    _animationFrameIndex++;
                }

                _animationElapsedTime = 0;
            }

            _spriteSourceRectangle = new Rectangle((_animationFrameIndex * 256), 0, 256, 512);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _spriteRectangle = new Rectangle((int)_spritePosition.X, (int)_spritePosition.Y, (int)_spriteScale.X, (int)_spriteScale.Y);

            if(_animationSpriteSheets.Count == 0)
            {
                spriteBatch.Draw(_spriteTexture, _spriteRectangle, Color.White);
            }
            else if(_animationIndex < _animationSpriteSheets.Count)
            {
                spriteBatch.Draw(_animationSpriteSheets[_animationIndex], _spriteRectangle, _spriteSourceRectangle, Color.White);
            }
        }

        public bool OnCollision(Rectangle otherObject)
        {
            if(BoxCollision.Intersects(otherObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
