using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float MoveTime { get; set; } = 0.25f;

    public Vector3 Target { get; private set; }
    public float Progress { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public Vector3 PathPoint { get; private set; }

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
            transform.position = Target;
            return false;
        }

        PathPoint += StepOffset;
        transform.position = GetStepPosition;
        return true;
    }
}
