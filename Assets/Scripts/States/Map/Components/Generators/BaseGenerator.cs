using System;
using UnityEngine;

namespace Assets.Scripts.States.Map.Components.Generators
{
    public abstract class BaseGenerator : MonoBehaviour
    {
        protected Vector2Int GenerateInputPoint(System.Random random, int width, int height)
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

        protected Vector2Int GenerateOutputPoint(System.Random random, int width, int height)
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

        public abstract MapPathConfig GeneratePathMap(int width, int height);

        public abstract int[,] GenerateMapObjectIndexes(MapPathConfig config, int maxPathIndex, int maxWallIndex);
    }
}
