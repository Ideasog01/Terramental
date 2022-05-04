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
        private bool _isVisible;
        private Vector2 _attachSpriteOffset;
        private Vector2 _spawnPosition;
        private int _layerOrder;
        private Color _spriteColor;

        //Animation Variables

        private List<Animation> _spriteAnimations = new List<Animation>();
        private int _animationIndex = 0;
        private float _animationElapsedTime;
        private int _animationFrameIndex;
        private int _previousAnimationIndex;

        #endregion

        #region Properties

        public int LayerOrder //0 is first -1 is second
        {
            get { return _layerOrder; }
            set { _layerOrder = value; }
        }

        public Color SpriteColor
        {
            get { return _spriteColor; }
            set { _spriteColor = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        public Texture2D SpriteTexture
        {
            get { return _spriteTexture; }
            set { _spriteTexture = value; }
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
            set { SpriteRectangle = value; }
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

        public Vector2 AttachSpriteOffset
        {
            get { return _attachSpriteOffset; }
            set { _attachSpriteOffset = value; }
        }

        public Vector2 SpawnPosition
        {
            get { return _spawnPosition; }
            set { _spawnPosition = value; }
        }

        #endregion

        #region Core
        public void Initialise(Vector2 startPosition, Texture2D texture, Vector2 scale)
        {
            _spritePosition = startPosition;
            _spriteTexture = texture;
            _spriteScale = scale;
            _spriteRectangle = new Rectangle((int)startPosition.X, (int)startPosition.Y, (int)scale.X, (int)scale.Y);
            _spawnPosition = startPosition;
            _spriteColor = Color.White;

            if (!SpriteManager.SpriteList.Contains(this))
            {
                SpriteManager.SpriteList.Add(this);
            }

            _isActive = true;

        }

        public void UpdateSprite(GameTime gameTime)
        {
            if(_previousAnimationIndex != _animationIndex)
            {
                _animationFrameIndex = 0;
                _previousAnimationIndex = _animationIndex;
            }

            if (_spriteAnimations.Count > 0)
            {
                UpdateAnimationFrames(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(_isActive)
            {
                _spriteRectangle = new Rectangle((int)_spritePosition.X, (int)_spritePosition.Y, (int)_spriteScale.X, (int)_spriteScale.Y);

                if (_spriteAnimations.Count != 0)
                {
                    if (!_spriteAnimations[AnimationIndex].AnimationActive)
                    {
                        spriteBatch.Draw(_spriteTexture, _spriteRectangle, Color.White);
                    }
                    else
                    {
                        if (!_spriteAnimations[_animationIndex].MirrorTexture)
                        {
                            spriteBatch.Draw(_spriteAnimations[_animationIndex].SpriteSheet, _spriteRectangle, _spriteSourceRectangle, _spriteColor);
                        }
                        else
                        {
                            spriteBatch.Draw(_spriteAnimations[_animationIndex].SpriteSheet, _spriteRectangle, _spriteSourceRectangle, _spriteColor, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
                        }
                    }
                }
                else
                {
                    spriteBatch.Draw(_spriteTexture, _spriteRectangle, Color.White);
                }
            }
        }

        #endregion

        #region DestroyFunctions

        public virtual void Destroy()
        {
            _isActive = false;
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
                            _animationFrameIndex = animation.FrameCount - 1;

                            if(animation.NextAnimation)
                            {
                                _animationIndex++;
                                animation.AnimationActive = true;
                                _animationFrameIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        _animationFrameIndex++;
                    }

                    _animationElapsedTime = 0;
                }

                _spriteSourceRectangle = new Rectangle(_animationFrameIndex * (int)animation.FrameDimensions.X, 0, (int)animation.FrameDimensions.X, (int)animation.FrameDimensions.Y);
            }
        }

        public void SetAnimation(int animationIndex)
        {
            bool mirrorTexture = false;

            if(_spriteAnimations[_animationIndex].MirrorTexture)
            {
                mirrorTexture = true;
            }

            _animationIndex = animationIndex;

            _spriteAnimations[_animationIndex].MirrorTexture = mirrorTexture;
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
