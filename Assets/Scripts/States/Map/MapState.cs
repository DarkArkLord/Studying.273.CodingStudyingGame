using Assets.Scripts.States.Map.Components;
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

        private bool GlobalMapPause = false;

        public override void OnUpdate()
        {
            if (GlobalMapPause) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GlobalMapPause = true;
                controller.PushState(MainStateCode.MainMenu);
                return;
            }

            _bootstrapper.OnUpdate();
        }

        #endregion

        [SerializeField]
        private MapBootstrapperComponent _bootstrapper;
        public MapBootstrapperComponent Bootstrapper => _bootstrapper;

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
            GlobalMapPause = false;
            _bootstrapper.OnMapInit(controller);
            _bootstrapper.SetPause(GlobalMapPause);
        }

        public override IEnumerator OnStatePush()
        {
            GlobalMapPause = true;
            _bootstrapper.SetPause(GlobalMapPause);
            //yield return Ui.HidePanelCorutine();

            //

            yield return base.OnStatePush();
        }

        public override IEnumerator OnStatePop()
        {
            //yield return Ui.ShowPanelCorutine();

            //

            yield return base.OnStatePop();
            GlobalMapPause = false;
            _bootstrapper.SetPause(GlobalMapPause);
            _bootstrapper.BattleController.ResolveBattle();
        }

        public override IEnumerator OnStateDestroy()
        {
            GlobalMapPause = true;
            _bootstrapper.SetPause(GlobalMapPause);
            _bootstrapper.OnMapDestroy();
            //Ui.StartButton.OnClick.RemoveAllListeners();
            //Ui.ContinueButton.OnClick.RemoveAllListeners();
            //Ui.ExitButton.OnClick.RemoveAllListeners();

            //yield return Ui.HidePanelCorutine();
            yield return base.OnStateDestroy();
        }
    }
}
