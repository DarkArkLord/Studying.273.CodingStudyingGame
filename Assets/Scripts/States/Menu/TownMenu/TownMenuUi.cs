using Assets.Scripts.CommonComponents;
using UnityEngine;

namespace Assets.Scripts.States.Menu.TownMenu
{
    public class TownMenuUi : BaseUiModel
    {
        [SerializeField]
        private ButtonComponent _goButton;
        public ButtonComponent GoButton => _goButton;

        [SerializeField]
        private ButtonComponent _talkButton;
        public ButtonComponent TalkButton => _talkButton;

        [SerializeField]
        private ButtonComponent _saveButton;
        public ButtonComponent SaveButton => _saveButton;

        [SerializeField]
        private ButtonComponent _exitButton;
        public ButtonComponent ExitButton => _exitButton;

    }
}
