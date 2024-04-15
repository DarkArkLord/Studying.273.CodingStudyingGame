using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquationsWithLetters.Common
{
    public class BEWLEquationComponent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _equationText;
        public TMP_Text EquationText => _equationText;
    }
}
