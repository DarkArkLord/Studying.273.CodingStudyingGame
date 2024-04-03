using Assets.Scripts.StatesMachine;
using UnityEngine;

namespace Assets.Scripts.States.Battles
{
    public abstract class BaseBattleState : BaseState<MainStateCode>
    {
        public override void OnUpdate()
        {
            //
        }

        protected void CheckBattleResultData()
        {
            if (Root.Data.BattleResult != null)
            {
                Debug.LogError("Battle controller started with exists battle result info");
                controller.UseState(MainStateCode.Exit);
            }
            else
            {
                Root.Data.BattleResult = new DataKeeper.BattleResultInfo();
            }
        }
    }
}
