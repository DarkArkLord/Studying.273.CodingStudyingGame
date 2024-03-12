using Assets.Scripts.Utils;

namespace Assets.Scripts.States.Map.Components.Generators
{
    public class FieldGenerator : BaseGenerator
    {
        public override MapPathConfig GeneratePathMap(int width, int height)
        {
            var random = RandomUtils.Random;

            var map = new MapCellContent[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = MapCellContent.Path;
                }
            }

            var input = GenerateInputPoint(random, width, height);
            map[input.x, input.y] = MapCellContent.Input;

            var output = GenerateOutputPoint(random, width, height);
            map[output.x, output.y] = MapCellContent.Output;

            return new MapPathConfig()
            {
                Map = map,
                InputPosition = input,
                OutputPosition = output,
            };
        }

        public override int[,] GenerateMapObjectIndexes(MapPathConfig config, int maxPathIndex, int maxWallIndex)
        {
            var random = RandomUtils.Random;

            int width = config.Map.GetLength(0);
            int height = config.Map.GetLength(1);
            var map = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (config.Map[x, y] == MapCellContent.Path)
                    {
                        map[x, y] = random.Next(maxPathIndex);
                    }
                    else if (config.Map[x, y] == MapCellContent.Wall)
                    {
                        map[x, y] = random.Next(maxWallIndex);
                    }
                    else
                    {
                        map[x, y] = 0;
                    }
                }
            }

            return map;
        }
    }
}
