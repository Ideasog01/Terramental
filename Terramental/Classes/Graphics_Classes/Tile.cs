using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class Tile : Sprite
    {
        /// <summary>
        /// Tiles make up levels and are managed by the TileManager class
        /// </summary>

        #region Variables

        private bool _isGroundTile;

        private bool _isWallTile;

        #endregion

        #region Properties
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

        #endregion

        #region CollisionDetection

        public void CheckCollision(PlayerCharacter playerCharacter)
        {
            if(_isGroundTile && !playerCharacter.IsGrounded)
            {
                if (TopCollision(playerCharacter))
                {
                    playerCharacter.GroundCollision(this);
                }
            }

            if(_isWallTile)
            {
                playerCharacter.WallCollision(LeftCollision(playerCharacter), RightCollision(playerCharacter));
            }
        }

        public bool LeftCollision(Sprite sprite)
        {
            return this.SpriteRectangle.Right + this.SpriteVelocity.X > sprite.SpriteRectangle.Left && this.SpriteRectangle.Left < sprite.SpriteRectangle.Left && this.SpriteRectangle.Bottom > sprite.SpriteRectangle.Top && this.SpriteRectangle.Top < sprite.SpriteRectangle.Bottom;
        }

        public bool RightCollision(Sprite sprite)
        {
            return this.SpriteRectangle.Left + this.SpriteVelocity.X < sprite.SpriteRectangle.Right && this.SpriteRectangle.Right > sprite.SpriteRectangle.Right && this.SpriteRectangle.Bottom > sprite.SpriteRectangle.Top && this.SpriteRectangle.Top < sprite.SpriteRectangle.Bottom;
        }

        public bool BottomCollision(Sprite sprite)
        {
            return this.SpriteRectangle.Bottom + this.SpriteVelocity.Y > sprite.SpriteRectangle.Top && this.SpriteRectangle.Top < sprite.SpriteRectangle.Top && this.SpriteRectangle.Right > sprite.SpriteRectangle.Left && this.SpriteRectangle.Left < sprite.SpriteRectangle.Right;
        }

        public bool TopCollision(Sprite sprite)
        {
            return this.SpriteRectangle.Top + this.SpriteVelocity.Y < sprite.SpriteRectangle.Bottom && this.SpriteRectangle.Bottom > sprite.SpriteRectangle.Bottom && this.SpriteRectangle.Right > sprite.SpriteRectangle.Left && this.SpriteRectangle.Left < sprite.SpriteRectangle.Right;
        }

        #endregion
    }
}
