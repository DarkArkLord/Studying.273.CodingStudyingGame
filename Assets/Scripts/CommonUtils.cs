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
    }
}
