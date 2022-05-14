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
        private int _originalElementIndex;

        public ElementPickup(int elementIndex, Texture2D fireTexture, Texture2D waterTexture, Texture2D snowTexture, PlayerCharacter playerCharacter) // ElementPickup constructor
        {
            _fireTexture = fireTexture;
            _waterTexture = waterTexture;
            _snowTexture = snowTexture;

            Animations.Add(new Animation(_fireTexture, 4, 120f, true, new Vector2(64, 64)));
            Animations.Add(new Animation(_waterTexture, 4, 120f, true, new Vector2(64, 64)));
            Animations.Add(new Animation(_snowTexture, 4, 120f, true, new Vector2(64, 64)));

            _elementIndex = elementIndex;
            _originalElementIndex = elementIndex;

            Player = playerCharacter;

            SetAnimation(_elementIndex);
        }

        public int ElementIndex
        {
            get { return _elementIndex; }
            set { _elementIndex = value; _originalElementIndex = value; }
        }

        public void ResetElementPickup() // Resets the element pickup to the starting element
        {
            _elementIndex = _originalElementIndex;
            ChangeTexture();
        }

        public void CheckElementPickupCollision(GameTime gameTime)
        {
            if(Player != null & IsActive && Player.ElementIndex != _elementIndex && _pickupTimer <= 0) // Checks to see if the required conditions are met before checking for a collision
            {
                if (OnCollision(Player.SpriteRectangle) && !Player.ultimateActive) // Checks for a collision between the element pickup and the player
                {
                    ChangeElement(Player.ElementIndex);
                }
            }

            if(_pickupTimer > 0)
            {
                _pickupTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreased pickup timer
            }
        }

        private void ChangeElement(int index) // Changes the element to the one at the specified index
        {
            Player.ElementIndex = _elementIndex;
            _elementIndex = index;
            _pickupTimer = 1.2f; // Resets the pickup timer
            Player.ultimateCooldown = 0;

            ChangeTexture();
        }

        private void ChangeTexture() // Updates the texture
        {
            SetAnimation(_elementIndex);
        }
    }
}
