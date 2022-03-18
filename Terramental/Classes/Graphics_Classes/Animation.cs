using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Terramental
{
    public class Animation
    {
        private Texture2D _spriteSheet;
        private int _frameCount;
        private float _frameDuration;
        private bool _loopActive;
        private bool _animationActive;

        public Animation(Texture2D spriteSheet, int frameCount, float frameDuration, bool loopActive)
        {
            _spriteSheet = spriteSheet;
            _frameCount = frameCount;
            _frameDuration = frameDuration;
            _loopActive = loopActive;
            _animationActive = true;
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
    }
}
