using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquationsWithLetters.Common
{
    public class BEWLEquationComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _equationText;
        public TMP_Text EquationText => _equationText;

        private EquationOfOperators equation;

        public void SetEquation(EquationOfOperators equation)
        {
            this.equation = equation;
            EquationText.text = equation.ToEqualString();
            EquationText.color = Color.black;
        }

        public bool CheckCorrection()
        {
            var isCorrect = equation.IsCorrect;
            EquationText.color = isCorrect ? Color.green : Color.red;
            EquationText.text = equation.ToString();
            return isCorrect;
        }
    }
}
