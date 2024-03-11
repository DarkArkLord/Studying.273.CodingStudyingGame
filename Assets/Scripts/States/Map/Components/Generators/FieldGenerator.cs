using Assets.Scripts.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.States.Map.Components.Generators
{
    public class FieldGenerator : BaseGenerator
    {
        public override MapPathConfig GenerateMap(int width, int height)
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

        private Vector2Int GenerateInputPoint(System.Random random, int width, int height)
        {
            var xPart = width / 10.0;
            var xBorderLeft = 0;
            var xBorderRight = (int)Math.Ceiling(xPart) + 1;
            var x = random.Next(xBorderLeft, xBorderRight);

            var yPart = height / 10.0;
            var yBorderTop = (int)Math.Floor(yPart * 9);
            var yBorderDown = height;
            var y = random.Next(yBorderTop, yBorderDown);

            return new Vector2Int(x, y);
        }

        private Vector2Int GenerateOutputPoint(System.Random random, int width, int height)
        {
            var xPart = width / 10.0;
            var xBorderLeft = (int)Math.Floor(xPart * 9);
            var xBorderRight = width;
            var x = random.Next(xBorderLeft, xBorderRight);

            var yPart = height / 10.0;
            var yBorderTop = 0;
            var yBorderDown = (int)Math.Ceiling(yPart) + 1;
            var y = random.Next(yBorderTop, yBorderDown);

            return new Vector2Int(x, y);
        }
    }
}
