using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{
    public class BattleController
    {
        private IBattleEntity player;
        private NPCsController npcs;
        private GlobalEventsController eventsController;

        public bool IsInBattle { get; private set; } = false;

        public BattleController(IBattleEntity player, NPCsController npcs, GlobalEventsController eventsController)
        {
            this.player = player;
            this.npcs = npcs;
            this.eventsController = eventsController;
        }

        public void OnUpdate()
        {
            if (IsInBattle)
            {
                //
            }
            else
            {
                foreach (var npc in npcs.NPCs)
                {
                    if (npc.Position2D == player.Position2D)
                    {
                        eventsController.MapPauseEvent.Invoke(true);
                        IsInBattle = true;
                        // Create battle
                        break;
                    }
                }
            }
        }
    }
}
