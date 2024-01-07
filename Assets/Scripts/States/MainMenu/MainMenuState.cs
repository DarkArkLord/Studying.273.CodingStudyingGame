﻿using Assets.Scripts.StatesMachine;
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
            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();
            yield return Ui.ShowPanelCorutine();

            Ui.StartButton.OnClick.AddListener(StartButtonClick);
            if (controller.UsingStates.Count < 1)
            {
                Ui.ContinueButton.gameObject.SetActive(false);
            }
            else
            {
                Ui.ContinueButton.gameObject.SetActive(true);
                Ui.ContinueButton.OnClick.AddListener(ContinueButtonClick);
            }
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
            Ui.StartButton.OnClick.RemoveAllListeners();
            Ui.ContinueButton.OnClick.RemoveAllListeners();
            Ui.ExitButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }

        private void StartButtonClick()
        {
            controller.UsingStates.Clear();
            controller.UseState(MainStateCode.Map);
        }

        private void ContinueButtonClick()
        {
            controller.PopState();
        }

        private void ExitButtonClick()
        {
            controller.UseState(MainStateCode.Exit);
        }
    }
}