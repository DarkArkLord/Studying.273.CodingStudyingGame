using Assets.Scripts.StatesMachine;
using System.Collections;

namespace Assets.Scripts.States.Init
{
    public class InitState : BaseState<MainStateCode>
    {
        #region Main info

        public override MainStateCode Id => MainStateCode.Init;

        public override void OnUpdate()
        {
            //
        }

        #endregion

        public override IEnumerator OnStateCreating()
        {
            yield return base.OnStateCreating();
            // Entry state Point
            controller.UseState(MainStateCode.MainMenu);
        }
    }
}
