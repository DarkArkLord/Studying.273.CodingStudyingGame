using Assets.Scripts.DataKeeper.QuestsSystem;
using Assets.Scripts.StatesMachine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.States.Map.Controllers
{
    public class QuestController
    {
        private MainStateCode currentMap;
        private Dictionary<QuestIdEnum, MissionConfigSO> missionConfigs;

        public QuestController(MainStateCode currentMap)
        {
            this.currentMap = currentMap;
        }

        public void Init()
        {
            missionConfigs = new Dictionary<QuestIdEnum, MissionConfigSO>();

            var root = Root.Instance;
            var questsInfo = root.Data.Progress.QuestsInfo;

            foreach (var state in questsInfo.QuestStates)
            {
                if (state.Value == QuestState.InProgress)
                {
                    var config = root.MissionConfigs.FirstOrDefault(config => config.Id == state.Key) as MissionConfig_InteractNpc_SO;
                    if (config.OnMap == currentMap)
                    {
                        missionConfigs.Add(state.Key, config);
                    }
                }
            }
        }

        public void RegisterWinInteraction(NpcType npcType)
        {
            var questsInfo = Root.Instance.Data.Progress.QuestsInfo;
            foreach (var state in missionConfigs)
            {
                var config = state.Value as MissionConfig_InteractNpc_SO;
                if (config.NpcType == npcType)
                {
                    questsInfo.QuestProgress[state.Key]++;
                }
            }
        }
    }
}
