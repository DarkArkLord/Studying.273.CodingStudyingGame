using UnityEngine;

namespace Assets.Scripts.States.Map.Components
{
    public class SmoothMoveComponent : MonoBehaviour
    {
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public Vector2Int Position2D => new Vector2Int((int)transform.position.x, (int)transform.position.z);

        public float MoveTime { get; set; } = 0.25f;

        public float Progress { get; protected set; }

        public Vector3? MoveTarget { get; protected set; }
        public bool HasMoveTarget => MoveTarget != null;
        public Vector3 MovePathPoint { get; protected set; }
        public Vector3 MoveDirection { get; protected set; }

        public Vector3? RotateTarget { get; protected set; }
        public bool HasRotateTarget => RotateTarget != null;
        public Vector3 RotatePathPoint { get; protected set; }
        public Vector3 RotateDirection { get; protected set; }

        public virtual void SetTarget(Vector3? moveTarget = null, Vector3? rotateTarget = null)
        {
            Progress = 0;

            MoveTarget = moveTarget;
            if (HasMoveTarget)
            {
                MovePathPoint = transform.position;
                MoveDirection = (Vector3)moveTarget - MovePathPoint;
            }

            RotateTarget = rotateTarget;
            if (HasRotateTarget)
            {
                RotatePathPoint = transform.forward;
                RotateDirection = (Vector3)rotateTarget - RotatePathPoint;
            }
        }

        protected float FramePart => Time.deltaTime / MoveTime;

        protected Vector3 MoveStepOffset => MoveDirection * FramePart;
        protected virtual Vector3 GetMoveStepPosition => MovePathPoint;

        protected Vector3 RotateStepOffset => RotateDirection * FramePart;
        protected Vector3 GetRotateStepPosition => RotatePathPoint;

        public bool Move()
        {
            Progress += FramePart;

            if (Progress >= 1)
            {
                Progress = 1;
                if (HasMoveTarget)
                {
                    transform.position = (Vector3)MoveTarget;
                }
                return false;
            }

            if (HasMoveTarget)
            {
                MovePathPoint += MoveStepOffset;
                transform.position = GetMoveStepPosition;
            }

            if (HasRotateTarget)
            {
                RotatePathPoint += RotateStepOffset;
                transform.forward = GetRotateStepPosition;
            }

            return true;
        }
    }
}