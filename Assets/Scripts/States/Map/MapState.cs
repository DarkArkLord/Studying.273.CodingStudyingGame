using Assets.Scripts.States.Map.Components;
using Assets.Scripts.StatesMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.Map
{
    public class MapState : BaseState<MainStateCode>
    {
        #region Main info
        [SerializeField]
        private MainStateCode mapId;

        public override MainStateCode Id => mapId;

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
            yield return base.OnStateCreating();
            GlobalMapPause = false;
            _bootstrapper.OnMapInit(controller);
            _bootstrapper.SetPause(GlobalMapPause);
            //_bootstrapper.SetChildsActive(!GlobalMapPause);
        }

        public override IEnumerator OnStatePush()
        {
            GlobalMapPause = true;
            _bootstrapper.SetPause(GlobalMapPause);
            //_bootstrapper.SetChildsActive(!GlobalMapPause);

            yield return base.OnStatePush();
        }

        public override IEnumerator OnStatePop()
        {
            yield return base.OnStatePop();

            GlobalMapPause = false;
            _bootstrapper.SetPause(GlobalMapPause);
            //_bootstrapper.SetChildsActive(!GlobalMapPause);

            _bootstrapper.BattleController.ResolveBattle();
        }

        public override IEnumerator OnStateDestroy()
        {
            GlobalMapPause = true;
            _bootstrapper.SetPause(GlobalMapPause);
            //_bootstrapper.SetChildsActive(!GlobalMapPause);
            _bootstrapper.OnMapDestroy();

            yield return base.OnStateDestroy();
        }
    }
}
