﻿using Assets.Scripts.CommonComponents;
using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.States.Battles.BattleWeightBalancer.Common;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
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

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            _input.text = "";

            var maxValue = InitButtons();
            var maxResultValue = GetMaxResultValue(maxValue, DifficultyLevel);

            correctResult = random.Next(1, maxResultValue);
            currentWeight = 0;
            SetCompareText();

            _checkButton.OnClick.AddListener(OnCheckButtonClick);
        }

        private int InitButtons()
        {
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

            return maxValue;
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

        private int GetMaxResultValue(int maxValue, BattleDifficultyLevel difficultyLevel)
        {
            if (difficultyLevel == BattleDifficultyLevel.Easy)
            {
                return maxValue / 4;
            }

            if (difficultyLevel == BattleDifficultyLevel.Medium)
            {
                return maxValue / 2;
            }

            return maxValue;
        }

        private void OnCheckButtonClick()
        {
            setAccumulateTimeFlag(false);

            var inputText = _input.text;
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
            foreach (var button in _weightButtons)
            {
                button.OnInit();
                button.OnClick.RemoveAllListeners();
            }
            _checkButton.OnClick.RemoveAllListeners();
        }
    }
}
