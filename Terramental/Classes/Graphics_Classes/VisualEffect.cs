using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class VisualEffect : Sprite
    {
        private Sprite _attachSprite;
        private Vector2 _positionOffset;
        private float _vfxTimer;


        public void InitialiseVFX(Sprite attachSprite, Vector2 positionOffset, float vfxTimer)
        {
            _attachSprite = attachSprite;
            _positionOffset = positionOffset;
            _vfxTimer = vfxTimer;

            IsActive = true;
        }

        public virtual void InitialiseVFX(float vfxTimer)
        {
            _vfxTimer = vfxTimer;
            IsActive = true;
        }

        public void UpdateVisualEffect(GameTime gameTime)
        {
            if(_attachSprite != null)
            {
                if(_attachSprite.Animations.Count > 0)
                {
                    if(_attachSprite.Animations[_attachSprite.AnimationIndex].MirrorTexture)
                    {
                        SpritePosition = _attachSprite.SpritePosition + (_attachSprite.SpriteScale / 2);
                    }
                    else
                    {
                        SpritePosition = _attachSprite.SpritePosition + new Vector2(0, _attachSprite.SpriteScale.Y / 2);
                    }
                }
                else
                {
                    SpritePosition = _attachSprite.SpritePosition + _positionOffset;
                }
            }

            if(_vfxTimer > 0)
            {
                _vfxTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                IsLoaded = false;
                IsActive = false;
            }
        }
    }
}
