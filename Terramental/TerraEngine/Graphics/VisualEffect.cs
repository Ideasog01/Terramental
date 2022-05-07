using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraEngine.Graphics
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
                SpritePosition = _attachSprite.SpritePosition + _positionOffset;
            }

            if(_vfxTimer > 0)
            {
                _vfxTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                IsActive = false;
            }
        }
    }
}
