using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.DataKeeper.QuestsSystem;
using System.Collections.Generic;

namespace Assets.Scripts.DataKeeper
{
    public interface IMainDataKeeper
    {
        public BattleResultInfo BattleResult { get; set; }

        public TextMenuDataKeeper TextMenuData { get; set; }

        public ProgressStateKeeper Progress { get; set; }

        public IReadOnlyList<MissionConfigSO> MissionConfigs { get; }
    }
}
