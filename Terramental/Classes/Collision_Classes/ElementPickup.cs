using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class ElementPickup : Pickup
    {
        private Texture2D _fireTexture;
        private Texture2D _waterTexture;
        private Texture2D _snowTexture;

        private int _elementIndex;

        private float _pickupTimer;

        public ElementPickup(int elementIndex, Texture2D fireTexture, Texture2D waterTexture, Texture2D snowTexture, PlayerCharacter playerCharacter)
        {
            _fireTexture = fireTexture;
            _waterTexture = waterTexture;
            _snowTexture = snowTexture;

            Animations.Add(new Animation(_fireTexture, 4, 120f, true));
            Animations.Add(new Animation(_waterTexture, 4, 120f, true));
            Animations.Add(new Animation(_snowTexture, 4, 120f, true));

            _elementIndex = elementIndex;

            Player = playerCharacter;

            SetAnimation(_elementIndex);
        }

        public void CheckElementPickupCollision(GameTime gameTime)
        {
            if(Player != null & IsActive && Player.ElementIndex != _elementIndex && _pickupTimer <= 0)
            {
                if (OnCollision(Player.SpriteRectangle))
                {
                    ChangeElement(Player.ElementIndex);
                }
            }

            if(_pickupTimer > 0)
            {
                _pickupTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void ChangeElement(int index)
        {
            Player.ElementIndex = _elementIndex;
            _elementIndex = index;
            _pickupTimer = 1.2f;

            ChangeTexture();
        }

        private void ChangeTexture()
        {
            SetAnimation(_elementIndex);
        }
    }
}
