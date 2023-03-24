using UnityEngine;

public class JumpController : MoveController
{
    public float JumpHeight { get; set; } = 0.5f;
    public bool NeedJump { get; set; } = true;
    private float sqrt_h;

    public override void SetTarget(Vector3 target)
    {
        base.SetTarget(target);
        sqrt_h = Mathf.Sqrt(JumpHeight);
    }

    private float HeightOffset => JumpHeight - ((Progress * 2 - 1) * sqrt_h) * ((Progress * 2 - 1) * sqrt_h);

    private Vector3 JumpOffset => NeedJump
        ? Vector3.up * HeightOffset
        : Vector3.zero;

    protected override Vector3 GetStepPosition => base.GetStepPosition + JumpOffset;
}
