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
    }
}
