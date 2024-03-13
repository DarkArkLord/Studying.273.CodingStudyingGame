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

            var _playerPrefab = Resources.Load("Models/PlayerModel") as GameObject;
            _playerElement = Instantiate(_playerPrefab);
            _playerElement.transform.name = "Player";
            _playerElement.transform.parent = transform;
            var movablePlayer = _playerElement.AddComponent<JumpComponent>();

            _playerMovementController = new PlayerMovementController(movablePlayer, movableCamera, _floorController);
            _playerMovementController.Resurrect();
        }

        private void InitEnemies()
        {
            var npcPrefub = Resources.Load("Models/NPCCube") as GameObject;
            var objectsPool = new GameObject("NpcObjectPool").AddComponent<ObjectPoolComponent>();
            objectsPool.transform.parent = transform;
            objectsPool.SetPrefab(npcPrefub);
            objectsPool.Init();

            var movablePlayer = _playerElement.GetComponent<JumpComponent>();

            _npcController = new NPCMasterController(5, objectsPool, movablePlayer, _floorController.Map);
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
            // Destroy player and enemy
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
            // Set active for player and enemy
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
