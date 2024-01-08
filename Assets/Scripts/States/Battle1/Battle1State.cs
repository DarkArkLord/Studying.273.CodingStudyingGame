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
            yield return Ui.ShowPanelCorutine();

            Ui.WinButton.OnClick.AddListener(Win);
            Ui.LoseButton.OnClick.AddListener(Lose);

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
            Ui.WinButton.OnClick.RemoveAllListeners();
            Ui.LoseButton.OnClick.RemoveAllListeners();

            yield return Ui.HidePanelCorutine();
            yield return base.OnStateDestroy();
        }

        private void Win()
        {
            //
        }

        private void Lose()
        {
            //
        }
    }
}
