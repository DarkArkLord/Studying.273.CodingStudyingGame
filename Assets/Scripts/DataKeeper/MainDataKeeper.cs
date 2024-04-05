using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.DataKeeper.QuestsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DataKeeper
{
    public class MainDataKeeper : MonoBehaviour, IMainDataKeeper
    {
        public BattleResultInfo BattleResult { get; set; } = null;
        public TextMenuDataKeeper TextMenuData { get; set; } = new TextMenuDataKeeper();
        public ProgressStateKeeper Progress { get; set; } = new ProgressStateKeeper();

        [SerializeField]
        private MissionConfigSO[] missionConfigs;
        public IReadOnlyList<MissionConfigSO> MissionConfigs => missionConfigs;
    }
}
