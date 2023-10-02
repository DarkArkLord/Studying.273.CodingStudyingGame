using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IEntityWithPosition : IGameObject
    {
        Vector2Int Position2D { get; }
        Transform Transform { get; }
    }
}
