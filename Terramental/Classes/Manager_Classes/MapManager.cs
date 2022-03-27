using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;

namespace Terramental
{
    class MapManager
    {
        public static List<Tile> tileList = new List<Tile>();

        private GameManager _gameManager;
        private MapData _mapData;

        private List<Texture2D> _tileMap1 = new List<Texture2D>();

        public MapManager(GameManager gameManager)
        {
            _gameManager = gameManager;

            LoadTextures();

            LoadMapData(@"MapData.jsn");

            if(_mapData != null)
            {
                GenerateMap();
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
                    tile.Initialise(new Vector2(x * 64, y * 64), _tileMap1[tileIndex], new Vector2(64, 64));
                    

                    bool isGround = tileIndex == 1;
                    tile.GroundTile = isGround;
                    tile.WallTile = isGround;

                    SpawnEntity(_mapData._entityMap[x, y], new Vector2(x * 64, y * 64));

                    tileList.Add(tile);

                }
            }
        }

        private void SpawnEntity(int index, Vector2 position)
        {
            if(index == 1)
            {
                SpawnManager.SpawnEnemy(0, position);
            }

            if(index == 2)
            {
                SpawnManager.SpawnHealthPickup(position);
            }

            if(index == 3)
            {
                SpawnManager.SpawnElementPickup(0, position);
            }

            if(index == 4)
            {
                SpawnManager.SpawnElementPickup(1, position);
            }

            if(index == 5)
            {
                SpawnManager.SpawnElementPickup(2, position);
            }

            if(index == 6)
            {
                SpawnManager.SpawnScorePickup(position);
            }
        }
    }
}
