using Assets.Scripts.DataKeeper;
using Assets.Scripts.States.Map.Controllers.Interfaces;
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
            foreach (var enemy in enemies.NPCs)
            {
                if (enemy.Position2D == player.Position2D && enemy.IsInteractive && player.IsInteractive)
                {
                    enemy.SetInteractive(false);

                    ResolveInteractionEvent.AddListener(EnemyInteractAction(enemy));

                    EnemyInteractionEvent.Invoke(MainStateCode.Battle_Test);

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
                    friend.SetInteractive(false);

                    ResolveInteractionEvent.AddListener(FriendInteractAction(friend));

                    dataKeeper.TextMenuData.SetText("Привет :з");
                    EnemyInteractionEvent.Invoke(MainStateCode.TextMenu);

                    return;
                }
            }

            foreach (var item in interactiveItems.NPCs)
            {
                if (item.Position2D == player.Position2D && item.IsInteractive && player.IsInteractive)
                {
                    item.SetInteractive(false);

                    ResolveInteractionEvent.AddListener(ObjectInteractAction(item));

                    EnemyInteractionEvent.Invoke(MainStateCode.Battle_Test);

                    return;
                }
            }
        }

        private UnityAction EnemyInteractAction(INpcController npc)
            => () =>
            {
                //if (dataKeeper.NpcInteraction == null) return;

                //if (dataKeeper.NpcInteraction.IsPlayerWin)
                //{
                //    dataKeeper.Progress.KilledEmeniesCounter++;
                //    dataKeeper.NpcInteraction.Npc.Kill();
                //}
                //else
                //{
                //    player.Kill();
                //}

                //dataKeeper.NpcInteraction = null;
                ResolveInteractionEvent.RemoveAllListeners();
            };

        private UnityAction FriendInteractAction(INpcController npc)
            => () =>
            {
                //if (dataKeeper.NpcInteraction == null) return;

                //if (dataKeeper.NpcInteraction.IsPlayerWin)
                //{
                //    dataKeeper.Progress.KilledEmeniesCounter++;
                //    dataKeeper.NpcInteraction.Npc.Kill();
                //}
                //else
                //{
                //    player.Kill();
                //}

                //dataKeeper.NpcInteraction = null;
                ResolveInteractionEvent.RemoveAllListeners();
            };

        private UnityAction ObjectInteractAction(INpcController npc)
            => () =>
            {
                //if (dataKeeper.NpcInteraction == null) return;

                //if (dataKeeper.NpcInteraction.IsPlayerWin)
                //{
                //    dataKeeper.Progress.KilledEmeniesCounter++;
                //    dataKeeper.NpcInteraction.Npc.Kill();
                //}
                //else
                //{
                //    player.Kill();
                //}

                //dataKeeper.NpcInteraction = null;
                ResolveInteractionEvent.RemoveAllListeners();
            };
    }
}
