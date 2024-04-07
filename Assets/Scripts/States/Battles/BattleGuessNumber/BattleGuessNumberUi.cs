using Assets.Scripts.CommonComponents;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleGuessNumber
{
    public class BattleGuessNumberUi : BaseBattleUiComponent
    {
        [SerializeField]
        private TMP_Text compareText;
        [SerializeField]
        private TMP_InputField input;

        [SerializeField]
        private ButtonComponent checkButton;
        [SerializeField]
        private ButtonComponent okButton;

        private int correctResult = 0;

        private System.Random random = RandomUtils.Random;

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            correctResult = random.Next(1, 101);
            compareText.text = "?";
            input.text = string.Empty;

            checkButton.OnClick.AddListener(OnCheckButtonClick);
            okButton.OnClick.AddListener(OnOkButtonClick);
        }

        private void OnCheckButtonClick()
        {
            var inputText = input.text;

            if (inputText != null
                && inputText.Length > 0
                && int.TryParse(inputText, out int inputValue))
            {
                if (inputValue < correctResult)
                {
                    compareText.text = "<";
                }
                else if (inputValue > correctResult)
                {
                    compareText.text = ">";
                }
                else
                {
                    compareText.text = "=";
                }
            }
            else
            {
                compareText.text = "?";
            }
        }

        private void OnOkButtonClick()
        {
            setAccumulateTimeFlag(false);

            var inputText = input.text;
            var isIncorrect = inputText == null
                || inputText.Length < 1
                || !int.TryParse(inputText, out int inputValue)
                || inputValue != correctResult;

            Root.Data.BattleResult.IsPlayerWin = !isIncorrect;
            if (isIncorrect)
            {
                ShowResultUi("Неверно");
            }
            else
            {
                ShowResultUi("Верно");
            }
        }

        public override void OnClose()
        {
            base.OnClose();

            checkButton.OnClick.RemoveAllListeners();
            okButton.OnClick.RemoveAllListeners();
        }
    }
}
