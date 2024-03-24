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
        private NpcMasterController interactiveItems;

        private MainDataKeeper dataKeeper;

        public UnityEvent<MainStateCode> EnemyInteractionEvent { get; private set; } = new UnityEvent<MainStateCode>();
        public UnityEvent<MainStateCode> FriendInteractionEvent { get; private set; } = new UnityEvent<MainStateCode>();
        public UnityEvent<MainStateCode> ItemInteractionEvent { get; private set; } = new UnityEvent<MainStateCode>();

        public UnityEvent ResolveInteractionEvent { get; private set; } = new UnityEvent();

        private System.Random random;

        public NpcInteractionController(PlayerMovementController player, NpcMasterController enemies, NpcMasterController friends, NpcMasterController interactiveItems, MainDataKeeper dataKeeper)
        {
            this.player = player;

            this.enemies = enemies;
            this.friends = friends;
            this.interactiveItems = interactiveItems;

            this.dataKeeper = dataKeeper;

            this.random = RandomUtils.Random;
        }

        public void OnUpdate()
        {
            if (dataKeeper.NpcInteraction != null) return;

            foreach (var enemy in enemies.NPCs)
            {
                if (enemy.Position2D == player.Position2D && enemy.IsInteractive && player.IsInteractive)
                {
                    enemy.SetInteractive(false);
                    dataKeeper.NpcInteraction = new NpcInteractionInfo { Npc = enemy, NpcType = InteractedNpcType.Enemy, };
                    ResolveInteractionEvent.AddListener(Test());
                    EnemyInteractionEvent.Invoke(MainStateCode.Battle_ExecutorNumbers);
                    return;
                }

                foreach (var friend in friends.NPCs)
                {
                    if (friend.Position2D == enemy.Position2D && friend.IsInteractive && enemy.IsInteractive)
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
                if (friend.Position2D == player.Position2D && friend.IsInteractive && player.IsInteractive)
                {
                    //dataKeeper.NpcInteraction = new NpcInteractionInfo { Npc = friend, NpcType = InteractedNpcType.Friend, };
                    friend.SetInteractive(false);
                    dataKeeper.TextMenuData.SetText("Привет :з");
                    EnemyInteractionEvent.Invoke(MainStateCode.TextMenu);
                    return;
                }
            }

            foreach (var item in interactiveItems.NPCs)
            {
                if (item.Position2D == player.Position2D && item.IsInteractive && player.IsInteractive)
                {
                    dataKeeper.NpcInteraction = new NpcInteractionInfo { Npc = item, NpcType = InteractedNpcType.Object, };
                    ResolveInteractionEvent.AddListener(Test());
                    EnemyInteractionEvent.Invoke(MainStateCode.Battle_ExecutorNumbers);
                    return;
                }
            }
        }

        private UnityAction Test()
            => () =>
            {
                if (dataKeeper.NpcInteraction == null) return;

                if (dataKeeper.NpcInteraction.IsPlayerWin)
                {
                    dataKeeper.Progress.KilledEmeniesCounter++;
                    dataKeeper.NpcInteraction.Npc.Kill();
                }
                else
                {
                    player.Kill();
                }

                dataKeeper.NpcInteraction = null;
            };
    }
}
