using Assets.Scripts.States.Map.Controllers;

namespace Assets.Scripts.DataKeeper
{
    public class BattleInfo
    {
        public NPCMovementController Enemy { get; set; }
        public bool IsPlayerWin { get; set; }
    }
}
