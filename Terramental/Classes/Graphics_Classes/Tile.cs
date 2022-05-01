using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

        private List<Tile> neighborTiles = new List<Tile>();

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

        public bool LeftCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Right <= otherSprite.SpriteRectangle.Right && this.SpriteRectangle.Right >= otherSprite.SpriteRectangle.Left - 5 && this.SpriteRectangle.Top <= otherSprite.SpriteRectangle.Bottom - (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.SpriteRectangle.Top + (otherSprite.SpriteRectangle.Width / 4));
        }

        public bool RightCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Left >= otherSprite.SpriteRectangle.Left && this.SpriteRectangle.Left <= otherSprite.SpriteRectangle.Right + 5 && this.SpriteRectangle.Top <= otherSprite.SpriteRectangle.Bottom - (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.SpriteRectangle.Top + (otherSprite.SpriteRectangle.Width / 4));
        }

        public bool TopCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Top + this.SpriteVelocity.Y < otherSprite.SpriteRectangle.Bottom && this.SpriteRectangle.Bottom > otherSprite.SpriteRectangle.Bottom && this.SpriteRectangle.Right > otherSprite.SpriteRectangle.Left + (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Left < otherSprite.SpriteRectangle.Right - (otherSprite.SpriteRectangle.Width / 4));
        }

        public bool BottomCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Bottom + this.SpriteVelocity.Y > otherSprite.SpriteRectangle.Top && this.SpriteRectangle.Top < otherSprite.SpriteRectangle.Top && this.SpriteRectangle.Right > otherSprite.SpriteRectangle.Left + (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Left < otherSprite.SpriteRectangle.Right - (otherSprite.SpriteRectangle.Width / 4));
        }

        #endregion

        public void SetNeighborTiles()
        {
            foreach(Tile tile in MapManager.tileList)
            {
                //North Tile
                if(tile.SpritePosition == SpritePosition + new Vector2(0, -64))
                {
                    neighborTiles.Add(tile);
                }

                //South Tile
                if(tile.SpritePosition == SpritePosition + new Vector2(0, 64))
                {
                    neighborTiles.Add(tile);
                }

                //East Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(64, 0))
                {
                    neighborTiles.Add(tile);
                }

                //West Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(-64, 0))
                {
                    neighborTiles.Add(tile);
                }

                //NorthEast Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(64, -64))
                {
                    neighborTiles.Add(tile);
                }

                //SouthEast Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(64, 64))
                {
                    neighborTiles.Add(tile);
                }

                //SouthWest Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(-64, 64))
                {
                    neighborTiles.Add(tile);
                }

                //NorthWest Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(-64, -64))
                {
                    neighborTiles.Add(tile);
                }
            }
        }
    }
}
