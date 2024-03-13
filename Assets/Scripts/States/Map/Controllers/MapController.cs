using Assets.Scripts.States.Map.Components.Generators;
using UnityEngine;

namespace Assets.Scripts.States.Map.Controllers
{
    public class MapController
    {
        public MapPathConfig MapConfig { get; private set; }

        public int Width => MapConfig.Width;
        public int Height => MapConfig.Height;

        public int StartX => MapConfig.InputPosition.x;
        public int StartY => MapConfig.InputPosition.y;

        public MapController(IMapGenerator mapGenerator, int width, int height)
        {
            MapConfig = mapGenerator.GeneratePathMap(width, height);
        }

        public MapCellContent GetMapCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return MapCellContent.None;
            }
            return MapConfig.Map[x, y];
        }

        public bool IsCanMove(int x, int y)
        {
            var cell = GetMapCell(x, y);
            return cell.IsMoveble();
        }

        public bool IsCanMove(Vector2Int to)
        {
            return IsCanMove(to.x, to.y);
        }
    }
}