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

        private List<Animation> _spriteAnimations = new List<Animation>();
        private int _animationIndex = 0;
        private float _animationElapsedTime;
        private int _animationFrameIndex;

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

        public List<Animation> Animations
        {
            get { return _spriteAnimations; }
        }

        public virtual void Update(GameTime gameTime)
        {
            if(_spriteAnimations.Count > 0)
            {
                UpdateAnimationFrames(gameTime);
            }
        }

        public void UpdateAnimationFrames(GameTime gameTime)
        {
            _animationElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            Animation animation = _spriteAnimations[_animationIndex];

            if(_animationElapsedTime >= animation.FrameDuration)
            {
                if(_animationFrameIndex >= animation.FrameCount - 1)
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

            if(_spriteAnimations.Count == 0)
            {
                spriteBatch.Draw(_spriteTexture, _spriteRectangle, Color.White);
            }
            else if(_animationIndex < _spriteAnimations.Count)
            {
                spriteBatch.Draw(_spriteAnimations[_animationIndex].SpriteSheet, _spriteRectangle, _spriteSourceRectangle, Color.White);
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
