using Assets.Scripts.DataKeeper;
using Assets.Scripts.States.Map.Controllers.Interfaces;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using UnityEngine.Events;
using static UnityEditor.Progress;

namespace Assets.Scripts.States.Map.Controllers
{
    public class NpcInteractionController
    {
        private PlayerMovementController player;

        private NpcMasterController enemies;
        private NpcMasterController friends;
        private NpcMasterController interactiveItems;

        private MainDataKeeper dataKeeper;

        public UnityEvent<MainStateCode> ChangeStateEvent { get; private set; } = new UnityEvent<MainStateCode>();

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
                    OnEnemyInteraction(enemy);
                    return;
                }

                foreach (var friend in friends.NPCs)
                {
                    if (friend.Position2D == enemy.Position2D && friend.IsInteractive && enemy.IsInteractive)
                    {
                        OnEnemyAndFriendInteract(enemy, friend);
                    }
                }
            }

            foreach (var friend in friends.NPCs)
            {
                if (friend.Position2D == player.Position2D && friend.IsInteractive && player.IsInteractive)
                {
                    OnFriendInteraction(friend);
                    return;
                }
            }

            foreach (var item in interactiveItems.NPCs)
            {
                if (item.Position2D == player.Position2D && item.IsInteractive && player.IsInteractive)
                {
                    OnItemInteraction(item);
                    return;
                }
            }
        }

        private void OnEnemyInteraction(INpcController npc)
        {
            npc.SetInteractive(false);

            ResolveInteractionEvent.AddListener(EnemyInteractAction(npc));

            ChangeStateEvent.Invoke(MainStateCode.Battle_Test);
        }

        private void OnEnemyAndFriendInteract(INpcController enemy, INpcController friend)
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

        private void OnFriendInteraction(INpcController npc)
        {
            npc.SetInteractive(false);

            ResolveInteractionEvent.AddListener(FriendInteractAction(npc));

            dataKeeper.TextMenuData.SetText("Привет :з");
            ChangeStateEvent.Invoke(MainStateCode.TextMenu);
        }

        private void OnItemInteraction(INpcController npc)
        {
            npc.SetInteractive(false);

            ResolveInteractionEvent.AddListener(ObjectInteractAction(npc));

            ChangeStateEvent.Invoke(MainStateCode.Battle_Test);
        }

        private UnityAction EnemyInteractAction(INpcController npc)
            => () =>
            {
                if (dataKeeper.NpcInteraction == null) return;

                if (dataKeeper.NpcInteraction.IsPlayerWin)
                {
                    dataKeeper.Progress.KilledEmeniesCounter++;
                    npc.Kill();
                }
                else
                {
                    player.Kill();
                }

                ResolveInteractionEvent.RemoveAllListeners();
            };

        private UnityAction FriendInteractAction(INpcController npc)
            => () =>
            {
                if (dataKeeper.NpcInteraction == null) return;

                //if (dataKeeper.NpcInteraction.IsPlayerWin)
                //{
                //    npc.Kill();
                //}
                //else
                //{
                //    player.Kill();
                //}

                ResolveInteractionEvent.RemoveAllListeners();
            };

        private UnityAction ObjectInteractAction(INpcController npc)
            => () =>
            {
                if (dataKeeper.NpcInteraction == null) return;

                if (dataKeeper.NpcInteraction.IsPlayerWin)
                {
                    npc.Kill();
                }
                else
                {
                    player.Kill();
                }

                ResolveInteractionEvent.RemoveAllListeners();
            };
    }
}
