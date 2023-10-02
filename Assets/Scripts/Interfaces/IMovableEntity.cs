using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IMovableEntity : IEntityWithPosition
    {
        public void SetTarget(Vector3 target);
        public bool Move();
    }
}
