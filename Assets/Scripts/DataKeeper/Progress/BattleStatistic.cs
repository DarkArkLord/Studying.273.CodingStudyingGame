using Assets.Scripts.StatesMachine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.DataKeeper.Progress
{
    [JsonObject]
    public class BattleStatistic
    {
        [JsonProperty]
        public BattleDifficultyLevel CurrentDifficulty { get; set; } = BattleDifficultyLevel.Medium;

        [JsonProperty]
        public Dictionary<BattleDifficultyLevel, BattleLevelStatistic> LevelsStatistic { get; set; }
            = new Dictionary<BattleDifficultyLevel, BattleLevelStatistic>();
    }

    public enum BattleDifficultyLevel
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }

    [JsonObject]
    public class BattleLevelStatistic
    {
        [JsonProperty]
        public List<float> WinTimes { get; set; } = new List<float>();
        [JsonProperty]
        public List<float> LoseTimes { get; set; } = new List<float>();

        [JsonProperty]
        public float AverageTime { get; set; } = 0;

        [JsonProperty]
        public int WinCount { get; set; } = 0;
        [JsonProperty]
        public int LoseCount { get; set; } = 0;

        [JsonProperty]
        public List<float> PrevTotalGrades { get; set; } = new List<float>();
        [JsonProperty]
        public List<float> Grades { get; set; } = new List<float>();
        [JsonProperty]
        public float TotalGrade { get; set; } = 0;
    }

    public static class BattleStatisticProcessor
    {
        public static void CalculateStatistics(MainStateCode battleId, float averageTime)
        {
            var data = Root.Instance.Data;
            var result = data.BattleResult;
            var statistic = data.Progress.BattleStatistics[battleId];

            if (!statistic.LevelsStatistic.ContainsKey(statistic.CurrentDifficulty))
            {
                statistic.LevelsStatistic.Add(statistic.CurrentDifficulty, new());
            }

            var level = statistic.LevelsStatistic[statistic.CurrentDifficulty];

            AddStatistics(result, level);
            UpdateGrade(result, level, averageTime);
            UpdateDifficulty(statistic, level);
        }

        private static void AddStatistics(BattleResultInfo result, BattleLevelStatistic level)
        {
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

            level.AverageTime = (level.WinTimes.Sum() + level.LoseTimes.Sum()) / (level.WinCount + level.LoseCount);
        }

        private static void UpdateGrade(BattleResultInfo result, BattleLevelStatistic level, float averageTime)
        {
            var baseGrade = result.IsPlayerWin ? 1f : -1f;
            var timeGrade = 1 - result.BattleTime / averageTime;
            var grade = baseGrade + timeGrade;

            level.Grades.Add(grade);
            level.TotalGrade += grade;
        }

        private static void UpdateDifficulty(BattleStatistic statistic, BattleLevelStatistic level)
        {
            var gradeBorder = 10;

            if (statistic.CurrentDifficulty == BattleDifficultyLevel.Easy)
            {
                if (level.TotalGrade > gradeBorder)
                {
                    statistic.CurrentDifficulty = BattleDifficultyLevel.Medium;
                    level.PrevTotalGrades.Add(level.TotalGrade);
                    level.TotalGrade = 0;
                }
            }
            else if (statistic.CurrentDifficulty == BattleDifficultyLevel.Medium)
            {
                if (level.TotalGrade > gradeBorder)
                {
                    statistic.CurrentDifficulty = BattleDifficultyLevel.Hard;
                    level.PrevTotalGrades.Add(level.TotalGrade);
                    level.TotalGrade = 0;
                }
                else if (level.TotalGrade < -gradeBorder)
                {
                    statistic.CurrentDifficulty = BattleDifficultyLevel.Easy;
                    level.PrevTotalGrades.Add(level.TotalGrade);
                    level.TotalGrade = 0;
                }
            }
            else if (statistic.CurrentDifficulty == BattleDifficultyLevel.Hard)
            {
                if (level.TotalGrade < -gradeBorder)
                {
                    statistic.CurrentDifficulty = BattleDifficultyLevel.Medium;
                    level.PrevTotalGrades.Add(level.TotalGrade);
                    level.TotalGrade = 0;
                }
            }
        }
    }
}
