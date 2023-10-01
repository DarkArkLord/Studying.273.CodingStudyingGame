using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloorController Floor;
    public JumpComponent Jumper;
    public SmoothMoveComponent CameraMove;

    private bool isBlocked = false;

    public bool IsActive { get; private set; } = true;

    public void SetActive(bool active)
    {
        IsActive = active;
    }

    public void SetStartPosition()
    {
        var offset = new Vector3(Floor.Map.StartX, transform.position.y, Floor.Map.StartY) - transform.position;
        transform.position += offset;
        CameraMove.transform.position += offset;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetStartPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            if (isBlocked)
            {
                // Тут я использую именно | вместо || чтобы оба метода были вызваны
                // Here I use | instead of || that both method were used
                isBlocked = Jumper.Move() | CameraMove.Move();
            }
            else
            {
                CheckInputStep();
            }
        }
    }

    private Vector2Int GetButtonsVector()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return Vector2Int.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            return Vector2Int.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            return Vector2Int.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            return Vector2Int.down;
        }
        return Vector2Int.zero;
    }

    private void CheckInputStep()
    {
        var input = GetButtonsVector();
        if (input.magnitude > 0)
        {
            isBlocked = true;
            var newPosition = Jumper.Position2D + input;
            var isMoved = Floor.Map.IsCanMove(newPosition);
            if (isMoved)
            {
                var offset = new Vector3(input.x, 0, input.y);
                var playerTarget = transform.position + offset;
                Jumper.SetTarget(playerTarget);
                var cameraTarget = CameraMove.transform.position + offset;
                CameraMove.SetTarget(cameraTarget);
            }
            else
            {
                Jumper.SetTarget(transform.position);
            }
        }
    }
}
