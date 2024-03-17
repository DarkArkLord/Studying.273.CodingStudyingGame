﻿using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Map.Components.MapGenerators;
using Assets.Scripts.States.Map.Controllers;
using Assets.Scripts.StatesMachine;
using Unity.VisualScripting;
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

        private StatesController<MainStateCode> statesController;
        private bool IsInited = false;

        private FloorController _floorController;

        private Camera _camera;
        private GameObject _playerElement;
        private PlayerMovementController _playerMovementController;

        private NpcMasterController _enemyNpcController;
        private NpcMasterController _friendNpcController;

        public NpcInteractionController InteractionController { get; private set; }

        public void OnMapInit(StatesController<MainStateCode> statesController)
        {
            this.statesController = statesController;
            InitMap();
            InitPlayer();
            InitEnemies();
            InitFriends();
            InitNpcInteraction();
            IsInited = true;
        }

        private void InitMap()
        {
            var mapSize = 50;
            var mapController = new MapController(MapGenerator, mapSize, mapSize);
            _floorController = new FloorController(mapController, floorElementsKeeper);
        }

        private void InitPlayer()
        {
            _camera = Camera.main;
            var movableCamera = _camera.GetOrAddComponent<SmoothMoveComponent>();

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
                    // Add town menu
                    statesController.UseState(MainStateCode.MainMenu);
                }
            });

            _playerMovementController.StandOnOutputEvent.AddListener(() =>
            {
                var currentStateCode = statesController.CurrentState.Id;
                SetChildsActive(false);

                switch (currentStateCode)
                {
                    case MainStateCode.Map_Forest_1:
                        statesController.PushState(MainStateCode.Map_Forest_2);
                        break;
                    case MainStateCode.Map_Forest_2:
                        // Add town menu
                        statesController.ClearStatesStack();
                        statesController.UseState(MainStateCode.MainMenu);
                        break;
                }
            });
        }

        private void InitEnemies()
        {
            var emeniesCount = 5;
            _enemyNpcPool.Init();
            _enemyNpcController = new NpcMasterController(
                emeniesCount,
                _enemyNpcPool,
                _playerMovementController,
                _floorController.Map,
                NpcMovementController.Create);
        }

        private void InitFriends()
        {
            var friendsCount = 5;
            _friendlyNpcPool.Init();
            _friendNpcController = new NpcMasterController(
                friendsCount,
                _friendlyNpcPool,
                _playerMovementController,
                _floorController.Map,
                NpcMovementController.Create);
        }

        private void InitNpcInteraction()
        {
            InteractionController = new NpcInteractionController(_playerMovementController, _enemyNpcController, _friendNpcController, Root.Data);

            InteractionController.EnemyInteractionEvent.AddListener((stateCode) =>
            {
                statesController.PushState(stateCode);
            });

            InteractionController.FriendInteractionEvent.AddListener((stateCode) =>
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

            InteractionController.EnemyInteractionEvent.RemoveAllListeners();
            InteractionController.FriendInteractionEvent.RemoveAllListeners();
        }

        public void OnUpdate()
        {
            if (!IsInited) return;

            var buttonDirection = GetButtonsDirection();

            _playerMovementController.OnUpdate(buttonDirection);

            _enemyNpcController.OnUpdate();
            _friendNpcController.OnUpdate();

            InteractionController.OnUpdate();
        }

        public void SetPause(bool pause)
        {
            _playerMovementController.SetPause(pause);
            _enemyNpcController.SetPause(pause);
            _friendNpcController.SetPause(pause);
        }

        public void SetChildsActive(bool isActive)
        {
            _floorController.SetFloorActive(isActive);
            _playerMovementController.SetActive(isActive);
            _enemyNpcController.SetActive(isActive);
            _friendNpcController.SetActive(isActive);
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
