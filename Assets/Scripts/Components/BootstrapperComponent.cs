using Assets.Scripts.Controllers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BootstrapperComponent : MonoBehaviour
{
    private Camera _camera;
    private GameObject _playerElement;

    private PlayerMovementController _playerMovementController;
    private FloorController _floorController;


    private void Awake()
    {
        InitMap();
        InitPlayer();
    }

    private void InitMap()
    {
        var mapController = new MapController(20, 20, 3, 3);

        var objectsPool = new GameObject("FlootObjectPool").AddComponent<ObjectPoolComponent>();
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
        var movableCamera = _camera.AddComponent<SmoothMoveComponent>();

        var _playerPrefab = Resources.Load("Models/PlayerModel") as GameObject;
        _playerElement = Instantiate(_playerPrefab);
        _playerElement.transform.name = "Player";
        var movablePlayer = _playerElement.AddComponent<JumpComponent>();

        _playerMovementController = new PlayerMovementController(movablePlayer, movableCamera, _floorController);
        _playerMovementController.SetStartPosition();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var buttonDirection = GetButtonsDirection();
        _playerMovementController.OnUpdate(buttonDirection);
        // Add keys down to event bus )0)
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
