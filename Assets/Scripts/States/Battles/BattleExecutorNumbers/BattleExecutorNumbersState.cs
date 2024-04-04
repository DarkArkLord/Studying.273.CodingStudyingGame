using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleExecutorNumbers
{
    public class BattleExecutorNumbersState : BaseBattleState
    {
        public override MainStateCode Id => MainStateCode.Battle_ExecutorNumbers;

        [SerializeField]
        private BattleExecutorNumbersUi _ui;
        public BattleExecutorNumbersUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            Ui.ShowBattleUi();
            Ui.OnInit(SetAccumulateTimeFlag);
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
            Ui.OnClose();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }
    }
}
