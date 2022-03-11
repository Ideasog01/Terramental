using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    class AnimationTest : Sprite
    {
        public void AddAnimation(Texture2D texture)
        {
            Animations.Add(texture);
        }

        public void SetAnimationIndex(int index, int frameCount, float frameDuration)
        {
            AnimationIndex = index;
            AnimationFrameCount = frameCount;
            AnimationFrameDuration = frameDuration;
        }
    }
}
