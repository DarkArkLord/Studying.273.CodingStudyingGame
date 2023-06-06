using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    internal static class CommonUtils
    {
        public static Vector2 DirectionToVector(this MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left: return Vector2.left;
                case MoveDirection.Right: return Vector2.right;
                case MoveDirection.Up: return Vector2.up;
                case MoveDirection.Down: return Vector2.down;
            }
            return Vector2.zero;
        }

        public static Vector2Int DirectionToVectorInt(this MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left: return Vector2Int.left;
                case MoveDirection.Right: return Vector2Int.right;
                case MoveDirection.Up: return Vector2Int.up;
                case MoveDirection.Down: return Vector2Int.down;
            }
            return Vector2Int.zero;
        }

        public static Vector2Int GetRandomDirection(this System.Random random)
        {
            var value = random.Next(4);
            switch (value)
            {
                case 0: return Vector2Int.left;
                case 1: return Vector2Int.right;
                case 2: return Vector2Int.up;
                case 3: return Vector2Int.down;
            }
            return Vector2Int.zero;
        }
    }
}
