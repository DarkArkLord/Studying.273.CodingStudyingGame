using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IMovableEntity : IEntityWithPosition
    {
        void SetTarget(Vector3 target);
        bool Move();
    }
}
