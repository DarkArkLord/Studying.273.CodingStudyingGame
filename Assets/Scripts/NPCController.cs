using Assets.Scripts;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public JumpController Jumper;
    public MapController Map;
    public PlayerMovement Player;
    public float TimeBeforeStep = 2;

    public bool IsAlive { get; private set; } = true;
    public bool IsActive { get; private set; } = true;

    private System.Random random = new System.Random(666);

    private bool isMoving = false;
    private float waitingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        waitingTime = TimeBeforeStep;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            if (isMoving && IsAlive)
            {
                isMoving = Jumper.Move();
            }
            else
            {
                waitingTime -= Time.deltaTime;
                if (waitingTime < 0)
                {
                    isMoving = true;
                    SetMoveTarget();
                    waitingTime = TimeBeforeStep;

                    if (!IsAlive)
                    {
                        IsAlive = true;
                        SetStartPosition();
                        Jumper.SetTarget(transform.position);
                    }
                }
            }
        }
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }

    public void SetAlive(bool alive)
    {
        IsAlive = alive;
    }

    public void SetVisibility(bool visibility)
    {
        gameObject.SetActive(visibility);
    }

    public void SetStartPosition()
    {
        var playerPos = Player.Jumper.Position2D;
        while (true)
        {
            var x = random.Next(Map.Width);
            var y = random.Next(Map.Height);

            if (Map.IsCanMove(x, y) && playerPos.x != x && playerPos.y != y)
            {
                gameObject.transform.position = new Vector3(x, 0, y);
                break;
            }
        }
    }

    public void SetMoveTarget()
    {
        while (true)
        {
            var target = Jumper.Position2D + random.GetRandomDirection();
            if (Map.IsCanMove(target))
            {
                var target3D = new Vector3(target.x, 0, target.y);
                Jumper.SetTarget(target3D);
                return;
            }
        }
    }
}
