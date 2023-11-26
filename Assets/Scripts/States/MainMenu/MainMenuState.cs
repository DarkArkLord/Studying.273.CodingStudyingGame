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
            yield return StartCoroutine(Ui.ShowPanelCorutine());
            Ui.ExitButton.OnClick.AddListener(ExitButtonClick);
            yield return base.OnStateCreating();
        }

        public override IEnumerator OnStatePush()
        {
            yield return StartCoroutine(Ui.HidePanelCorutine());
            yield return base.OnStatePush();
        }

        public override IEnumerator OnStatePop()
        {
            yield return StartCoroutine(Ui.ShowPanelCorutine());
            yield return base.OnStatePop();
        }

        public override IEnumerator OnStateDestroy()
        {
            Ui.ExitButton.OnClick.RemoveAllListeners();
            yield return StartCoroutine(Ui.HidePanelCorutine());
            yield return base.OnStateDestroy();
        }

        private void ExitButtonClick()
        {
            Debug.Log(123);
        }
    }
}
