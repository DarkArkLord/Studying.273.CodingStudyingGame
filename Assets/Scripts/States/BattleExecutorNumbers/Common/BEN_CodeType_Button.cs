using Assets.Scripts.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public class BEN_CodeType_Button : ButtonComponent
    {
        [SerializeField]
        private TMP_Text buttonText;
        [SerializeField]
        private BEN_CodeElementType elementType;
        public BEN_CodeElementType ElementType => elementType;

        public void OnInit(Action<BEN_CodeType_Button> selector)
        {
            buttonText.text = elementType.GetName();
            OnClick.AddListener(() =>
            {
                selector.Invoke(this);
            });
        }

        public void OnClose()
        {
            OnClick.RemoveAllListeners();
        }
    }
}
