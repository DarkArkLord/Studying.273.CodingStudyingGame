using Assets.Scripts.StatesMachine;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Scripts.DataKeeper.Progress
{
    public class ProgressStateKeeper
    {
        [JsonProperty]
        public QuestsInfoKeeper QuestsInfo { get; set; } = new QuestsInfoKeeper();

        [JsonProperty]
        public Dictionary<MainStateCode, BattleStatistic> BattleStatistics { get; set; } = new Dictionary<MainStateCode, BattleStatistic>();
    }
}
