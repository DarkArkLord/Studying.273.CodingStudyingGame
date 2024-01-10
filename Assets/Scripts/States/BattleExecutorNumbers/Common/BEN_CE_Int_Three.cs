using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public class BEN_CE_Int_Three : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _dropdown_res;
        public TMP_Dropdown DropDownRes => _dropdown_res;

        [SerializeField]
        private TMP_Dropdown _dropdown_a;
        public TMP_Dropdown DropDownA => _dropdown_a;

        [SerializeField]
        private TMP_Text _operText;
        public TMP_Text OperText => _operText;

        [SerializeField]
        private TMP_Dropdown _dropdown_b;
        public TMP_Dropdown DropDownB => _dropdown_b;

        public void OnInit(BEN_CodeElementType type, BEN_ExecutionContext context)
        {
            _operText.text = type.GetOper();

            _dropdown_res.ClearOptions();
            context.SetMemoryDropDownOptions(_dropdown_res.options);
            _dropdown_a.ClearOptions();
            context.SetMemoryDropDownOptions(_dropdown_a.options);
            _dropdown_b.ClearOptions();
            context.SetMemoryDropDownOptions(_dropdown_b.options);

            gameObject.SetActive(true);
            SetElementsActive(true);
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
            SetElementsActive(false);
        }

        public void SetElementsActive(bool active)
        {
            _dropdown_res.interactable = active;
            _dropdown_a.interactable = active;
            _dropdown_b.interactable = active;
        }
    }
}
