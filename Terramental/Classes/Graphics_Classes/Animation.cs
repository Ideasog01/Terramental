using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Terramental
{
    public class Animation
    {
        /// <summary>
        /// Use this class to add animations to sprites. This class stores all the animation's data.
        /// </summary>

        private Texture2D _spriteSheet;
        private int _frameCount;
        private float _frameDuration;
        private bool _loopActive;
        private bool _animationActive;
        private bool _nextAnimation;
        private Vector2 _frameDimensions;
        private bool _mirrorTexture;

        public Animation(Texture2D spriteSheet, int frameCount, float frameDuration, bool loopActive, Vector2 frameDimensions)
        {
            _spriteSheet = spriteSheet;
            _frameCount = frameCount;
            _frameDuration = frameDuration;
            _loopActive = loopActive;
            _animationActive = true;
            _frameDimensions = frameDimensions;
        }

        public Texture2D SpriteSheet
        {
            get { return _spriteSheet; }
            set { _spriteSheet = value; }
        }

        public int FrameCount
        {
            get { return _frameCount; }
            set { _frameCount = value; }
        }

        public float FrameDuration
        {
            get { return _frameDuration; }
            set { _frameDuration = value; }
        }

        public bool NextAnimation
        {
            get { return _nextAnimation; }
            set { _nextAnimation = value; }
        }

        public bool MirrorTexture
        {
            get { return _mirrorTexture; }
            set { _mirrorTexture = value; }
        }

        public bool LoopActive
        {
            get { return _loopActive; }
            set { _loopActive = value; }
        }

        public bool AnimationActive
        {
            get { return _animationActive; }
            set { _animationActive = value; }
        }

        public Vector2 FrameDimensions
        {
            get { return _frameDimensions; }
            set { _frameDimensions = value; }
        }
    }
}
