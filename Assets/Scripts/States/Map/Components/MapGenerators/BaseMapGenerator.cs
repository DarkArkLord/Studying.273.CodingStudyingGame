using System;
using UnityEngine;

namespace Assets.Scripts.States.Map.Components.MapGenerators
{
    public abstract class BaseMapGenerator : MonoBehaviour, IMapGenerator
    {
        private const double PointPartsCount = 5;

        protected Vector2Int GenerateInputPoint(System.Random random, int width, int height)
        {
            var xPart = width / PointPartsCount;
            var xBorderLeft = 0;
            var xBorderRight = (int)Math.Ceiling(xPart) + 1;
            var x = random.Next(xBorderLeft, xBorderRight);

            var yPart = height / PointPartsCount;
            var yBorderTop = 0;
            var yBorderDown = (int)Math.Ceiling(yPart) + 1;
            var y = random.Next(yBorderTop, yBorderDown);

            return new Vector2Int(x, y);
        }

        protected Vector2Int GenerateOutputPoint(System.Random random, int width, int height)
        {
            var xPart = width / PointPartsCount;
            var xBorderLeft = (int)Math.Floor(xPart * (PointPartsCount - 1));
            var xBorderRight = width;
            var x = random.Next(xBorderLeft, xBorderRight);

            var yPart = height / PointPartsCount;
            var yBorderTop = (int)Math.Floor(yPart * (PointPartsCount - 1));
            var yBorderDown = height;
            var y = random.Next(yBorderTop, yBorderDown);

            return new Vector2Int(x, y);
        }

        public abstract MapPathConfig GeneratePathMap(int width, int height);
    }

    public interface IMapGenerator
    {
        public MapPathConfig GeneratePathMap(int width, int height);
    }
}
