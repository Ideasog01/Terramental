using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    class MapManager
    {
        private int _mapWidth;
        private int _mapHeight;
        private int _levelIndex;
        private Texture2D _defaultTileTexture;
        private SpawnManager _spawnManager;
        private PlayerCharacter _playerCharacter;
        private List<Tile> _tileList = new List<Tile>();
        private GameManager _gameManager;

        public MapManager(int width, int height, int levelIndex, GameManager gameManager, SpawnManager spawnManager, PlayerCharacter playerCharacter)
        {
            _mapWidth = width;
            _mapHeight = height;
            _levelIndex = levelIndex;
            _defaultTileTexture = gameManager.GetTexture("Sprites/Tiles/DefaultTile");
            _spawnManager = spawnManager;
            _playerCharacter = playerCharacter;
            _gameManager = gameManager;

            GenerateMap();
        }

        public void Update()
        {
            foreach(Tile tile in _tileList)
            {
                tile.CheckCollision(_playerCharacter);
            }
        }

        private void GenerateMap()
        {
            for(int x = 0; x < _mapWidth; x++)
            {
                for(int y = 0; y < _mapHeight; y++)
                {
                    Tile tile = new Tile();
                    tile.Initialise(new Vector2(x * 64, y * 64), _defaultTileTexture, new Vector2(64, 64), _spawnManager);
                    _tileList.Add(tile);

                    if(y * 64 == 448)
                    {
                        tile.GroundTile = true;
                        tile.SpriteTexture = _gameManager.GetTexture("Sprites/Tiles/Tile-Fire");
                    }

                    if(x * 64 == 320 && y * 64 == 384)
                    {
                        tile.WallTile = true;
                        tile.SpriteTexture = _gameManager.GetTexture("Sprites/Tiles/Tile-Fire");
                    }
                }
            }
        }


    }
}
