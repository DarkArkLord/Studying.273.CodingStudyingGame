namespace Assets.Scripts.DataKeeper
{
    public class MainDataKeeper : BaseModel
    {
        public NpcInteractionInfo NpcInteraction { get; set; }

        public string TextMenuText { get; set; }

        public int KilledEmeniesCounter { get; set; }
    }
}
