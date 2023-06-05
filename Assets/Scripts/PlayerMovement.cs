using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloorController Floor;
    public JumpController Jumper;
    public MoveController CameraMove;

    private bool isBlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
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

    private Vector2 GetButtonsVector()
    {
        var result = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            result += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            result += Vector2.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            result += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            result += Vector2.down;
        }
        return result;
    }

    private void CheckInputStep()
    {
        var input = GetButtonsVector();
        if (input.magnitude > 0)
        {
            isBlocked = true;
            var isMoved = Floor.Map.MovePlayer(input);
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
