using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    class Tile : Sprite
    {
        private bool _isGroundTile;

        public bool GroundTile
        {
            get { return _isGroundTile; }
            set { _isGroundTile = value; }
        }

        public void CheckCollision(PlayerCharacter playerCharacter)
        {
            if(_isGroundTile && !playerCharacter.IsGrounded)
            {
                if (OnCollision(playerCharacter.SpriteRectangle))
                {
                    playerCharacter.IsGrounded = true;
                }
            }
        }
    }
}
