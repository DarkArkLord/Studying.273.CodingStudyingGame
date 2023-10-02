using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IEntityWithPosition : IGameObject
    {
        public Vector2Int Position2D { get; }
        public Transform Transform { get; }
    }
}
