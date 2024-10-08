﻿using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.Menu.TextMenu
{
    public class TextMenuState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.TextMenu;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        [SerializeField]
        private TextMenuUi _ui;
        public TextMenuUi Ui => _ui;

        public override IEnumerator OnStateCreating()
        {
            Ui.Text.text = Root.Data.TextMenuData.Text ?? string.Empty;

            Ui.SetCanvasAlpha(0);
            Ui.EnableCanvas();

            yield return Ui.ShowPanelCorutine();

            Ui.NextButton.OnClick.AddListener(NextButtonButtonClick);

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
            Ui.NextButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            Ui.DisableCanvas();
            yield return base.OnStateDestroy();
        }

        private void NextButtonButtonClick()
        {
            var nextState = Root.Data.TextMenuData.NextState ?? MainStateCode.None;

            Root.Data.TextMenuData.Clear();

            if (nextState == MainStateCode.None)
            {
                controller.PopState();
            }
            else
            {
                controller.UseState(nextState);
            }
        }
    }
}
