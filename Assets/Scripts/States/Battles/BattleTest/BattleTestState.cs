using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleTest
{
    public class BattleTestState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.Battle_Test;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        [SerializeField]
        private BattleTestUi _ui;
        public BattleTestUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            Ui.ShowBattleUi();
            yield return Ui.ShowPanelCorutine();

            Ui.WinButton.OnClick.AddListener(Win);
            Ui.LoseButton.OnClick.AddListener(Lose);
            Ui.CloseButton.OnClick.AddListener(CloseBattle);

            yield return base.OnStateCreating();
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
            Ui.WinButton.OnClick.RemoveAllListeners();
            Ui.LoseButton.OnClick.RemoveAllListeners();
            Ui.CloseButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }

        private void Win()
        {
            Root.Data.NpcInteraction.IsPlayerWin = true;
            Ui.ShowResultUi("Победа");
        }

        private void Lose()
        {
            Root.Data.NpcInteraction.IsPlayerWin = false;
            Ui.ShowResultUi("Поражение");
        }

        private void CloseBattle()
        {
            controller.PopState();
        }
    }
}
