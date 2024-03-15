using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Map.Components.Generators;
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

        private StatesController<MainStateCode> statesController;
        private bool IsInited = false;

        private FloorController _floorController;

        private Camera _camera;
        private GameObject _playerElement;
        private PlayerMovementController _playerMovementController;

        private NPCMasterController _npcController;

        public BattleController BattleController { get; private set; }

        public void OnMapInit(StatesController<MainStateCode> statesController)
        {
            this.statesController = statesController;
            InitMap();
            InitPlayer();
            InitEnemies();
            InitBattle();
            IsInited = true;
        }

        private void InitMap()
        {
            var mapController = new MapController(MapGenerator, 20, 20);
            _floorController = new FloorController(mapController, floorElementsKeeper);
        }

        private void InitPlayer()
        {
            _camera = Camera.main;
            var movableCamera = _camera.GetOrAddComponent<SmoothMoveComponent>();

            var _playerPrefab = Resources.Load("Models/Map/PlayerModel") as GameObject;
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
            var npcPrefub = Resources.Load("Models/Map/NPCCube") as GameObject;

            var objectsPool = new GameObject("NpcObjectPool").AddComponent<ObjectPoolComponent>();
            objectsPool.transform.parent = transform;
            objectsPool.SetPrefab(npcPrefub);
            objectsPool.Init();

            _npcController = new NPCMasterController(5, objectsPool, _playerMovementController, _floorController.Map);
        }

        private void InitBattle()
        {
            BattleController = new BattleController(_playerMovementController, _npcController, Root.Data);
            BattleController.StartBattleEvent.AddListener((stateCode) =>
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
            Destroy(_npcController.Pool.gameObject);
            BattleController.StartBattleEvent.RemoveAllListeners();
        }

        public void OnUpdate()
        {
            if (!IsInited) return;
            var buttonDirection = GetButtonsDirection();
            _playerMovementController.OnUpdate(buttonDirection);
            _npcController.OnUpdate();
            BattleController.OnUpdate();
        }

        public void SetPause(bool pause)
        {
            _playerMovementController.SetPause(pause);
            _npcController.SetPause(pause);
        }

        public void SetChildsActive(bool isActive)
        {
            _floorController.SetFloorActive(isActive);
            _playerMovementController.SetActive(isActive);
            _npcController.SetActive(isActive);
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
