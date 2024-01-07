using Assets.Scripts.StatesMachine;
using System.Collections;

namespace Assets.Scripts.States.Exit
{
    public class ExitState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.Exit;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        public override IEnumerator OnStateCreating()
        {
            yield return base.OnStateCreating();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
