using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class MapData
    {
        public int _mapHeight;
        public int _mapWidth;

        public int[,] _tileMap;
        public int[,] _entityMap;

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
