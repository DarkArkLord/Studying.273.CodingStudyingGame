using UnityEngine;

namespace Assets.Scripts.DataKeeper.QuestsSystem
{
    public sealed class MissionConfig_InteractNpc_SO : MissionConfigSO
    {
        [SerializeField]
        private int npcType;
        public int NpcType => npcType;

        [SerializeField]
        private int interactsCount;
        public int InteractsCount => interactsCount;
    }
}
