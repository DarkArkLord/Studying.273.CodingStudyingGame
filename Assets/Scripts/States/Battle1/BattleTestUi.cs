using Assets.Scripts.CommonComponents;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battle1
{
    public class BattleTestUi : BaseUiModel
    {
        [SerializeField]
        private ButtonComponent _winButton;
        public ButtonComponent WinButton => _winButton;

        [SerializeField]
        private ButtonComponent _loseButton;
        public ButtonComponent LoseButton => _loseButton;

        [SerializeField]
        private GameObject _mainBattleUi;
        //public GameObject BattleUi => _mainBattleUi;

        [SerializeField]
        private ButtonComponent _closeButton;
        public ButtonComponent CloseButton => _closeButton;

        [SerializeField]
        private GameObject _resultUi;
        //public GameObject ResultUi => _resultUi;

        [SerializeField]
        private TMP_Text _resultText;

        public void ShowBattleUi()
        {
            _mainBattleUi.SetActive(true);
            _resultUi.SetActive(false);
        }

        public void ShowResultUi(string text)
        {
            _mainBattleUi.SetActive(false);
            _resultUi.SetActive(true);
            _resultText.text = text;
        }
    }
}
