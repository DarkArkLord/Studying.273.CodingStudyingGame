using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IMovableEntity : IEntityWithPosition, ITransform, IGameObject
    {
        void SetTarget(Vector3 target);
        bool Move();
    }
}
