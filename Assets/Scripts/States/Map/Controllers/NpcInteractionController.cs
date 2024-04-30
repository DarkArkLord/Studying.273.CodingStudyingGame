using Assets.Scripts.DataKeeper.QuestsSystem;
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

        private QuestController questController;

        private MainStateCode currentState;

        public UnityEvent<MainStateCode> ChangeStateEvent { get; private set; } = new UnityEvent<MainStateCode>();

        public UnityEvent ResolveInteractionEvent { get; private set; } = new UnityEvent();

        private System.Random random;

        public NpcInteractionController(PlayerMovementController player, NpcMasterController enemies, NpcMasterController friends, NpcMasterController interactiveItems, QuestController questController, MainStateCode currentState)
        {
            this.player = player;

            this.enemies = enemies;
            this.friends = friends;
            this.interactiveItems = interactiveItems;

            this.questController = questController;

            this.currentState = currentState;

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
            var data = Root.Instance.Data;
            var questsInfo = data.Progress.QuestsInfo;

            npc.SetInteractive(false);

            ResolveInteractionEvent.AddListener(KillingInteractAction(npc));
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
            var data = Root.Instance.Data;
            var questsInfo = data.Progress.QuestsInfo;

            npc.SetInteractive(false);

            if (currentState == MainStateCode.Map_Forest_1)
            {
                if (questsInfo.IsQuestInState(QuestIdEnum.Q2_F1_HealFriends, QuestState.NotAvailable))
                {
                    data.TextMenuData.SetText("Привет :з");
                    ChangeStateEvent.Invoke(MainStateCode.TextMenu);
                }
                else if (questsInfo.IsQuestInState(QuestIdEnum.Q2_F1_HealFriends, QuestState.InProgress))
                {
                    ResolveInteractionEvent.AddListener(KillingInteractAction(npc));
                    ChangeStateEvent.Invoke(MainStateCode.Battle_Test);
                }
                else if (questsInfo.IsQuestInState(QuestIdEnum.Q2_F1_HealFriends, QuestState.Complete))
                {
                    data.TextMenuData.SetText("Спасибо за помощь :з");
                    ChangeStateEvent.Invoke(MainStateCode.TextMenu);
                }
            }
            else if (currentState == MainStateCode.Map_Forest_2)
            {
                if (questsInfo.IsQuestInState(QuestIdEnum.Q5_F2_HealFriends, QuestState.NotAvailable))
                {
                    data.TextMenuData.SetText("Будь осторожен, тут опасно :з");
                    ChangeStateEvent.Invoke(MainStateCode.TextMenu);
                }
                else if (questsInfo.IsQuestInState(QuestIdEnum.Q5_F2_HealFriends, QuestState.InProgress))
                {
                    ResolveInteractionEvent.AddListener(KillingInteractAction(npc));
                    ChangeStateEvent.Invoke(MainStateCode.Battle_Test);
                }
                else if (questsInfo.IsQuestInState(QuestIdEnum.Q5_F2_HealFriends, QuestState.Complete))
                {
                    data.TextMenuData.SetText("Спасибо за помощь :з");
                    ChangeStateEvent.Invoke(MainStateCode.TextMenu);
                }
            }
        }

        private void OnItemInteraction(INpcController npc)
        {
            var data = Root.Instance.Data;
            var questsInfo = data.Progress.QuestsInfo;

            npc.SetInteractive(false);

            if (currentState == MainStateCode.Map_Forest_1)
            {
                if (questsInfo.IsQuestInState(QuestIdEnum.Q1_F1_GatherFlowers, QuestState.InProgress))
                {
                    ResolveInteractionEvent.AddListener(KillingInteractAction(npc));
                    ChangeStateEvent.Invoke(MainStateCode.Battle_Test);
                }
            }
            else if (currentState == MainStateCode.Map_Forest_2)
            {
                if (questsInfo.IsQuestInState(QuestIdEnum.Q4_F2_GatherFlowers, QuestState.InProgress))
                {
                    ResolveInteractionEvent.AddListener(KillingInteractAction(npc));
                    ChangeStateEvent.Invoke(MainStateCode.Battle_Test);
                }
            }
        }

        private UnityAction KillingInteractAction(INpcController npc)
            => () =>
            {
                var data = Root.Instance.Data;
                var result = data.BattleResult;

                if (result == null) return;

                if (result.IsPlayerWin)
                {
                    questController.RegisterWinInteraction(npc.Type);
                    npc.Kill();
                }
                else
                {
                    player.Kill();
                }

                ResolveInteractionEvent.RemoveAllListeners();

                data.BattleResult = null;
            };
    }
}
