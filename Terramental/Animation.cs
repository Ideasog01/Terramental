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

        public Animation(Texture2D spriteSheet, int frameCount, float frameDuration)
        {
            _spriteSheet = spriteSheet;
            _frameCount = frameCount;
            _frameDuration = frameDuration;
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
    }
}
