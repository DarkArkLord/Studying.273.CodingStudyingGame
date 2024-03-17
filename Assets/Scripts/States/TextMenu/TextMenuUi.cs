using Assets.Scripts.CommonComponents;
using UnityEngine;

namespace Assets.Scripts.States.TextMenu
{
    public class TextMenuUi : BaseUiModel
    {
        [SerializeField]
        private ButtonComponent _nextButton;
        public ButtonComponent NextButton => _nextButton;
    }
}
