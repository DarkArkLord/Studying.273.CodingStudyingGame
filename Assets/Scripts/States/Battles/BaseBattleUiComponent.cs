using Assets.Scripts.CommonComponents;
using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.StatesMachine;
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
        protected MainStateCode currentState;

        protected BattleDifficultyLevel DifficultyLevel { get; private set; }

        public virtual void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            this.setAccumulateTimeFlag = setAccumulateTimeFlag;
            this.currentState = currentState;

            if (!Root.Data.Progress.BattleStatistics.ContainsKey(currentState))
            {
                Root.Data.Progress.BattleStatistics.Add(currentState, new());
            }

            DifficultyLevel = Root.Data.Progress.BattleStatistics[currentState].CurrentDifficulty;
        }
    }
}
