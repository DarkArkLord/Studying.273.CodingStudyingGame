using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquations
{
    public class BattleEquationsState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.Battle_Equations;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        [SerializeField]
        private BattleEquationsUi _ui;
        public BattleEquationsUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            Ui.ShowBattleUi();
            Ui.OnInit();
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
            Ui.OnClose();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }

        private void CloseBattle()
        {
            controller.PopState();
        }
    }
}
