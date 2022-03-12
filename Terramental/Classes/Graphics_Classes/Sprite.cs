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
        private bool _isActive;
        private Sprite _attachSprite;

        private Game1 _game1;

        private List<Animation> _spriteAnimations = new List<Animation>();
        private int _animationIndex = 0;
        private float _animationElapsedTime;
        private int _animationFrameIndex;

        private float _destroyTimer;
        private bool _destructionActivated;

        public void Initialise(Vector2 startPosition, Texture2D texture, Vector2 scale, Game1 game1)
        {
            _spritePosition = startPosition;
            _spriteTexture = texture;
            _spriteScale = scale;
            _game1 = game1;
            _isActive = true;

            SpriteManager.SpriteList.Add(this);
        }

        public virtual void Destroy()
        {
            _isActive = false;
        }

        public virtual void Destroy(float destroyTime)
        {
            _destroyTimer = destroyTime;
            _destructionActivated = true;
        }

        public Game1 GameManager
        {
            get { return _game1; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public Sprite AttachSprite
        {
            get { return _attachSprite; }
            set { _attachSprite = value; }
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

        public Vector2 SpriteScale
        {
            get { return _spriteScale; }
            set { _spriteScale = value; }
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
            if(_isActive)
            {
                if (_spriteAnimations.Count > 0)
                {
                    UpdateAnimationFrames(gameTime);
                }
            }

            if(_attachSprite != null)
            {
                _spritePosition = _attachSprite._spritePosition + new Vector2(_attachSprite.SpriteRectangle.Width / 4, 0.5f);
            }

            if(_destructionActivated)
            {
                if(_destroyTimer > 0)
                {
                    _destroyTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    Destroy();
                }
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
            if(_isActive)
            {
                _spriteRectangle = new Rectangle((int)_spritePosition.X, (int)_spritePosition.Y, (int)_spriteScale.X, (int)_spriteScale.Y);

                if (_spriteAnimations.Count == 0)
                {
                    spriteBatch.Draw(_spriteTexture, _spriteRectangle, Color.White);
                }
                else if (_animationIndex < _spriteAnimations.Count)
                {
                    spriteBatch.Draw(_spriteAnimations[_animationIndex].SpriteSheet, _spriteRectangle, _spriteSourceRectangle, Color.White);
                }
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
