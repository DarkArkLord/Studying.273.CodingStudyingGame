using Assets.Scripts.DataKeeper;
using Assets.Scripts.StatesMachine;
using UnityEngine.Events;

namespace Assets.Scripts.States.Map.Controllers
{
    public class BattleController
    {
        private PlayerMovementController player;
        private NPCMasterController npcs;
        private MainDataKeeper dataKeeper;

        public UnityEvent<MainStateCode> StartBattleEvent { get; private set; } = new UnityEvent<MainStateCode>();

        public BattleController(PlayerMovementController player, NPCMasterController npcs, MainDataKeeper dataKeeper)
        {
            this.player = player;
            this.npcs = npcs;
            this.dataKeeper = dataKeeper;
        }

        public void OnUpdate()
        {
            if (dataKeeper.Battle != null) return;

            foreach (var npc in npcs.NPCs)
            {
                if (npc.Position2D == player.Position2D && npc.IsAlive && player.IsAlive)
                {
                    dataKeeper.Battle = new BattleInfo { Enemy = npc, };
                    StartBattleEvent.Invoke(MainStateCode.Battle_NumbersOrder);
                    return;
                }
            }
        }

        public void ResolveBattle()
        {
            if (dataKeeper.Battle == null) return;

            if (dataKeeper.Battle.IsPlayerWin)
            {
                dataKeeper.Battle.Enemy.Kill();
            }
            else
            {
                player.Kill();
            }
            dataKeeper.Battle = null;
        }
    }
}
