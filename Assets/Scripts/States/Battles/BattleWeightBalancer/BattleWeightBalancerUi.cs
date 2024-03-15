using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Battles.BattleWeightBalancer.Common;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleWeightBalancer
{
    public class BattleWeightBalancerUi : BaseBattleUiComponent
    {
        [SerializeField]
        private BWB_Weight_Button[] _weightButtons;
        [SerializeField]
        private TMP_Text _compareText;
        [SerializeField]
        private TMP_InputField _input;
        [SerializeField]
        private ButtonComponent _checkButton;

        private int correctResult = 0;
        private int currentWeight = 0;

        private System.Random random = RandomUtils.Random;

        public void OnInit()
        {
            _input.text = "";

            var maxValue = 0;
            foreach (var button in _weightButtons)
            {
                button.OnInit();
                maxValue += button.Value;
                button.OnClick.AddListener(() =>
                {
                    button.SetButtonSelection(!button.IsSelected);

                    if (button.IsSelected)
                    {
                        currentWeight += button.Value;
                    }
                    else
                    {
                        currentWeight -= button.Value;
                    }

                    SetCompareText();
                });
            }

            correctResult = random.Next(1, maxValue);
            currentWeight = 0;
            SetCompareText();

            _checkButton.OnClick.AddListener(() =>
            {
                var inputText = _input.text;
                if (inputText == null
                || inputText.Length < 1
                || !int.TryParse(inputText, out int inputValue)
                || inputValue != correctResult)
                {
                    Root.Data.Battle.IsPlayerWin = false;
                    ShowResultUi("Неверно");
                }
                else
                {
                    Root.Data.Battle.IsPlayerWin = true;
                    ShowResultUi("Верно");
                }
            });
        }

        private void SetCompareText()
        {
            if (correctResult < currentWeight)
            {
                _compareText.text = "<";
            }
            else if (correctResult > currentWeight)
            {
                _compareText.text = ">";
            }
            else
            {
                _compareText.text = "=";
            }
        }

        public void OnClose()
        {
            foreach (var button in _weightButtons)
            {
                button.OnInit();
                button.OnClick.RemoveAllListeners();
            }
            _checkButton.OnClick.RemoveAllListeners();
        }
    }
}
