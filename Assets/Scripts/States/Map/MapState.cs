using Assets.Scripts.StatesMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States.Map
{
    public class MapState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.Map;

        private bool skipUpdateFlag = false;

        public override void OnUpdate()
        {
            if (skipUpdateFlag) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                skipUpdateFlag = true;
                controller.PushState(MainStateCode.MainMenu);
            }
        }

        #endregion

        public override IEnumerator OnStateCreating()
        {
            //yield return Ui.ShowPanelCorutine();

            //Ui.StartButton.OnClick.AddListener(StartButtonClick);
            //Ui.ContinueButton.OnClick.AddListener(ContinueButtonClick);
            //Ui.ExitButton.OnClick.AddListener(ExitButtonClick);

            //if (controller.UsingStates.Count < 1)
            //{
            //    Ui.ContinueButton.gameObject.SetActive(false);
            //}

            yield return base.OnStateCreating();
            skipUpdateFlag = false;
        }

        public override IEnumerator OnStatePush()
        {
            skipUpdateFlag = true;
            //yield return Ui.HidePanelCorutine();

            //

            yield return base.OnStatePush();
        }

        public override IEnumerator OnStatePop()
        {
            //yield return Ui.ShowPanelCorutine();

            //

            yield return base.OnStatePop();
            skipUpdateFlag = false;
        }

        public override IEnumerator OnStateDestroy()
        {
            skipUpdateFlag = true;
            //Ui.StartButton.OnClick.RemoveAllListeners();
            //Ui.ContinueButton.OnClick.RemoveAllListeners();
            //Ui.ExitButton.OnClick.RemoveAllListeners();

            //yield return Ui.HidePanelCorutine();
            yield return base.OnStateDestroy();
        }
    }
}
