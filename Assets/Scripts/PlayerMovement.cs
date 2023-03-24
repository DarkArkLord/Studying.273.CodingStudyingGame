using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloorController Floor;
    public JumpController Jumper;

    private bool isBlocked = false;
    private bool isMoveBack = false;

    private float jumpTime = 0.25f;
    private float moveBackTime = 0.1f;

    private float playerShiftLimit = 2f;

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
            if (isMoveBack)
            {
                MoveBackStep();
            }
            else
            {
                MovePlayerStep();
            }
        }
        else
        {
            CheckInputStep();
        }
    }

    private Vector2 GetButtonsVector()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            return Vector2.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            return Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            return Vector2.down;
        }
        return Vector2.zero;
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
                var target = transform.position + new Vector3(input.x, 0, input.y);
                Jumper.SetTarget(target);
            }
            else
            {
                Jumper.SetTarget(transform.position);
            }
        }
    }

    private void MoveBackStep()
    {
        isBlocked = Jumper.Move();
        foreach (var item in Floor.GetObjects())
        {
            var moveItem = item.GetComponent<MoveController>();
            if (moveItem != null)
            {
                isBlocked |= moveItem.Move();
            }
        }

        if (!isBlocked)
        {
            isMoveBack = false;
            Jumper.NeedJump = true;
            Jumper.MoveTime = jumpTime;
            Floor.Clear();
            Floor.Redraw();
        }
    }

    private void MovePlayerStep()
    {
        isBlocked = Jumper.Move();
        if (!isBlocked && transform.position.magnitude >= playerShiftLimit)
        {
            isBlocked = isMoveBack = true;
            Jumper.SetTarget(Vector3.zero);
            Jumper.NeedJump = false;
            Jumper.MoveTime = moveBackTime;
            foreach (var item in Floor.GetObjects())
            {
                var moveItem = item.GetComponent<MoveController>();
                if (moveItem != null)
                {
                    moveItem.SetTarget(item.transform.position - transform.position);
                    moveItem.MoveTime = moveBackTime;
                }
            }
        }
    }
}
