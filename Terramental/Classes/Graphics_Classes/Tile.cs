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

        private bool _isBlocking;

        private List<Tile> _neighborTiles = new List<Tile>();

        #endregion

        #region Properties

        public bool IsBlocking
        {
            get { return _isBlocking; }
            set { _isBlocking = value; }
        }

        public List<Tile> NeighborList
        {
            get { return _neighborTiles; }
        }

        #endregion

        #region CollisionDetection

        public bool LeftCollision(Rectangle otherRectangle)
        {
            return (this.SpriteRectangle.Right <= otherRectangle.Right && this.SpriteRectangle.Right >= otherRectangle.Left - 5 && this.SpriteRectangle.Top <= otherRectangle.Bottom - (otherRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherRectangle.Top + (otherRectangle.Width / 4));
        }

        public bool RightCollision(Rectangle otherRectangle)
        {
            return (this.SpriteRectangle.Left >= otherRectangle.Left && this.SpriteRectangle.Left <= otherRectangle.Right + 5 && this.SpriteRectangle.Top <= otherRectangle.Bottom - (otherRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherRectangle.Top + (otherRectangle.Width / 4));
        }

        public bool TopCollision(Rectangle rectangle)
        {
            return (this.SpriteRectangle.Top + this.SpriteVelocity.Y < rectangle.Bottom && this.SpriteRectangle.Bottom > rectangle.Bottom && this.SpriteRectangle.Right > rectangle.Left + (rectangle.Width / 4) && this.SpriteRectangle.Left < rectangle.Right - (rectangle.Width / 4));
        }

        public bool BottomCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Bottom + this.SpriteVelocity.Y > otherSprite.SpriteRectangle.Top && this.SpriteRectangle.Top < otherSprite.SpriteRectangle.Top && this.SpriteRectangle.Right > otherSprite.SpriteRectangle.Left + (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Left < otherSprite.SpriteRectangle.Right - (otherSprite.SpriteRectangle.Width / 4));
        }

        #endregion

        public void SetNeighborTiles()
        {
            _neighborTiles.Clear();

            foreach(Tile tile in MapManager.tileList)
            {
                //North Tile
                if(tile.SpritePosition == SpritePosition + new Vector2(0, -64))
                {
                    _neighborTiles.Add(tile);
                }

                //South Tile
                if(tile.SpritePosition == SpritePosition + new Vector2(0, 64))
                {
                    _neighborTiles.Add(tile);
                }

                //East Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(64, 0))
                {
                    _neighborTiles.Add(tile);
                }

                //West Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(-64, 0))
                {
                    _neighborTiles.Add(tile);
                }

                //NorthEast Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(64, -64))
                {
                    _neighborTiles.Add(tile);
                }

                //SouthEast Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(64, 64))
                {
                    _neighborTiles.Add(tile);
                }

                //SouthWest Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(-64, 64))
                {
                    _neighborTiles.Add(tile);
                }

                //NorthWest Tile
                if (tile.SpritePosition == SpritePosition + new Vector2(-64, -64))
                {
                    _neighborTiles.Add(tile);
                }
            }
        }
    }
}
