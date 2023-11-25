using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class MapController
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int StartX { get; private set; }
        public int StartY { get; private set; }

        private int[,] map;

        public MapController(int width, int height, int startX, int startY)
        {
            Width = width;
            Height = height;
            StartX = startX;
            StartY = startY;
            map = MapGenerator.GenerateMap(Width, Height, 4, StartX, StartY);
        }

        public int GetMapCell(int x, int y)
        {
            if (x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1))
            {
                return 0;
            }
            return map[x, y];
        }

        public bool IsCanMove(int x, int y)
        {
            var cell = GetMapCell(x, y);
            return cell > 0;
        }

        public bool IsCanMove(Vector2Int to)
        {
            return IsCanMove(to.x, to.y);
        }

        public int GetRelativeMapCell(Vector2Int anchor, int x, int y)
        {
            var tx = anchor.x + x;
            var ty = anchor.y + y;
            return GetMapCell(tx, ty);
        }

        public bool IsCanMove(Vector2Int from, Vector2Int offset)
        {
            var cell = GetRelativeMapCell(from, offset.x, offset.y);
            return cell > 0;
        }
    }
}