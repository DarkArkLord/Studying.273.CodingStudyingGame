using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.MainMenu
{
    public class MainMenuState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.MainMenu;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        [SerializeField]
        private MainMenuUi _ui;
        public MainMenuUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            yield return Ui.ShowPanelCorutine();

            Ui.StartButton.OnClick.AddListener(StartButtonClick);
            Ui.ContinueButton.OnClick.AddListener(ContinueButtonClick);
            Ui.ExitButton.OnClick.AddListener(ExitButtonClick);

            if (controller.UsingStates.Count < 1)
            {
                Ui.ContinueButton.gameObject.SetActive(false);
            }

            yield return base.OnStateCreating();
        }

        public override IEnumerator OnStatePush()
        {
            yield return Ui.HidePanelCorutine();

            //

            yield return base.OnStatePush();
        }

        public override IEnumerator OnStatePop()
        {
            yield return Ui.ShowPanelCorutine();

            //

            yield return base.OnStatePop();
        }

        public override IEnumerator OnStateDestroy()
        {
            Ui.StartButton.OnClick.RemoveAllListeners();
            Ui.ContinueButton.OnClick.RemoveAllListeners();
            Ui.ExitButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            yield return base.OnStateDestroy();
        }

        private void StartButtonClick()
        {
            controller.UseState(MainStateCode.Exit);
        }

        private void ContinueButtonClick()
        {
            controller.UseState(MainStateCode.Exit);
        }

        private void ExitButtonClick()
        {
            controller.UseState(MainStateCode.Exit);
        }
    }
}
