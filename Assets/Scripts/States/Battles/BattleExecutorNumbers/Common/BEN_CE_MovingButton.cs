using Assets.Scripts.CommonComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.States.Battles.BattleExecutorNumbers.Common
{
    public class BEN_CE_MovingButton : ButtonComponent
    {
        [SerializeField]
        private Image _buttomImage;
        [SerializeField]
        private Color _enableColor;
        [SerializeField]
        private Color _disableColor;

        public override void SetButtonActive(bool isActive)
        {
            base.SetButtonActive(isActive);
            _buttomImage.color = isActive ? _enableColor : _disableColor;
        }
    }
}
