using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    class Tile : Sprite
    {
        private bool _isGroundTile;

        private bool _isWallTile;

        public bool GroundTile
        {
            get { return _isGroundTile; }
            set { _isGroundTile = value; }
        }

        public bool WallTile
        {
            get { return _isWallTile; }
            set { _isWallTile = value; }
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

            if(_isWallTile)
            {
                if (OnCollision(playerCharacter.SpriteRectangle))
                {
                    playerCharacter.WallCollision();
                }
            }
        }
    }
}
