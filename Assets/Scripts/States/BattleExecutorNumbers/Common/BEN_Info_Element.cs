using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public class BEN_Info_Element : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;
        public TMP_Text Text => _text;
        private Color defaultColor;

        public void OnInit()
        {
            defaultColor = _text.color;
        }

        public void OnClose()
        {
            _text.color = defaultColor;
        }
    }
}
