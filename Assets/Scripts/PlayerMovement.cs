using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloorController Floor;
    public JumpController Jumper;

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
            isBlocked = Jumper.Move();
            if (!isBlocked)
            {
                transform.position = Vector3.zero;
                Floor.Clear();
                Floor.Redraw();
            }
        }
        else
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
}
