using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public class BEN_CE_Int_Two : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _dropdown;
        public TMP_Dropdown DropDown => _dropdown;

        [SerializeField]
        private TMP_InputField _input;
        public TMP_InputField Input => _input;

        public void OnInit(BEN_CodeElementType type, BEN_ExecutionContext context)
        {
            _dropdown.ClearOptions();
            context.SetMemoryDropDownOptions(_dropdown.options);

            gameObject.SetActive(true);
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}
