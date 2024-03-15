using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.States.Map.Components.MapGenerators
{
    public static class Common
    {
        //public static int[,] GenerateMapObjectIndexes(MapPathConfig config, int maxPathIndex, int maxWallIndex)
        //{
        //    var random = RandomUtils.Random;

        //    int width = config.Map.GetLength(0);
        //    int height = config.Map.GetLength(1);
        //    var map = new int[width, height];

        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            if (config.Map[x, y] == MapCellContent.Path)
        //            {
        //                map[x, y] = random.Next(maxPathIndex);
        //            }
        //            else if (config.Map[x, y] == MapCellContent.Wall)
        //            {
        //                map[x, y] = random.Next(maxWallIndex);
        //            }
        //            else
        //            {
        //                map[x, y] = 0;
        //            }
        //        }
        //    }

        //    return map;
        //}
    }

    public class MapPathConfig
    {

        public int Width { get; set; }
        public int Height { get; set; }

        public MapCellContent[,] Map { get; set; }

        public Vector2Int InputPosition { get; set; }
        public Vector2Int OutputPosition { get; set; }
    }

    public enum MapCellContent
    {
        None = -1,
        Input = 0,
        Path = 1,
        Wall = 2,
        Output = 3,
    }

    public static class MapCellContentExt
    {
        public static bool IsMoveble(this MapCellContent content)
            => content == MapCellContent.Input
                || content == MapCellContent.Path
                || content == MapCellContent.Output;
    }
}
