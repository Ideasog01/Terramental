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
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/DefaultTile")); //0
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Tile_Sand")); //1
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Tile_Filler")); //2
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Right_Corner")); //3
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Left_Slide")); //4
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Right_Slide")); //5
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/First_Sky_Tile")); //6
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Second_Sky_Tile")); //7
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Third_Sky_Tile")); //8
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Fourth_Sky_Tile")); //9
            _tileMap1.Add(_gameManager.GetTexture("Sprites/Tiles/Left_Corner")); //10
        }

        private void GenerateMap()
        {
            int[,] tileData = _mapData._tileMap;

            for(int x = 0; x < _mapData._mapWidth; x++)
            {
                for(int y = 0; y < _mapData._mapHeight; y++)
                {                  
                    int tileIndex = tileData[x, y];

                    Tile tile = new Tile();
                    tile.Initialise(new Vector2(x * 64, y * 64), _tileMap1[tileIndex], new Vector2(64, 64));


                    bool isGround = false;

                    if(tileIndex == 1 || tileIndex == 2 || tileIndex == 3 || tileIndex == 4 || tileIndex == 5 || tileIndex == 10)
                    {
                        isGround = true;
                    }

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
