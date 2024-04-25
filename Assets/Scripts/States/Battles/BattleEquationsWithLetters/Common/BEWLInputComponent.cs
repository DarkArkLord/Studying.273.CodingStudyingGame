using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquationsWithLetters.Common
{
    public class BEWLInputComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _leftSideText;
        public TMP_Text LeftSideText => _leftSideText;

        [SerializeField]
        private TMP_InputField _rightSideInputField;
        public TMP_InputField InputField => _rightSideInputField;

        private VariableOperator _operator;

        public void SetVariableOperator(VariableOperator _operator)
        {
            this._operator = _operator;
            LeftSideText.text = $"{_operator.Name} = ";
            LeftSideText.color = Color.black;
            InputField.text = string.Empty;
        }

        public void SetReadonly(bool readOnlyFlag)
        {
            InputField.readOnly = readOnlyFlag;
        }

        public bool TryReadInput()
        {
            if (InputField.text != null && InputField.text.Length >= 1
                && int.TryParse(InputField.text, out int input) && input >= 0)
            {
                _operator.Value = input;
                return true;
            }

            LeftSideText.color = Color.red;
            return false;
        }
    }
}
