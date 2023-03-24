using UnityEngine;

public class JumpController : MoveController
{
    public float JumpHeight { get; set; } = 0.5f;
    private float sqrt_h;

    public new void SetTarget(Vector3 target)
    {
        base.SetTarget(target);
        sqrt_h = Mathf.Sqrt(JumpHeight);
    }

    private float HeightOffset => JumpHeight - ((Progress * 2 - 1) * sqrt_h) * ((Progress * 2 - 1) * sqrt_h);

    protected new Vector3 GetStepPosition()
    {
        return base.GetStepPosition() + Vector3.up * HeightOffset;
    }
}
