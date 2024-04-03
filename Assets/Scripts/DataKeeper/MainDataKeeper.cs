namespace Assets.Scripts.DataKeeper
{
    public class MainDataKeeper
    {
        public MainDataKeeper()
        {
            BattleResult = null;
            Progress = new ProgressStateKeeper();
            TextMenuData = new TextMenuDataKeeper();
        }

        public BattleResultInfo BattleResult { get; set; }

        public TextMenuDataKeeper TextMenuData { get; set; }

        public ProgressStateKeeper Progress { get; set; }
    }
}
