using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BootstrapperController : MonoBehaviour
{
    private Camera _camera;
    private GameObject _playerElement;

    private PlayerMovementController _playerMovementController;


    private void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        _camera = Camera.main;
        var movableCamera = _camera.AddComponent<SmoothMoveComponent>();

        var _playerPrefab = Resources.Load("Models/PlayerModel") as GameObject;
        _playerElement = Instantiate(_playerPrefab);
        _playerElement.transform.name = "Player";
        var movablePlayer = _playerElement.AddComponent<JumpComponent>();

        _playerMovementController = new PlayerMovementController(movablePlayer, movableCamera);
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
