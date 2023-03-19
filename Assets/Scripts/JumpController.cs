using UnityEngine;

public class JumpController : MonoBehaviour
{
    public float JumpHeight { get; set; } = 0.5f;
    public float JumpTime { get; set; } = 0.25f;

    private Vector3 target;
    private float progress;
    private float sqrt_h;
    private Vector3 offset;
    private Vector3 pathPoint;

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        progress = 0;
        sqrt_h = Mathf.Sqrt(JumpHeight);
        pathPoint = transform.position;
        offset = target - pathPoint;
    }

    public bool Move()
    {
        var framePath = Time.deltaTime / JumpTime;
        progress += framePath;

        if (progress >= 1)
        {
            transform.position = target;
            return false;
        }

        pathPoint += offset * framePath;
        transform.position = pathPoint + Vector3.up * HeightOffset;
        return true;
    }

    private float HeightOffset => JumpHeight - ((progress * 2 - 1) * sqrt_h) * ((progress * 2 - 1) * sqrt_h);
}
