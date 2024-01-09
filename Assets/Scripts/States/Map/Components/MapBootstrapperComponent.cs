using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Map.Controllers;
using Assets.Scripts.StatesMachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.States.Map.Components
{
    public class MapBootstrapperComponent : BaseModel
    {
        private StatesController<MainStateCode> statesController;
        private bool IsInited = false;

        private FloorController _floorController;

        private Camera _camera;
        private GameObject _playerElement;
        private PlayerMovementController _playerMovementController;

        private NPCMasterController _npcController;

        public BattleController BattleController {  get; private set; }

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
            var mapController = new MapController(20, 20, 3, 3);

            var objectsPool = new GameObject("FloorObjectPool").AddComponent<ObjectPoolComponent>();
            objectsPool.transform.parent = transform;
            var floorPrefub = Resources.Load("Models/FloorModel") as GameObject;
            objectsPool.Init(floorPrefub);

            var floorMaterials = new Material[4]
            {
                Resources.Load("Materials/NoFloorMaterial") as Material,
                Resources.Load("Materials/FloorMaterial_1") as Material,
                Resources.Load("Materials/FloorMaterial_2") as Material,
                Resources.Load("Materials/FloorMaterial_3") as Material,
            };

            _floorController = new FloorController(mapController, objectsPool, floorMaterials);
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
            objectsPool.Init(npcPrefub);

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
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
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
