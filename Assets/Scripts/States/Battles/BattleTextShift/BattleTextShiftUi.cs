using Assets.Scripts.CommonComponents;
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

            var textLength = 3;
            text = GenerateText(textLength);

            offset = random.Next(-5, 5);
            while (offset == 0)
            {
                offset = random.Next(-5, 5);
            }

            questText.text = text;
            offsetText.text = offset.ToString();

            checkButton.OnClick.AddListener(OnCheckButtonClick);
        }

        private string GenerateText(int length)
        {
            return string.Join("", Enumerable.Range(0, length).Select(_ => letters[random.Next(letters.Count)]));
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
