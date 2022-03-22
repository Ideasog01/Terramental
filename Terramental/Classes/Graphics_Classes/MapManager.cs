using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;

namespace Terramental
{
    class MapManager
    {
        private SpawnManager _spawnManager;
        private PlayerCharacter _playerCharacter;
        private GameManager _gameManager;
        private MapData _mapData;

        private List<Texture2D> _tileMap1 = new List<Texture2D>();

        private List<Tile> _tileList = new List<Tile>();

        public MapManager(GameManager gameManager, SpawnManager spawnManager, PlayerCharacter playerCharacter)
        {
            _spawnManager = spawnManager;
            _playerCharacter = playerCharacter;
            _gameManager = gameManager;

            LoadTextures();

            LoadMapData(@"MapData.jsn");

            if(_mapData != null)
            {
                GenerateMap();
            }
        }

        public void Update()
        {
            foreach(Tile tile in _tileList)
            {
                tile.CheckCollision(_playerCharacter);
            }
        }

        public void LoadMapData(string filePath)
        {
            string strResultJson = File.ReadAllText(@"MapData.json");
            MapData newMapData = JsonConvert.DeserializeObject<MapData>(strResultJson);
            _mapData = newMapData;
        }

        private void LoadTextures()
        {
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/DefaultTile"));
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Tile-Fire"));
        }

        private void GenerateMap()
        {
            int[,] tileData = _mapData._tileMap;

            for(int x = 0; x < _mapData._mapHeight; x++)
            {
                for(int y = 0; y < _mapData._mapWidth; y++)
                {                  
                    int tileIndex = tileData[x, y];

                    Tile tile = new Tile();
                    tile.Initialise(new Vector2(x * 64, y * 64), _tileMap1[tileIndex], new Vector2(64, 64), _spawnManager);
                    

                    bool isGround = tileIndex == 1;
                    tile.GroundTile = isGround;

                    _tileList.Add(tile);

                }
            }
        }


    }
}
