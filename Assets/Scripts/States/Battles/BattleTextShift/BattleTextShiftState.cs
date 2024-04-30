﻿using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleTextShift
{
    public class BattleTextShiftState : BaseBattleState
    {
        public override MainStateCode Id => MainStateCode.Battle_TextShift;

        protected override float AverageTime => 60f;

        [SerializeField]
        private BattleTextShiftUi _ui;
        public BattleTextShiftUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            Ui.ShowBattleUi();
            Ui.OnInit(SetAccumulateTimeFlag, Id);
            yield return Ui.ShowPanelCorutine();

            Ui.CloseButton.OnClick.AddListener(CloseBattle);

            yield return base.OnStateCreating();

            OnInit();
        }

        public override IEnumerator OnStatePush()
        {
            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();

            //

            yield return base.OnStatePush();
        }

        public override IEnumerator OnStatePop()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            Ui.ShowBattleUi();
            yield return Ui.ShowPanelCorutine();

            //

            yield return base.OnStatePop();
        }

        public override IEnumerator OnStateDestroy()
        {
            Ui.CloseButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            Ui.OnClose();
            yield return base.OnStateDestroy();
        }
    }
}