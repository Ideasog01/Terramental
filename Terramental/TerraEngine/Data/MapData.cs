using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TerraEngine.Data
{
    public class MapData
    {
        public int _mapHeight;
        public int _mapWidth;

        public int[,] _tileMap;
        public int[,] _entityMap;

        public List<int> assetList = new List<int>();
        public List<Vector2> assetPositionList = new List<Vector2>();

        public string _levelName;

        public MapData(int mapHeight, int mapWidth, int[,] tileMap, int[,] entityMap, string levelName)
        {
            _mapHeight = mapHeight;
            _mapWidth = mapWidth;
            _tileMap = tileMap;
            _entityMap = entityMap;
            _levelName = levelName;
        }
    }
}
