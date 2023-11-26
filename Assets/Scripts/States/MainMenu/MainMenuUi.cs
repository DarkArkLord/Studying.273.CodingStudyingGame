using Assets.Scripts.CommonComponents;
using UnityEngine;

namespace Assets.Scripts.States.MainMenu
{
    public class MainMenuUi : BaseUiModel
    {
        [SerializeField]
        private CanvasRenderer _backgroundPanel;
        public CanvasRenderer BackgroundPanel => _backgroundPanel;

        [SerializeField]
        private ButtonComponent _exitButton;
        public ButtonComponent ExitButton => _exitButton;
    }
}
