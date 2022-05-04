using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using System;

namespace Terramental
{
    public class MapManager
    {
        public static List<Tile> activeTiles = new List<Tile>();
        public static List<Tile> tileList = new List<Tile>();
        public static List<bool> isWall = new List<bool>();
        public static List<Sprite> assetSpriteList = new List<Sprite>();

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
            _gameManager.IsMouseVisible = false;
            _gameManager.playerCharacter.ResetPlayer();
            SpawnManager.ResetEntities();
        }

        public void UnloadLevel()
        {
            SpawnManager.UnloadEntities();
            DestroyAssets();
        }

        public void CheckActiveTiles()
        {
            foreach(Tile tile in tileList)
            {
                if(tile.IsVisible && !activeTiles.Contains(tile))
                {
                    if(tile.IsBlocking)
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

            int tileCount = 0;

            for (int x = 0; x < _mapData._mapWidth; x++)
            {
                for(int y = 0; y < _mapData._mapHeight; y++)
                {                  
                    int tileIndex = tileData[x, y];

                    if(tileCount < tileList.Count)
                    {
                        Tile tile = tileList[tileCount];
                        tile.Initialise(new Vector2(x * 64, y * 64), _tileMap1[tileIndex], new Vector2(64, 64));
                        tile.LayerOrder = 0;

                        bool isBlocking = false;

                        if (tileIndex <= 13 || tileIndex > 20)
                        {
                            isBlocking = true;
                        }

                        tile.IsBlocking = isBlocking;
                    }
                    else
                    {
                        Tile tile = new Tile();
                        tile.Initialise(new Vector2(x * 64, y * 64), _tileMap1[tileIndex], new Vector2(64, 64));
                        tile.LayerOrder = 0;
                        bool isBlocking = false;

                        if (tileIndex <= 13 || tileIndex > 20)
                        {
                            isBlocking = true;
                        }

                        tile.IsBlocking = isBlocking;

                        tileList.Add(tile);
                    }

                    SpawnEntity(_mapData._entityMap[x, y], new Vector2(x * 64, y * 64));
                    tileCount++;
                }
            }

            foreach(Tile tile in tileList)
            {
                tile.SetNeighborTiles();
            }

            int assetCount = 0;
            int spawnedAssetCount = 0;
            
            foreach(int assetIndex in _mapData.assetList)
            {
                if(spawnedAssetCount < assetSpriteList.Count)
                {
                    Sprite assetSprite = assetSpriteList[0];
                    Texture2D assetTexture = _assetTextureList[assetIndex];
                    assetSprite.SpriteTexture = assetTexture;
                    assetSprite.Initialise(_mapData.assetPositionList[assetCount], _assetTextureList[assetIndex], new Vector2(assetTexture.Width, assetTexture.Height));
                    assetSprite.IsActive = true;
                    assetCount++;
                    spawnedAssetCount++;
                }
                else
                {
                    Texture2D assetTexture = _assetTextureList[assetIndex];
                    Sprite assetSprite = new Sprite();
                    assetSprite.Initialise(_mapData.assetPositionList[assetCount], _assetTextureList[assetIndex], new Vector2(assetTexture.Width, assetTexture.Height));
                    assetSpriteList.Add(assetSprite);
                    assetCount++;
                }
            }
        }

        private void DestroyAssets()
        {
            foreach(Sprite asset in assetSpriteList)
            {
                asset.IsActive = false;
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
                SpawnManager.SpawnCannonObstacle(position, true);
            }

            if (index == 17)
            {
                SpawnManager.SpawnCannonObstacle(position, false);
            }
        }

        public Vector2 FindValidLoaction(Vector2 originalPos, Vector2 destination, Rectangle rectangle)
        {
            Vector2 movementToTry = destination - originalPos;
            Vector2 furthestAvailableLocationSoFar = originalPos;

            int numberOfStepsToBreakMovementInto = (int)(movementToTry.Length() * 2) + 1;

            Vector2 oneStep = movementToTry / numberOfStepsToBreakMovementInto;

            for(int i = 1; i <= numberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPos + oneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, rectangle.Width, rectangle.Height);

                if(HasRoomForRectangle(newBoundary))
                {
                    furthestAvailableLocationSoFar = positionToTry;
                }
                else
                {
                    bool isDiagonalMove = movementToTry.X != 0 && movementToTry.Y != 0;

                    if(isDiagonalMove)
                    {
                        int stepsLeft = numberOfStepsToBreakMovementInto - (i - 1);

                        Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * stepsLeft;
                        Vector2 finalPositionIfMovingHorizontal = furthestAvailableLocationSoFar + remainingHorizontalMovement;

                        furthestAvailableLocationSoFar = FindValidLoaction(furthestAvailableLocationSoFar, finalPositionIfMovingHorizontal, rectangle);

                        Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * stepsLeft;
                        Vector2 finalPositionIfMovingVertical = furthestAvailableLocationSoFar + remainingVerticalMovement;
                        furthestAvailableLocationSoFar = FindValidLoaction(furthestAvailableLocationSoFar, finalPositionIfMovingVertical, rectangle);
                    }

                    break;
                }
            }

            return furthestAvailableLocationSoFar;
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (Tile tile in activeTiles)
            {
                if (tile.IsBlocking)
                {
                    if(tile.SpriteRectangle.Intersects(rectangleToCheck))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Tile FindTile(Rectangle wallRect)
        {
            foreach(Tile tile in tileList)
            {
                if(tile.SpriteRectangle.Intersects(wallRect))
                {
                    return tile;
                }
            }

            return null;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

    }
}
