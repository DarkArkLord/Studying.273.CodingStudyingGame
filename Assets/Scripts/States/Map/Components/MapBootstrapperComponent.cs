using Assets.Scripts.CommonComponents;
using Assets.Scripts.DataKeeper.QuestsSystem;
using Assets.Scripts.States.Map.Components.MapGenerators;
using Assets.Scripts.States.Map.Controllers;
using Assets.Scripts.StatesMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.States.Map.Components
{
    public class MapBootstrapperComponent : BaseModel
    {
        [SerializeField]
        private BaseMapGenerator MapGenerator;

        [SerializeField]
        private FloorElementsKeeper floorElementsKeeper;

        [SerializeField]
        private GameObject _playerPrefab;

        [SerializeField]
        private ObjectPoolComponent _enemyNpcPool;

        [SerializeField]
        private ObjectPoolComponent _friendlyNpcPool;

        [SerializeField]
        private ObjectPoolComponent _interactiveItemsPool;

        private StatesController<MainStateCode> statesController;
        private bool IsInited = false;

        private FloorController _floorController;

        private Camera _camera;
        private GameObject _playerElement;
        private PlayerMovementController _playerMovementController;

        private NpcMasterController _enemyNpcController;
        private NpcMasterController _friendNpcController;
        private NpcMasterController _interactiveItemsController;

        private QuestController questController;

        public NpcInteractionController InteractionController { get; private set; }

        private MainStateCode CurrentState => statesController.CurrentState.Id;

        public void OnMapInit(StatesController<MainStateCode> statesController)
        {
            this.statesController = statesController;
            InitMap();

            InitPlayer();

            InitEnemies();
            InitFriends();
            InitInteractiveItems();

            InitQuests();

            InitNpcInteraction();
            IsInited = true;
        }

        private void InitMap()
        {
            var mapSize = GetMapSize();
            var mapController = new MapController(MapGenerator, mapSize, mapSize);
            _floorController = new FloorController(mapController, floorElementsKeeper);
        }

        private int GetMapSize()
        {
            if (CurrentState == MainStateCode.Map_Forest_1)
            {
                return 50;
            }
            else if (CurrentState == MainStateCode.Map_Forest_2)
            {
                return 50;
            }
            else if (CurrentState == MainStateCode.Map_Forest_3)
            {
                return 20;
            }

            return 10;
        }

        private void InitPlayer()
        {
            _camera = Camera.main;
            var movableCamera = _camera.gameObject.GetComponent<SmoothMoveComponent>();
            if (movableCamera is null)
            {
                movableCamera = _camera.gameObject.AddComponent<SmoothMoveComponent>();
            }

            _playerElement = Instantiate(_playerPrefab);
            _playerElement.transform.name = "Player";
            _playerElement.transform.parent = transform;
            var movablePlayer = _playerElement.AddComponent<JumpComponent>();

            _playerMovementController = new PlayerMovementController(movablePlayer, movableCamera, _floorController);
            _playerMovementController.Resurrect();

            _playerMovementController.StandOnInputEvent.AddListener(() =>
            {
                if (statesController.UsingStates.Count > 0)
                {
                    statesController.PopState();
                }
                else
                {
                    statesController.ClearStatesStack();
                    statesController.UseState(MainStateCode.TownMenu);
                }
            });

            _playerMovementController.StandOnOutputEvent.AddListener(() =>
            {
                var data = Root.Instance.Data;
                var quests = data.Progress.QuestsInfo;

                if (CurrentState == MainStateCode.Map_Forest_1)
                {
                    var wolfesKilled = quests.IsQuestInState(QuestIdEnum.Q3_F1_KillWolfes, QuestState.Complete);
                    var friendsHealed = quests.IsQuestInState(QuestIdEnum.Q2_F1_HealFriends, QuestState.Complete);

                    if (wolfesKilled && friendsHealed)
                    {
                        SetChildsActive(false);
                        statesController.PushState(MainStateCode.Map_Forest_2);
                    }
                    else
                    {
                        data.TextMenuData.SetText("Сперва нужно решить все дела здесь.");
                        statesController.PushState(MainStateCode.TextMenu);
                    }
                }
                else if (CurrentState == MainStateCode.Map_Forest_2)
                {
                    var wolfesKilled = quests.IsQuestInState(QuestIdEnum.Q6_F2_KillWolfes, QuestState.Complete);
                    var friendsHealed = quests.IsQuestInState(QuestIdEnum.Q5_F2_HealFriends, QuestState.Complete);
                    if (wolfesKilled && friendsHealed)
                    {
                        SetChildsActive(false);
                        statesController.PushState(MainStateCode.Map_Forest_3);
                    }
                    else
                    {
                        data.TextMenuData.SetText("Сперва нужно решить все дела здесь.");
                        statesController.PushState(MainStateCode.TextMenu);
                    }
                }
                else if (CurrentState == MainStateCode.Map_Forest_3)
                {
                    SetChildsActive(false);
                    data.TextMenuData.SetText("Дальше идти некуда, вы возвращаетесь в деревню.", MainStateCode.TownMenu);
                    statesController.ClearStatesStack();
                    statesController.UseState(MainStateCode.TextMenu);
                }
            });
        }

        private void InitEnemies()
        {
            var emeniesCount = GetEnemiesCount();
            _enemyNpcPool.Init();
            _enemyNpcController = new NpcMasterController(
                emeniesCount,
                _enemyNpcPool,
                _playerMovementController,
                _floorController.Map,
                NpcType.Enemy_Wolf,
                NpcMovementController.Create);
        }

        private int GetEnemiesCount()
        {
            var data = Root.Instance.Data;
            var quests = data.Progress.QuestsInfo;

            if (CurrentState == MainStateCode.Map_Forest_1)
            {
                var wolfesKilled = quests.IsQuestInState(QuestIdEnum.Q3_F1_KillWolfes, QuestState.Complete);
                if (wolfesKilled)
                {
                    return 5;
                }
                else
                {
                    return 10;
                }
            }
            else if (CurrentState == MainStateCode.Map_Forest_2)
            {
                var wolfesKilled = quests.IsQuestInState(QuestIdEnum.Q6_F2_KillWolfes, QuestState.Complete);
                if (wolfesKilled)
                {
                    return 5;
                }
                else
                {
                    return 10;
                }
            }
            else if (CurrentState == MainStateCode.Map_Forest_3)
            {
                var wolfesKilled = quests.IsQuestInState(QuestIdEnum.Q7_F3_KillWolfes, QuestState.Complete);
                if (wolfesKilled)
                {
                    return 0;
                }
                else
                {
                    return 5;
                }
            }

            return 1;
        }

        private void InitFriends()
        {
            var friendsCount = GetFriendsCount();
            _friendlyNpcPool.Init();
            _friendNpcController = new NpcMasterController(
                friendsCount,
                _friendlyNpcPool,
                _playerMovementController,
                _floorController.Map,
                NpcType.Friend_Elf,
                NpcMovementController.Create);
        }

        private int GetFriendsCount()
        {
            var data = Root.Instance.Data;
            var quests = data.Progress.QuestsInfo;

            if (CurrentState == MainStateCode.Map_Forest_1)
            {
                var friendsHealed = quests.IsQuestInState(QuestIdEnum.Q2_F1_HealFriends, QuestState.Complete);
                if (friendsHealed)
                {
                    return 10;
                }
                else
                {
                    return 5;
                }
            }
            else if (CurrentState == MainStateCode.Map_Forest_2)
            {
                return 5;
            }
            else if (CurrentState == MainStateCode.Map_Forest_3)
            {
                return 0;
            }

            return 1;
        }

        private void InitInteractiveItems()
        {
            var itemsCount = GetInteractiveItemsCount();
            _interactiveItemsPool.Init();
            _interactiveItemsController = new NpcMasterController(
                itemsCount,
                _interactiveItemsPool,
                _playerMovementController,
                _floorController.Map,
                NpcType.Item_Flower,
                ItemsMovementController.Create);
        }

        private int GetInteractiveItemsCount()
        {
            var data = Root.Instance.Data;
            var quests = data.Progress.QuestsInfo;

            if (CurrentState == MainStateCode.Map_Forest_1)
            {
                var flowersGathered = quests.IsQuestInState(QuestIdEnum.Q1_F1_GatherFlowers, QuestState.Complete);
                if (flowersGathered)
                {
                    return 5;
                }
                else
                {
                    return 10;
                }
            }
            else if (CurrentState == MainStateCode.Map_Forest_2)
            {
                return 5;
            }
            else if (CurrentState == MainStateCode.Map_Forest_3)
            {
                return 5;
            }

            return 1;
        }

        private void InitQuests()
        {
            questController = new QuestController(CurrentState);
            questController.Init();
        }

        private void InitNpcInteraction()
        {
            InteractionController = new NpcInteractionController(_playerMovementController, _enemyNpcController, _friendNpcController, _interactiveItemsController, questController, CurrentState);

            InteractionController.ChangeStateEvent.AddListener((stateCode) =>
            {
                statesController.PushState(stateCode);
            });
        }

        public void OnMapDestroy()
        {
            IsInited = false;

            _floorController.Clear();

            _playerMovementController.StandOnInputEvent.RemoveAllListeners();
            _playerMovementController.StandOnOutputEvent.RemoveAllListeners();

            Destroy(_playerElement);

            _enemyNpcController.OnDestroy();
            _friendNpcController.OnDestroy();
            _interactiveItemsController.OnDestroy();

            InteractionController.ChangeStateEvent.RemoveAllListeners();
        }

        public void OnUpdate()
        {
            if (!IsInited) return;

            var buttonDirection = GetButtonsDirection();

            _playerMovementController.OnUpdate(buttonDirection);

            _enemyNpcController.OnUpdate();
            _friendNpcController.OnUpdate();
            _interactiveItemsController.OnUpdate();

            InteractionController.OnUpdate();
        }

        public void SetPause(bool pause)
        {
            _playerMovementController.SetPause(pause);
            _enemyNpcController.SetPause(pause);
            _friendNpcController.SetPause(pause);
            _interactiveItemsController.SetPause(pause);
        }

        public void SetChildsActive(bool isActive)
        {
            _floorController.SetFloorActive(isActive);
            _playerMovementController.SetActive(isActive);
            _enemyNpcController.SetActive(isActive);
            _friendNpcController.SetActive(isActive);
            _interactiveItemsController.SetActive(isActive);
        }

        private MoveDirection? GetButtonsDirection()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                return MoveDirection.Left;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                return MoveDirection.Right;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                return MoveDirection.Up;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                return MoveDirection.Down;
            }
            return null;
        }
    }
}
