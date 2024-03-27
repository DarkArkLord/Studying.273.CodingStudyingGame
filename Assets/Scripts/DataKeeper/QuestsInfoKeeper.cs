﻿using Assets.Scripts.DataKeeper.QuestsSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.DataKeeper
{
    [JsonObject]
    public class QuestsInfoKeeper
    {
        public QuestsInfoKeeper()
        {
            QuestsForStarting = new List<QuestIdEnum>();
            QuestsForCompleting = new List<QuestIdEnum>();

            QuestStates = new Dictionary<QuestIdEnum, QuestState>();
            QuestProgress = new Dictionary<QuestIdEnum, int>();

            foreach (QuestIdEnum state in Enum.GetValues(typeof(QuestIdEnum)).Cast<QuestIdEnum>())
            {
                QuestStates.Add(state, QuestState.NotAvailable);
                QuestProgress.Add(state, 0);
            }
        }

        public List<QuestIdEnum> QuestsForStarting { get; set; }
        public List<QuestIdEnum> QuestsForCompleting { get; set; }
        public Dictionary<QuestIdEnum, QuestState> QuestStates { get; set; }
        public Dictionary<QuestIdEnum, int> QuestProgress { get; set; }
    }
}
