using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DataKeeper.QuestsSystem
{
    public class MissionConfigSO : ScriptableObject
    {
        [SerializeField]
        private QuestIdEnum id;
        public QuestIdEnum Id => id;

        [SerializeField]
        private string title;
        public string Title => title;

        [SerializeField]
        private string onStartText;
        public string OnStartText => onStartText;

        [SerializeField]
        private string onCompleteText;
        public string OnCompleteText => onCompleteText;

        [SerializeField]
        private QuestIdEnum[] questsForAvailable;
        public IReadOnlyList<QuestIdEnum> QuestsForAvailable => questsForAvailable;
    }
}
