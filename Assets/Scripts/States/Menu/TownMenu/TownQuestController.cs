using Assets.Scripts.DataKeeper.QuestsSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.States.Menu.TownMenu
{
    public static class TownQuestController
    {
        public static void UpdateQuestStates()
        {
            var quests = Enum.GetValues(typeof(QuestIdEnum)).Cast<QuestIdEnum>();
            var data = Root.Instance.Data;
            var questsInfo = data.Progress.QuestsInfo;

            foreach (var quest in quests)
            {
                if (questsInfo.QuestStates[quest] == QuestState.InProgress)
                {
                    var questConfig = data.MissionConfigs.FirstOrDefault(x => x.Id == quest) as MissionConfig_InteractNpc_SO;
                    if (questConfig != null && questsInfo.QuestProgress[quest] >= questConfig.InteractionsCount)
                    {
                        questsInfo.QuestStates[quest] = QuestState.Complete;
                        questsInfo.QuestsForCompleting.Add(quest);
                    }
                }
            }

            foreach (var quest in quests)
            {
                if (questsInfo.QuestStates[quest] == QuestState.NotAvailable)
                {
                    var questConfig = data.MissionConfigs.FirstOrDefault(x => x.Id == quest);
                    if (questConfig != null)
                    {
                        var allRequiredQuestCompleted = questConfig.QuestsForAvailable
                            .All(requiredQuest => questsInfo.QuestStates[requiredQuest] == QuestState.Complete);
                        if (allRequiredQuestCompleted)
                        {
                            questsInfo.QuestStates[quest] = QuestState.InProgress;
                            questsInfo.QuestProgress[quest] = 0;
                            questsInfo.QuestsForStarting.Add(quest);
                        }
                    }
                }
            }
        }

        public static string GetQuestsText()
        {
            return "\n\n" + string.Join("\n\n", GetQuestStrings());
        }

        private static IReadOnlyList<string> GetQuestStrings()
        {
            var result = new List<string>();
            var data = Root.Instance.Data;
            var questsInfo = data.Progress.QuestsInfo;

            foreach (var quest in questsInfo.QuestsForCompleting)
            {
                var questConfig = data.MissionConfigs.FirstOrDefault(x => x.Id == quest);
                if (questConfig != null)
                {
                    result.Add(questConfig.OnCompleteText);
                }
            }

            questsInfo.QuestsForCompleting.Clear();

            foreach (var quest in questsInfo.QuestsForStarting)
            {
                var questConfig = data.MissionConfigs.FirstOrDefault(x => x.Id == quest);
                if (questConfig != null)
                {
                    result.Add(questConfig.OnStartText);
                }
            }

            questsInfo.QuestsForStarting.Clear();

            var quests = Enum.GetValues(typeof(QuestIdEnum)).Cast<QuestIdEnum>();

            foreach (var quest in quests)
            {
                if (questsInfo.QuestStates[quest] == QuestState.InProgress)
                {
                    var questConfig = data.MissionConfigs.FirstOrDefault(x => x.Id == quest) as MissionConfig_InteractNpc_SO;
                    var progress = questsInfo.QuestProgress[quest];
                    result.Add($"{questConfig.Title}\n{questConfig.InProgressText}\nВыполнено {progress} из {questConfig.InteractionsCount}");
                }
            }

            return result;
        }
    }
}
