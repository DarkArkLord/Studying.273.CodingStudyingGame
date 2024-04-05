using Newtonsoft.Json;
using System.Collections.Generic;

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
        public List<float> PrevGrades { get; set; } = new List<float>();
        [JsonProperty]
        public float Grade { get; set; } = 0;
    }
}
