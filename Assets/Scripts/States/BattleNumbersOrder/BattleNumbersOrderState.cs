using Assets.Scripts.States.BattleTest;
using Assets.Scripts.StatesMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States.BattleNumbersOrder
{
    public class BattleNumbersOrderState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.Battle_NumbersOrder;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        [SerializeField]
        private BattleNumbersOrderUi _ui;
        public BattleNumbersOrderUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            //Ui.ShowBattleUi();
            Ui.ShowResultUi("123");
            yield return Ui.ShowPanelCorutine();

            //Ui.WinButton.OnClick.AddListener(Win);
            //Ui.LoseButton.OnClick.AddListener(Lose);
            Ui.CloseButton.OnClick.AddListener(CloseBattle);

            yield return base.OnStateCreating();

            if (Root.Data.Battle == null)
            {
                Debug.LogError("Start battle container without battle info");
                controller.UseState(MainStateCode.Exit);
            }
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
            //Ui.WinButton.OnClick.RemoveAllListeners();
            //Ui.LoseButton.OnClick.RemoveAllListeners();
            Ui.CloseButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }

        private void CloseBattle()
        {
            controller.PopState();
        }
    }
}
