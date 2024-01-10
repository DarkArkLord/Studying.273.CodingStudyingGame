using Assets.Scripts.CommonComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.States.BattleWeightBalancer.Common
{
    public class BWB_Weight_Button : ButtonComponent
    {
        [SerializeField]
        private TMP_Text _text;
        public TMP_Text Text => _text;

        [SerializeField]
        private Image _background;
        public Image Background => _background;

        [SerializeField]
        private int _value;
        public int Value => _value;

        public bool IsSelected { get; private set; }

        public void OnInit()
        {
            _text.text = Value.ToString();
            SetButtonSelection(false);
        }

        public void SetButtonSelection(bool selection)
        {
            if (selection == IsSelected) return;

            IsSelected = selection;
            if (IsSelected)
            {
                _background.color = Color.green;
                _text.color = Color.black;
            }
            else
            {
                _background.color = Color.black;
                _text.color = Color.white;
            }
        }
    }
}
