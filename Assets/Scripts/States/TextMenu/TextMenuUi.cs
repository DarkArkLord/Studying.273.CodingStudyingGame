using Assets.Scripts.CommonComponents;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.TextMenu
{
    public class TextMenuUi : BaseUiModel
    {
        [SerializeField]
        private ButtonComponent _nextButton;
        public ButtonComponent NextButton => _nextButton;

        [SerializeField]
        private TMP_Text _text;
        public TMP_Text Text => _text;
    }
}
