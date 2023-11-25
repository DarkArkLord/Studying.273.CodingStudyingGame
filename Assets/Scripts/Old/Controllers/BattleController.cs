using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{
    public class BattleController
    {
        private IBattleEntity player;
        private NPCsController npcs;
        private GlobalEventsController eventsController;

        private TempBattle1Conponent tempBattle1Conponent;

        public bool IsInBattle { get; private set; } = false;
        private IBattleEntity currentEnemy;

        public BattleController(IBattleEntity player, NPCsController npcs, GlobalEventsController eventsController, TempBattle1Conponent tempBattle1Conponent)
        {
            this.player = player;
            this.npcs = npcs;
            this.eventsController = eventsController;
            this.tempBattle1Conponent = tempBattle1Conponent;

        }

        public void OnUpdate()
        {
            if (IsInBattle)
            {
                if(tempBattle1Conponent.IsWin is not null)
                {
                    bool result = (bool)tempBattle1Conponent.IsWin;
                    tempBattle1Conponent.SetNoResult();
                    tempBattle1Conponent.SetActivity(false);
                    IsInBattle = false;
                    eventsController.MapPauseEvent.Invoke(false);
                    if (result)
                    {
                        currentEnemy.Kill();
                    }
                    else
                    {
                        player.Kill();
                    }
                    currentEnemy = null;
                }
             }
            else
            {
                foreach (var npc in npcs.NPCs)
                {
                    if (npc.Position2D == player.Position2D && npc.IsAlive && player.IsAlive)
                    {
                        eventsController.MapPauseEvent.Invoke(true);
                        IsInBattle = true;
                        currentEnemy = npc;
                        // Create battle
                        tempBattle1Conponent.SetActivity(true);
                        //
                        break;
                    }
                }
            }
        }
    }
}
