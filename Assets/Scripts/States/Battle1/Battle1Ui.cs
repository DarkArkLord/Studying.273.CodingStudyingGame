using Assets.Scripts.CommonComponents;
using UnityEngine;

namespace Assets.Scripts.States.Battle1
{
    public class Battle1Ui : BaseUiModel
    {
        [SerializeField]
        private ButtonComponent _winButton;
        public ButtonComponent WinButton => _winButton;

        [SerializeField]
        private ButtonComponent _loseButton;
        public ButtonComponent LoseButton => _loseButton;
    }
}
