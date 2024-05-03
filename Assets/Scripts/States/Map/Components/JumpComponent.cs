using UnityEngine;

namespace Assets.Scripts.States.Map.Components
{
    public class JumpComponent : SmoothMoveComponent
    {
        public float JumpHeight { get; set; } = 0.5f;
        public bool NeedJump { get; set; } = true;

        public override void SetTarget(Vector3? moveTarget = null, Vector3? rotateTarget = null)
        {
            base.SetTarget(moveTarget, rotateTarget);
        }

        private float HeightProgress => (2 * Progress - 1) * (2 * Progress - 1);
        private float HeightOffset => (1 - HeightProgress) * JumpHeight;

        private Vector3 JumpOffset => NeedJump
            ? Vector3.up * HeightOffset
            : Vector3.zero;

        protected override Vector3 GetMoveStepPosition => base.GetMoveStepPosition + JumpOffset;
    }
}