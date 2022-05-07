using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terramental;

namespace TerraEngine.Graphics
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
