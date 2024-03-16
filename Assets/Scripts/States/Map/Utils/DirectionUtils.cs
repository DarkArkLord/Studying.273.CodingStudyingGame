using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.States.Map.Utils
{
    public static class DirectionUtils
    {
        public static Vector2 DirectionToVector2(this MoveDirection direction)
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

        public static Vector2Int DirectionToVector2Int(this MoveDirection? direction)
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

        public static Vector2Int DirectionToVector2Int(this MoveDirection direction)
            => ((MoveDirection?)direction).DirectionToVector2Int();

        public static MoveDirection GetRandomDirection(this System.Random random)
        {
            var value = random.Next(4);
            switch (value)
            {
                case 0: return MoveDirection.Left;
                case 1: return MoveDirection.Right;
                case 2: return MoveDirection.Up;
                case 3: return MoveDirection.Down;
            }
            return MoveDirection.None;
        }
    }
}
