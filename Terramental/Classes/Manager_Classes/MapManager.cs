using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;

namespace Terramental
{
    public class MapManager
    {
        public static List<Tile> activeTiles = new List<Tile>();
        public static List<Tile> tileList = new List<Tile>();
        public static List<bool> isWall = new List<bool>();

        public static float mapWidth;
        public static float mapHeight;

        private GameManager _gameManager;
        private MapData _mapData;

        private List<Texture2D> _tileMap1 = new List<Texture2D>();
        private List<Texture2D> _assetTextureList = new List<Texture2D>();

        public MapManager(GameManager gameManager)
        {
            _gameManager = gameManager;

            LoadTextures();

        }

        public void ResetLevel()
        {
            SpawnManager.ResetEntities();
            _gameManager.IsMouseVisible = true;
            _gameManager.playerCharacter.ResetPlayer();
        }

        public void CheckActiveTiles()
        {
            foreach(Tile tile in tileList)
            {
                if(tile.IsVisible && !activeTiles.Contains(tile))
                {
                    if(tile.WallTile || tile.GroundTile)
                    {
                        activeTiles.Add(tile);
                    }
                }
                
                if(activeTiles.Contains(tile) && !tile.IsVisible)
                {
                    activeTiles.Remove(tile);
                }
            }

            if(_gameManager.playerCharacter.SpritePosition.Y > (_mapData._mapHeight * 64))
            {
                _gameManager.playerCharacter.PlayerTakeDamage(3);
            }
        }

        public Tile GetTile(Vector2 position)
        {
            foreach (Tile tile in tileList)
            {
                if (tile.SpritePosition == position)
                {
                    return tile;
                }
            }
            return null;
        }

        public void LoadMapData(string filePath)
        {
            string strResultJson = File.ReadAllText(filePath);
            MapData newMapData = JsonConvert.DeserializeObject<MapData>(strResultJson);
            _mapData = newMapData;

            mapWidth = _mapData._mapWidth;
            mapHeight = _mapData._mapHeight;

            if (_mapData != null)
            {
                GenerateMap();
            }
        }

        private void LoadTextures()
        {
            _tileMap1.Add(_gameManager.GetTexture("Tiles/DefaultTile")); //0
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Corner_Tile_UpwardsLeft")); //0
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Corner_Tile_UpwardsRight")); //1
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_BottomLeft_CornerTile")); //2
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_BottomRight_CornerTile")); //3
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_Left_CornerTile")); //4
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_LeftSide_Tile")); //5
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_Right_CornerTile")); //6
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_RightSlide_Tile")); //7
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_Tile")); //8
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Left_Corner")); //9
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Left_Slide")); //10
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Right_Corner")); //11
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Right_Slide")); //12
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FifthTile")); //13
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FirstTile")); //14
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FourthTile")); //15
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_SecondTile")); //16
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_SeventhTile")); //17
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_SixthTile")); //18
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_ThirdTile")); //19
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Thin_Tile_64x32")); //20
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Tile_Filler")); //21
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Tile_Sand")); //22
            _tileMap1.Add(_gameManager.GetTexture("Tiles/WaterLevel/Tile_SandReverse")); //23

            _assetTextureList.Add(_gameManager.GetTexture("Tiles/DefaultTile")); //0
            _assetTextureList.Add(_gameManager.GetTexture("Assets/WaterLevel/Big_Palm"));
            _assetTextureList.Add(_gameManager.GetTexture("Assets/WaterLevel/Grass_1"));
            _assetTextureList.Add(_gameManager.GetTexture("Assets/WaterLevel/Grass_2"));
            _assetTextureList.Add(_gameManager.GetTexture("Assets/WaterLevel/Grass_3"));
            _assetTextureList.Add(_gameManager.GetTexture("Assets/WaterLevel/Palm_Tree"));
            _assetTextureList.Add(_gameManager.GetTexture("Assets/WaterLevel/Palm_Tree2"));
        }

        private void GenerateMap()
        {
            int[,] tileData = _mapData._tileMap;

            for (int x = 0; x < _mapData._mapWidth; x++)
            {
                for(int y = 0; y < _mapData._mapHeight; y++)
                {                  
                    int tileIndex = tileData[x, y];

                    Tile tile = new Tile();
                    tile.Initialise(new Vector2(x * 64, y * 64), _tileMap1[tileIndex], new Vector2(64, 64));
                    tile.LayerOrder = 0;
                    bool isGround = false;

                    if(tileIndex <= 13 || tileIndex > 20)
                    {
                        isGround = true;
                    }

                    tile.GroundTile = isGround;
                    tile.WallTile = isGround;

                    SpawnEntity(_mapData._entityMap[x, y], new Vector2(x * 64, y * 64));

                    tileList.Add(tile);

                }
            }

            int assetCount = 0;
            
            foreach(int assetIndex in _mapData.assetList)
            {
                Texture2D assetTexture = _assetTextureList[assetIndex];

                Sprite assetSprite = new Sprite();
                assetSprite.Initialise(_mapData.assetPositionList[assetCount], _assetTextureList[assetIndex], new Vector2(assetTexture.Width, assetTexture.Height));
                assetCount++;
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

            if(index == 7)
            {
                _gameManager.playerCharacter.TeleportPlayer(position, true);
            }

            if(index == 8)
            {
                SpawnManager.SpawnElementWall(0, position, this);
            }

            if (index == 9)
            {
                SpawnManager.SpawnElementWall(1, position, this);
            }

            if (index == 10)
            {
                SpawnManager.SpawnElementWall(2, position, this);
            }

            if(index == 12)
            {
                SpawnManager.SpawnCheckpoint(position);
            }

            if(index == 13)
            {
                SpawnManager.SpawnFragment(position);
            }

            if(index == 14)
            {
                SpawnManager.SpawnEnemy(1, position);
            }

            if(index == 15)
            {
                SpawnManager.SpawnSpikeObstacle(position);
            }

            if(index == 16)
            {
                SpawnManager.SpawnCannonObstacle(position, 1);
            }

            if (index == 17)
            {
                SpawnManager.SpawnCannonObstacle(position, -1);
            }
        }
    }
}
