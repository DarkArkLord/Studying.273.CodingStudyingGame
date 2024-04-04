using Assets.Scripts.StatesMachine;
using UnityEngine;

namespace Assets.Scripts.States.Battles
{
    public abstract class BaseBattleState : BaseState<MainStateCode>
    {
        protected bool accumulateTimeFlag;

        protected void OnInit()
        {
            CheckBattleResultData();

            Root.Data.BattleResult.BattleTime = 0;
            SetAccumulateTimeFlag(true);
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

        protected void SetAccumulateTimeFlag(bool flag)
        {
            accumulateTimeFlag = flag;
        }

        public override void OnUpdate()
        {
            if (accumulateTimeFlag)
            {
                Root.Data.BattleResult.BattleTime += Time.deltaTime;
            }
        }

        protected void CloseBattle()
        {
            controller.PopState();

            var result = Root.Data.BattleResult;
            var statistic = Root.Data.Progress.BattleStatistics[Id];

            if (!statistic.LevelsStatistic.ContainsKey(statistic.CurrentDifficulty))
            {
                statistic.LevelsStatistic.Add(statistic.CurrentDifficulty, new());
            }

            var level = statistic.LevelsStatistic[statistic.CurrentDifficulty];

            if (result.IsPlayerWin)
            {
                level.WinCount++;
                level.WinTimes.Add(result.BattleTime);
            }
            else
            {
                level.LoseCount++;
                level.LoseTimes.Add(result.BattleTime);
            }

            // Add grade
        }
    }
}
