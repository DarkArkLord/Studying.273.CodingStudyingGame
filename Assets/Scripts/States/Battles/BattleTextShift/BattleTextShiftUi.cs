using Assets.Scripts.CommonComponents;
using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleTextShift
{
    public class BattleTextShiftUi : BaseBattleUiComponent
    {
        [SerializeField]
        private TMP_Text questText;
        [SerializeField]
        private TMP_Text offsetText;
        [SerializeField]
        private TMP_InputField input;

        [SerializeField]
        private ButtonComponent checkButton;

        private System.Random random = RandomUtils.Random;
        private IReadOnlyList<char> letters = Enumerable.Range(0, 'Я' - 'А' + 1)
            .Select(v => Convert.ToChar('А' + v)).ToArray();
        private string text;
        private int offset;

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            var textLength = GetTextLength(DifficultyLevel);
            text = GenerateText(textLength);

            do { offset = GetOffset(DifficultyLevel); }
            while (offset == 0);

            questText.text = text;
            offsetText.text = offset.ToString();
            input.text = string.Empty;

            checkButton.OnClick.AddListener(OnCheckButtonClick);
        }

        private int GetTextLength(BattleDifficultyLevel difficultyLevel)
        {
            if (difficultyLevel == BattleDifficultyLevel.Easy)
            {
                return random.Next(2, 5);
            }

            if (difficultyLevel == BattleDifficultyLevel.Medium)
            {
                return random.Next(4, 7);
            }

            if (difficultyLevel == BattleDifficultyLevel.Hard)
            {
                return random.Next(6, 9);
            }

            return 5;
        }

        private string GenerateText(int length)
        {
            return string.Join("", Enumerable.Range(0, length).Select(_ => letters[random.Next(letters.Count)]));
        }

        private int GetOffset(BattleDifficultyLevel difficultyLevel)
        {
            int borders = 0;

            if (difficultyLevel == BattleDifficultyLevel.Easy)
            {
                borders = 5;
            }
            else if (difficultyLevel == BattleDifficultyLevel.Medium)
            {
                borders = 10;
            }
            else if (difficultyLevel == BattleDifficultyLevel.Hard)
            {
                borders = 15;
            }

            return random.Next(-borders, borders + 1);
        }

        private void OnCheckButtonClick()
        {
            Root.Data.BattleResult.IsPlayerWin = false;

            if (input.text.Length != text.Length)
            {
                ShowResultUi("Неверно");
                return;
            }

            var isCorrect = CheckInputCorrect();

            Root.Data.BattleResult.IsPlayerWin = isCorrect;
            if (isCorrect)
            {
                ShowResultUi("Верно");
            }
            else
            {
                ShowResultUi("Неверно");
            }
        }

        private bool CheckInputCorrect()
        {
            var builder = new StringBuilder();
            var isCorrect = true;

            for (int i = 0; i < input.text.Length; i++)
            {

                var letterIndex = text[i] - 'А' + offset;
                while (letterIndex < 0)
                {
                    letterIndex += letters.Count;
                }
                if (letterIndex >= letters.Count)
                {
                    letterIndex %= letters.Count;
                }

                var cur = input.text[i];
                var result = letters[letterIndex];
                var isLetterCorrect = cur == result;

                var color = isLetterCorrect ? "green" : "red";
                builder.Append($"<color={color}>{cur}</color>");

                isCorrect &= isLetterCorrect;
            }

            input.text = builder.ToString();

            return isCorrect;
        }

        public override void OnClose()
        {
            base.OnClose();

            checkButton.OnClick.RemoveAllListeners();
            input.text = string.Empty;
        }
    }
}
