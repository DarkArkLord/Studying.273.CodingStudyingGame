using Assets.Scripts.CommonComponents;
using UnityEngine;

namespace Assets.Scripts.States.Menu.MainMenu
{
    public class MainMenuUi : BaseUiModel
    {
        [SerializeField]
        private ButtonComponent _startButton;
        public ButtonComponent StartButton => _startButton;

        [SerializeField]
        private ButtonComponent _continueButton;
        public ButtonComponent ContinueButton => _continueButton;

        [SerializeField]
        private ButtonComponent _exitButton;
        public ButtonComponent ExitButton => _exitButton;
    }
}
