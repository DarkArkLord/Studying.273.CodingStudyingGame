using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.TownMenu
{
    public class TownMenuState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.TownMenu;

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && controller.CurrentState.Id == Id)
            {
                controller.PushState(MainStateCode.MainMenu);
                return;
            }
        }

        #endregion

        [SerializeField]
        private TownMenuUi _ui;
        public TownMenuUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();

            yield return Ui.ShowPanelCorutine();

            Ui.GoButton.OnClick.AddListener(GoButtonClick);
            Ui.TalkButton.OnClick.AddListener(TalkButtonClick);
            Ui.SaveButton.OnClick.AddListener(SaveButtonClick);
            Ui.ExitButton.OnClick.AddListener(ExitButtonClick);

            yield return base.OnStateCreating();
        }

        public override IEnumerator OnStatePush()
        {
            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            yield return base.OnStatePush();
        }

        public override IEnumerator OnStatePop()
        {
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            yield return Ui.ShowPanelCorutine();
            yield return base.OnStatePop();
        }

        public override IEnumerator OnStateDestroy()
        {
            Ui.GoButton.OnClick.RemoveAllListeners();
            Ui.TalkButton.OnClick.RemoveAllListeners();
            Ui.SaveButton.OnClick.RemoveAllListeners();
            Ui.ExitButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }

        private void GoButtonClick()
        {
            controller.ClearStatesStack();
            controller.UseState(MainStateCode.Map_Forest_1);
        }

        private void TalkButtonClick()
        {
            controller.PushState(MainStateCode.TextMenu);
        }

        private void SaveButtonClick()
        {
            //
        }

        private void ExitButtonClick()
        {
            controller.UseState(MainStateCode.Exit);
        }
    }
}
