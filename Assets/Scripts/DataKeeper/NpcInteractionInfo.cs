using Assets.Scripts.States.Map.Controllers.Interfaces;

namespace Assets.Scripts.DataKeeper
{
    public class NpcInteractionInfo
    {
        public INpcController Npc { get; set; }
        public InteractedNpcType NpcType { get; set; }
        public bool IsPlayerWin { get; set; }
    }

    public enum InteractedNpcType
    {
        Enemy = 0,
        Friend = 1,
        Object = 2,
    }
}
