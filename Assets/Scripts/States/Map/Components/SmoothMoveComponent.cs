using UnityEngine;

namespace Assets.Scripts.States.Map.Components
{
    public class SmoothMoveComponent : MonoBehaviour
    {
        public float MoveTime { get; set; } = 0.25f;

        public Vector3 Target { get; protected set; }
        public float Progress { get; protected set; }
        public Vector3 MoveDirection { get; protected set; }
        public Vector3 PathPoint { get; protected set; }

        public Vector2Int Position2D => new Vector2Int((int)transform.position.x, (int)transform.position.z);
        public Transform Transform => transform;

        public GameObject GameObject => gameObject;

        public virtual void SetTarget(Vector3 target)
        {
            Target = target;
            Progress = 0;
            PathPoint = transform.position;
            MoveDirection = target - PathPoint;
        }

        protected float FramePart => Time.deltaTime / MoveTime;
        protected Vector3 StepOffset => MoveDirection * FramePart;
        protected virtual Vector3 GetStepPosition => PathPoint;

        public bool Move()
        {
            Progress += FramePart;

            if (Progress >= 1)
            {
                Progress = 1;
                transform.position = Target;
                return false;
            }

            PathPoint += StepOffset;
            transform.position = GetStepPosition;
            return true;
        }
    }
}