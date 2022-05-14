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
        public static List<Tile> activeTiles = new List<Tile>(); //The visible tiles currently within the viewport bounds
        public static List<Tile> tileList = new List<Tile>(); //The tiles currently instantiated
        public static List<Sprite> assetSpriteList = new List<Sprite>(); //A list that includes all environment assets that have been instantiated

        public static float mapWidth; //The level width in tiles. Multiply by 64 (tile size) to get the map width in pixels.
        public static float mapHeight; //The level height tiles. Multiply by 64 (tile size) to get the map height in pixels.

        private GameManager _gameManager; //Reference to GameManager
        private MapData _mapData; //The map data currently being used to load the tiles, entities and environment assets

        private List<Texture2D> _waterTileMap = new List<Texture2D>(); //The tile map that includes all tiles that are used in levels set in a water region.
        private List<Texture2D> _snowTileMap = new List<Texture2D>(); //The tile map that includes all tiles that are used in levels set in a snow region
        private List<Texture2D> _fireTileMap = new List<Texture2D>(); //The tile map that includes all tiles that are used in levels set in a fire region
        private List<Texture2D> _assetWaterList = new List<Texture2D>(); //The environment assets that are used in levels set in the water region.
        private List<Texture2D> _assetSnowList = new List<Texture2D>(); //The environment assets that are used in levels set in the snow region.
        private List<Texture2D> _assetFireList = new List<Texture2D>(); //The environment assets that are used in levels set in the fire region.
        private List<Texture2D> _currentTileMap = new List<Texture2D>(); //The current tile map being used
        private List<Texture2D> _currentAssetTexture = new List<Texture2D>(); //The current list of environment assets being used

        public MapManager(GameManager gameManager) //MapManager constructed used to assign the reference to the GameManager class
        {
            _gameManager = gameManager;

            LoadTextures();

        }

        public void ResetLevel() //Resets the player and all entities currently instantiated
        {
            _gameManager.IsMouseVisible = false;
            _gameManager.playerCharacter.ResetPlayer(); //Resets the player object to it's starting state
            SpawnManager.ResetEntities(); //Resets all Entities, including reseting position, activation and properties including enemy health.
        }

        public void UnloadLevel()
        {
            SpawnManager.UnloadEntities(); //Unloads all entities by setting the IsActive and IsLoaded booleans to false
            DestroyAssets(); //Destroys all assets and sets them to inactive
        }

        public void CheckActiveTiles() //Checks the currently visible tiles within the viewport. If the tile is not active, it is removed from the activeTileList.
        {
            foreach(Tile tile in tileList)
            {
                if(tile.IsVisible && !activeTiles.Contains(tile))
                {
                    if(tile.IsBlocking)
                    {
                        activeTiles.Add(tile); //Adds the tile if it is a blocking tile and visible
                    }
                }
                
                if(activeTiles.Contains(tile) && !tile.IsVisible) //If the tile is within the activeTile list, and it is no longer visible, remove the tile from this list
                {
                    activeTiles.Remove(tile);
                }
            }

            if(_gameManager.playerCharacter.SpritePosition.Y > (_mapData._mapHeight * 64)) //If the player is outside the world boundaries on the Y axis, the player will be damaged resulting in displaying the respawn screen
            {
                _gameManager.playerCharacter.PlayerTakeDamage(3);
            }
        }

        public void LoadMapData(string filePath) //Loads the map data associtated with the filePath string argument
        {
            string strResultJson = File.ReadAllText(filePath); //Reads the data in the .json file, returning a string
            MapData newMapData = JsonConvert.DeserializeObject<MapData>(strResultJson); //Converts the .json file string to the MapData class
            _mapData = newMapData; //Sets the global MapData variable to the recently converted MapData

            mapWidth = _mapData._mapWidth; //Sets the static float mapWidth variable to the width associated with the .json data
            mapHeight = _mapData._mapHeight; //Sets the static float mapHeight variable to the width associated with the .json data

            if (_mapData != null)
            {
                GenerateMap();
            }
        }

        private void LoadTextures() //Loads and stores all textures in the appropriate list
        {
            //Water Level Tile Map
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/DefaultTile")); //0
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Corner_Tile_UpwardsLeft")); //0
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Corner_Tile_UpwardsRight")); //1
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_BottomLeft_CornerTile")); //2
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_BottomRight_CornerTile")); //3
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_Left_CornerTile")); //4
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_LeftSide_Tile")); //5
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_Right_CornerTile")); //6
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_RightSlide_Tile")); //7
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Grass_Tile")); //8
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Left_Corner")); //9
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Left_Slide")); //10
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Right_Corner")); //11
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Right_Slide")); //12
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FifthTile")); //13
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FirstTile")); //14
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FourthTile")); //15
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_SecondTile")); //16
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_SeventhTile")); //17
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_SixthTile")); //18
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_ThirdTile")); //19
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Thin_Tile_64x32")); //20
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Tile_Filler")); //21
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Tile_Sand")); //22
            _waterTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Tile_SandReverse")); //23


            //Snow Level Tile Map
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/DefaultTile")); //0
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_GroundTile")); //0
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_RightCorner")); //1
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_RighSlide")); //2
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_LeftCorner")); //3
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_LeftSlide")); //4
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_DownRightCorner")); //5
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_DownLeftCorner")); //6
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_Backwards")); //7
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Snow_Filler")); //8
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_GroundTile")); //9
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_RightCorner")); //10
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_RightSlide")); //11
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_LeftCorner")); //12
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Sky_FirstTile")); //13
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Sky_SecondTile")); //14
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Sky_ThirdTile")); //15
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FourthTile")); //16
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FifthTile")); //17
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Sky_SixthTile")); //18
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Sky_SeventhTile")); //19
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_LeftSlide")); //20
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_DownRightCorner")); //21
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_DownLeftCorner")); //22
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_Filler")); //23
            _snowTileMap.Add(_gameManager.GetTexture("Tiles/SnowLevel/Ice_Backwards")); //24

            //Fire Level Tile Map
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/DefaultTile")); //0
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_Tile")); //0
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_RightCorner")); //1
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_RightSlide")); //2
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_LeftCorner")); //3
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_LeftSlide")); //4
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_UpwardsRightCorner")); //5
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_UpwradsLeftCorner")); //6
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_UpwardsTile")); //7
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire_Filler")); //8
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_GroundTile")); //9
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_RightCorner")); //10
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_RightSlide")); //11
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_LeftCorner")); //12
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/WaterLevel/Sky_FifthTile")); //13
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/FireSky_FirstFrame")); //14
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/FireSky_SecondFrame")); //15
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/FireSky_Thirdframe")); //16
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/FireSky_FourthFrame")); //17
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/FireSky_FifthFrame")); //18
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/FireSky_SeventhFrame")); //19
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/FireSky_EightFrame")); //20
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_LeftSlide")); //21
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_DownRightCorner")); //22
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_DownLeftCorner")); //23
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_Filler")); //24
            _fireTileMap.Add(_gameManager.GetTexture("Tiles/FireLevel/Fire2_Backwards")); //25

            //Load the Water Level Environment Assets
            _assetWaterList.Add(_gameManager.GetTexture("Tiles/DefaultTile")); //0
            _assetWaterList.Add(_gameManager.GetTexture("Assets/WaterLevel/Big_Palm"));
            _assetWaterList.Add(_gameManager.GetTexture("Assets/WaterLevel/Grass_1"));
            _assetWaterList.Add(_gameManager.GetTexture("Assets/WaterLevel/Grass_2"));
            _assetWaterList.Add(_gameManager.GetTexture("Assets/WaterLevel/Grass_3"));
            _assetWaterList.Add(_gameManager.GetTexture("Assets/WaterLevel/Palm_Tree"));
            _assetWaterList.Add(_gameManager.GetTexture("Assets/WaterLevel/Palm_Tree2"));

            //Load the Fire Level Environment Assets
            _assetSnowList.Add(_gameManager.GetTexture("Tiles/DefaultTile")); //0
            _assetSnowList.Add(_gameManager.GetTexture("Assets/SnowLevel/Igloo1")); //2
            _assetSnowList.Add(_gameManager.GetTexture("Assets/SnowLevel/Snow_Pile")); //3
            _assetSnowList.Add(_gameManager.GetTexture("Assets/SnowLevel/Snow_Pile2")); //4
            _assetSnowList.Add(_gameManager.GetTexture("Assets/SnowLevel/Snow_Pine")); //5


            
        }

        private void GenerateMap()
        {
            if (GameManager.levelIndex == 1 || GameManager.levelIndex == 2 || GameManager.levelIndex == 3)
            {
                _currentTileMap = _waterTileMap;
                _currentAssetTexture = _assetWaterList;

            }
            else if (GameManager.levelIndex == 4)
            {
                _currentTileMap = _snowTileMap;
                _currentAssetTexture = _assetSnowList;
            }
            else if (GameManager.levelIndex == 5 || GameManager.levelIndex == 6)
            {
                _currentTileMap = _fireTileMap;
                _currentAssetTexture = _assetFireList;
            }


            int[,] tileData = _mapData._tileMap;

            int tileCount = 0;

            for (int x = 0; x < _mapData._mapWidth; x++)
            {
                for(int y = 0; y < _mapData._mapHeight; y++)
                {                  
                    int tileIndex = tileData[x, y];

                    if(tileCount < tileList.Count)
                    {
                        if(tileIndex < _currentTileMap.Count)
                        {
                            Tile tile = tileList[tileCount];
                            tile.Initialise(new Vector2(x * 64, y * 64), _currentTileMap[tileIndex], new Vector2(64, 64));
                            tile.LayerOrder = 0;

                            bool isBlocking = false;

                            if (tileIndex <= 13 || tileIndex > 20)
                            {
                                isBlocking = true;
                            }

                            tile.IsBlocking = isBlocking;
                        }
                    }
                    else
                    {
                        if(tileIndex < _currentTileMap.Count)
                        {
                            Tile tile = new Tile();
                            tile.Initialise(new Vector2(x * 64, y * 64), _currentTileMap[tileIndex], new Vector2(64, 64));
                            tile.LayerOrder = 0;
                            tile.IsBlocking = false;

                            bool isBlocking = false;

                            if (tileIndex <= 13 || tileIndex > 20)
                            {
                                isBlocking = true;
                            }

                            tile.IsBlocking = isBlocking;

                            tileList.Add(tile);
                        }
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
                    Texture2D assetTexture = _currentAssetTexture[assetIndex];
                    assetSprite.SpriteTexture = assetTexture;
                    assetSprite.Initialise(_mapData.assetPositionList[assetCount], _currentAssetTexture[assetIndex], new Vector2(assetTexture.Width, assetTexture.Height));
                    assetSprite.IsActive = true;
                    assetCount++;
                    spawnedAssetCount++;
                }
                else
                {
                    Texture2D assetTexture = _currentAssetTexture[assetIndex];
                    Sprite assetSprite = new Sprite();
                    assetSprite.Initialise(_mapData.assetPositionList[assetCount], _currentAssetTexture[assetIndex], new Vector2(assetTexture.Width, assetTexture.Height));
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

        private void SpawnEntity(int index, Vector2 position) //Handles creating entities in the level
        {
            if (index == 1)
            {
                //Set Player Position To Start
                SpawnManager.levelStartPosition = position; //Sets the level start position for player spawning and respawning
                _gameManager.playerCharacter.TeleportPlayer(position);
            }

            if (index == 2)
            {
                SpawnManager.SpawnLevelEnd(position);
            }

            if(index == 3)
            {
                SpawnManager.SpawnEnemy(0, position, 0);
            }

            if (index == 4)
            {
                SpawnManager.SpawnEnemy(0, position, 1);
            }

            if (index == 5)
            {
                SpawnManager.SpawnEnemy(0, position, 2);
            }

            if (index == 6)
            {
                SpawnManager.SpawnEnemy(1, position, 0);
            }

            if (index == 7)
            {
                SpawnManager.SpawnEnemy(1, position, 1);
            }

            if (index == 8)
            {
                SpawnManager.SpawnEnemy(1, position, 2);
            }

            if (index == 9)
            {
                SpawnManager.SpawnHealthPickup(position);
            }

            if (index == 10)
            {
                SpawnManager.SpawnScorePickup(position);
            }

            if(index == 11)
            {
                SpawnManager.SpawnElementPickup(0, position);
            }

            if(index == 12)
            {
                SpawnManager.SpawnElementPickup(1, position);
            }

            if(index == 13)
            {
                SpawnManager.SpawnElementPickup(2, position);
            }

            if(index == 14)
            {
                SpawnManager.SpawnElementWall(0, position, this);
            }

            if(index == 15)
            {
                SpawnManager.SpawnElementWall(1, position, this);
            }

            if(index == 16)
            {
                SpawnManager.SpawnElementWall(2, position, this);
            }

            if(index == 17)
            {
                SpawnManager.SpawnSpikeObstacle(position);
            }

            if(index == 18)
            {
                SpawnManager.SpawnCannonObstacle(position, true);
            }

            if(index == 19)
            {
                SpawnManager.SpawnCannonObstacle(position, false);
            }

            if(index == 20)
            {
                SpawnManager.SpawnMovingPlatform(position, this);
            }

        }

        public Vector2 FindValidLoaction(Vector2 originalPos, Vector2 destination, Rectangle rectangle) //Finds if the space between the original position and destination is valid, otherwise, returns the furthest available location
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

        public bool HasRoomForRectangle(Rectangle rectangleToCheck) //Checks if there is room for the given rectangle within the generated level tile map
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

        public bool HasRoomForRectangleMP(Rectangle rectangleToCheck) //Checks if there is room for the given rectangle with reference to the generated moving platforms
        {
            foreach (MovingPlatform movingPlatform in SpawnManager.movingPlatformList)
            {
                if (movingPlatform.SpriteRectangle.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }

            return true;
        }

        public Tile FindTile(Rectangle wallRect) //Finds the tile associated with the position of the rectangle
        {
            foreach(Tile tile in tileList)
            {
                if(tile.SpriteRectangle.Contains(wallRect))
                {
                    return tile;
                }
            }

            return null;
        }

        public Tile FindNearestTile(Rectangle rectangle) //Finds the nearest tile based on distance
        {
            foreach (Tile tile in tileList)
            {
                float distance = MathF.Sqrt(MathF.Pow(rectangle.X - tile.SpritePosition.X, 2) + MathF.Pow(rectangle.Y - tile.SpritePosition.Y, 2));

                if(distance <= 64)
                {
                    return tile;
                }
            }

            return null;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height) //Creates a rectangle based on the given values
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }
    }
}
