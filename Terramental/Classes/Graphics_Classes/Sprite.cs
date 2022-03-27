using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class Sprite
    {
        /// <summary>
        /// The fundamental class for adding visuals
        /// </summary>

        #region Variables

        //Sprite Properties
        
        private Vector2 _spritePosition;
        private Vector2 _spriteScale;
        private Vector2 _spriteVelocity;
        private Texture2D _spriteTexture;
        private Rectangle _spriteRectangle;
        private Rectangle _spriteSourceRectangle;
        private bool _isActive;
        private Sprite _attachSprite;
        private int _layerOrder;

        //Animation Variables

        private List<Animation> _spriteAnimations = new List<Animation>();
        private int _animationIndex = 0;
        private float _animationElapsedTime;
        private int _animationFrameIndex;

        //Destroy Variables

        private float _destroyTimer;
        private bool _destructionActivated;

        #endregion

        #region Properties

        public int LayerOrder //0 is first -1 is second
        {
            get { return _layerOrder; }
            set { _layerOrder = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public Texture2D SpriteTexture
        {
            get { return _spriteTexture; }
            set { _spriteTexture = value; }
        }

        public Sprite AttachSprite
        {
            get { return _attachSprite; }
            set { _attachSprite = value; }
        }

        public Vector2 SpriteVelocity
        {
            get { return _spriteVelocity; }
            set { _spriteVelocity = value; }
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

        public int AnimationIndex
        {
            get { return _animationIndex; }
            set { _animationIndex = value; }
        }

        #endregion

        #region Core
        public void Initialise(Vector2 startPosition, Texture2D texture, Vector2 scale)
        {
            _spritePosition = startPosition;
            _spriteTexture = texture;
            _spriteScale = scale;
            _isActive = true;
            _spriteRectangle = new Rectangle((int)startPosition.X, (int)startPosition.Y, (int)scale.X, (int)scale.Y);
            SpriteManager.SpriteList.Add(this);
        }

        public void UpdateSprite(GameTime gameTime)
        {
            if (_isActive)
            {
                if (_spriteAnimations.Count > 0)
                {
                    UpdateAnimationFrames(gameTime);
                }
            }

            if (_attachSprite != null)
            {
                _spritePosition = _attachSprite._spritePosition + new Vector2(_attachSprite.SpriteRectangle.Width / 4, 0.5f);
            }

            if (_destructionActivated)
            {
                if (_destroyTimer > 0)
                {
                    _destroyTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    Destroy();
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_isActive)
            {
                _spriteRectangle = new Rectangle((int)_spritePosition.X, (int)_spritePosition.Y, (int)_spriteScale.X, (int)_spriteScale.Y);

                if (_spriteAnimations.Count == 0)
                {
                    spriteBatch.Draw(_spriteTexture, _spriteRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(_spriteAnimations[_animationIndex].SpriteSheet, _spriteRectangle, _spriteSourceRectangle, Color.White);
                }
            }
        }

        #endregion

        #region DestroyFunctions

        public virtual void Destroy()
        {
            _isActive = false;
        }

        public virtual void Destroy(float destroyTime)
        {
            _destroyTimer = destroyTime;
            _destructionActivated = true;
        }

        #endregion

        #region Animation

        public void UpdateAnimationFrames(GameTime gameTime)
        {
            _animationElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            Animation animation = _spriteAnimations[_animationIndex];

            if (animation.AnimationActive)
            {
                if (_animationElapsedTime >= animation.FrameDuration)
                {
                    if (_animationFrameIndex >= animation.FrameCount - 1)
                    {
                        _animationFrameIndex = 0;

                        if (!animation.LoopActive)
                        {
                            animation.AnimationActive = false;
                        }
                    }
                    else
                    {
                        _animationFrameIndex++;
                    }

                    _animationElapsedTime = 0;
                }

                // _spriteSourceRectangle = new Rectangle((_animationFrameIndex * animation.SpriteSheet.Width / animation.FrameCount), 0, animation.SpriteSheet.Width / animation.FrameCount, animation.SpriteSheet.Height);
                _spriteSourceRectangle = new Rectangle(_animationFrameIndex * 64, 0, 64, 64);
            }
        }

        public void SetAnimation(int animationIndex)
        {
            _animationIndex = animationIndex;
        }

        public void AddAnimation(Animation animation)
        {
            _spriteAnimations.Add(animation);
        }

        #endregion

        #region Collision

        public bool OnCollision(Rectangle otherObject)
        {
            if (SpriteRectangle.Intersects(otherObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


    }
}
