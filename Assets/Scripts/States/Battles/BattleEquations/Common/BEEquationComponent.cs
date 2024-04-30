using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.Utils;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquations.Common
{
    public class BEEquationComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _leftSideText;
        [SerializeField]
        private TMP_InputField _rightSideInputField;
        [SerializeField]
        private TMP_Text _resultText;

        public int Result { get; private set; }
        public bool IsCorrect { get; private set; }

        private System.Random _random = RandomUtils.Random;

        private OperatorType[] _operatorsList = new OperatorType[2] { OperatorType.Plus, OperatorType.Minus };
        private OperatorType[] _operatorsListHard = new OperatorType[3] { OperatorType.Plus, OperatorType.Minus, OperatorType.Mult };

        public void GenerateTask(BattleDifficultyLevel difficultyLevel)
        {
            var tree = GenerateEquation(difficultyLevel);

            _leftSideText.text = $"{tree} = ";
            Result = tree.Result;
            _rightSideInputField.text = "";
            _rightSideInputField.interactable = true;
            _resultText.text = "";
        }

        private BaseOperator GenerateEquation(BattleDifficultyLevel difficultyLevel)
        {
            var numbersCount = GetNumbersCount(difficultyLevel);
            var operatorsList = GetOperatorsList(difficultyLevel);

            BaseOperator tree;

            do
            {
                var numbers = Enumerable.Range(0, numbersCount)
                    .Select(_ => _random.Next(2, 11)).ToArray();
                var operators = Enumerable.Range(0, numbersCount - 1)
                    .Select(_ => operatorsList[_random.Next(operatorsList.Length)]).ToArray();
                tree = OperationsParser.Parse(numbers, operators);
            } while (tree.Result < -100 || tree.Result > 100);

            return tree;
        }

        private int GetNumbersCount(BattleDifficultyLevel difficultyLevel)
        {
            if (difficultyLevel == BattleDifficultyLevel.Easy)
            {
                return 2;
            }

            return 3;
        }

        private OperatorType[] GetOperatorsList(BattleDifficultyLevel difficultyLevel)
        {
            if (difficultyLevel == BattleDifficultyLevel.Hard)
            {
                return _operatorsListHard;
            }

            return _operatorsList;
        }

        public bool CheckCorection()
        {
            _rightSideInputField.interactable = false;
            IsCorrect = _rightSideInputField.text != null && _rightSideInputField.text.Length >= 1
                && int.TryParse(_rightSideInputField.text, out int input) && input == Result;

            if (IsCorrect)
            {
                _resultText.text = $" = {Result}";
                _resultText.color = Color.green;
            }
            else
            {
                _resultText.text = $" != {Result}";
                _resultText.color = Color.red;
            }

            return IsCorrect;
        }
    }
}
