using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.States.Map.Components
{
    public class JumpComponent : SmoothMoveComponent
    {
        public float JumpHeight { get; set; } = 0.5f;
        public bool NeedJump { get; set; } = true;
        private float sqrt_h;

        public override void SetTarget(Vector3? moveTarget = null, Vector3? rotateTarget = null)
        {
            base.SetTarget(moveTarget, rotateTarget);
            sqrt_h = Mathf.Sqrt(JumpHeight);
        }

        private float HeightOffset => JumpHeight - (Progress * 2 - 1) * sqrt_h * ((Progress * 2 - 1) * sqrt_h);

        private Vector3 JumpOffset => NeedJump
            ? Vector3.up * HeightOffset
            : Vector3.zero;

        protected override Vector3 GetMoveStepPosition => base.GetMoveStepPosition + JumpOffset;
    }
}