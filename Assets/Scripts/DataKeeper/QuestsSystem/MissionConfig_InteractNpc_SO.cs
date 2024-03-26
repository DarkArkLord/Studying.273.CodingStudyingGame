using Assets.Scripts.States.Map.Controllers;
using Assets.Scripts.StatesMachine;
using UnityEngine;

namespace Assets.Scripts.DataKeeper.QuestsSystem
{
    [CreateAssetMenu(fileName = "InteractNpcMission", menuName = "Missions/New InteractNpcMission", order = 51)]
    public sealed class MissionConfig_InteractNpc_SO : MissionConfigSO
    {
        [SerializeField]
        private MainStateCode onMap;
        public MainStateCode OnMap => onMap;

        [SerializeField]
        private NpcType npcType;
        public NpcType NpcType => npcType;

        [SerializeField]
        private int interactionsCount;
        public int InteractionsCount => interactionsCount;
    }
}
