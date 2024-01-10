using Assets.Scripts.CommonComponents;
using UnityEngine;

namespace Assets.Scripts.States.BattleWeightBalancer
{
    public class BattleWeightBalancerUi : BaseBattleUiComponent
    {
        [SerializeField]
        private ButtonComponent _winButton;
        public ButtonComponent WinButton => _winButton;

        [SerializeField]
        private ButtonComponent _loseButton;
        public ButtonComponent LoseButton => _loseButton;
    }
}
