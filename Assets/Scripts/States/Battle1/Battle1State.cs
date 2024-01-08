using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.Battle1
{
    public class Battle1State : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.Battle1;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        [SerializeField]
        private Battle1Ui _ui;
        public Battle1Ui Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            yield return Ui.ShowPanelCorutine();

            Ui.WinButton.OnClick.AddListener(Win);
            Ui.LoseButton.OnClick.AddListener(Lose);

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
            yield return Ui.ShowPanelCorutine();

            //

            yield return base.OnStatePop();
        }

        public override IEnumerator OnStateDestroy()
        {
            Ui.WinButton.OnClick.RemoveAllListeners();
            Ui.LoseButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }

        private void Win()
        {
            Root.Data.Battle.IsPlayerWin = true;
            controller.PopState();
        }

        private void Lose()
        {
            Root.Data.Battle.IsPlayerWin = false;
            controller.PopState();
        }
    }
}
