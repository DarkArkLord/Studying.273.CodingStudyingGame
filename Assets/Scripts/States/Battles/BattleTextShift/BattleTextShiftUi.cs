using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Battles.BattleEquationsWithLetters.Common;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleTextShift
{
    public class BattleTextShiftUi : BaseBattleUiComponent
    {
        [SerializeField]
        private TMP_Text questText;
        [SerializeField]
        private TMP_Text offsetText;
        [SerializeField]
        private TMP_InputField input;

        [SerializeField]
        private ButtonComponent checkButton;

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            //
        }

        public override void OnClose()
        {
            base.OnClose();

            //
        }
    }
}
