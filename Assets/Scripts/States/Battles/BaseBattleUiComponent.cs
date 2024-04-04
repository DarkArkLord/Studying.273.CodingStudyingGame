using Assets.Scripts.CommonComponents;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles
{
    public class BaseBattleUiComponent : BaseUiModel
    {
        [SerializeField]
        protected GameObject _mainBattleUi;

        public void ShowBattleUi()
        {
            _mainBattleUi.SetActive(true);
            _resultUi.SetActive(false);
        }

        [SerializeField]
        protected ButtonComponent _closeButton;
        public ButtonComponent CloseButton => _closeButton;

        [SerializeField]
        protected GameObject _resultUi;

        [SerializeField]
        protected TMP_Text _resultText;

        public void ShowResultUi(string text)
        {
            _mainBattleUi.SetActive(false);
            _resultUi.SetActive(true);
            _resultText.text = text;
        }

        protected Action<bool> setAccumulateTimeFlag;

        public virtual void OnInit(Action<bool> setAccumulateTimeFlag)
        {
            this.setAccumulateTimeFlag = setAccumulateTimeFlag;
        }
    }
}
