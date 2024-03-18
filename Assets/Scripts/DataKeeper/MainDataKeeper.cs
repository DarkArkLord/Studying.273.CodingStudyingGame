namespace Assets.Scripts.DataKeeper
{
    public class MainDataKeeper
    {
        public MainDataKeeper()
        {
            NpcInteraction = null;
            Progress = new ProgressStateKeeper();
            TextMenuData = new TextMenuDataKeeper();
        }

        public NpcInteractionInfo NpcInteraction { get; set; }

        public TextMenuDataKeeper TextMenuData { get; set; }

        public ProgressStateKeeper Progress { get; set; }
    }
}
