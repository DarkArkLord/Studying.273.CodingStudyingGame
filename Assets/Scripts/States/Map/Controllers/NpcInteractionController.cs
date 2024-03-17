using Assets.Scripts.DataKeeper;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using UnityEngine.Events;

namespace Assets.Scripts.States.Map.Controllers
{
    public class NpcInteractionController
    {
        private PlayerMovementController player;

        private NpcMasterController enemies;
        private NpcMasterController friends;

        private MainDataKeeper dataKeeper;

        public UnityEvent<MainStateCode> EnemyInteractionEvent { get; private set; } = new UnityEvent<MainStateCode>();
        public UnityEvent<MainStateCode> FriendInteractionEvent { get; private set; } = new UnityEvent<MainStateCode>();

        private System.Random random;

        public NpcInteractionController(PlayerMovementController player, NpcMasterController enemies, NpcMasterController friends, MainDataKeeper dataKeeper)
        {
            this.player = player;

            this.enemies = enemies;
            this.friends = friends;

            this.dataKeeper = dataKeeper;

            this.random = RandomUtils.Random;
        }

        public void OnUpdate()
        {
            if (dataKeeper.NpcInteraction != null) return;

            foreach (var enemy in enemies.NPCs)
            {
                if (enemy.Position2D == player.Position2D && enemy.IsAlive && player.IsAlive)
                {
                    dataKeeper.NpcInteraction = new NpcInteractionInfo { Npc = enemy, NpcType = InteractedNpcType.Enemy, };
                    EnemyInteractionEvent.Invoke(MainStateCode.Battle_ExecutorNumbers);
                    return;
                }

                foreach (var friend in friends.NPCs)
                {
                    if (friend.Position2D == enemy.Position2D && friend.IsAlive && enemy.IsAlive)
                    {
                        if (random.Next() % 2 == 0)
                        {
                            friend.Kill();
                        }
                        else
                        {
                            enemy.Kill();
                        }
                    }
                }
            }

            foreach (var friend in friends.NPCs)
            {
                if (friend.Position2D == player.Position2D && friend.IsAlive && player.IsAlive)
                {
                    dataKeeper.NpcInteraction = new NpcInteractionInfo { Npc = friend, NpcType = InteractedNpcType.Friend, };
                    EnemyInteractionEvent.Invoke(MainStateCode.Battle_ExecutorNumbers);
                    return;
                }
            }
        }

        public void ResolveInteraction()
        {
            if (dataKeeper.NpcInteraction == null) return;

            if (dataKeeper.NpcInteraction.IsPlayerWin)
            {
                dataKeeper.NpcInteraction.Npc.Kill();
            }
            else
            {
                player.Kill();
            }

            dataKeeper.NpcInteraction = null;
        }
    }
}
