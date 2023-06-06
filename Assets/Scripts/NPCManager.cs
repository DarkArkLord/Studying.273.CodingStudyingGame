using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public int EnemiesCount;

    public ObjertPoolManager ObjectPool;
    public MapController Map;
    public PlayerMovement Player;

    public bool IsAlive { get; private set; } = true;
    public bool IsActive { get; private set; } = true;

    private GameObject enemyObj;
    private NPCController enemy;
    private System.Random random = new System.Random(666);

    // Start is called before the first frame update
    void Start()
    {
        enemyObj = ObjectPool.GetObject();
        enemy = enemyObj.GetComponent<NPCController>();
        SetEnemyPosition();
        enemyObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive && IsAlive && Player.Jumper.Position2D == enemy.Jumper.Position2D)
        {
            IsActive = false;
            SetEnemyPosition();
            IsActive = true;
        }
    }

    void SetEnemyPosition()
    {
        var playerPos = Player.Jumper.Position2D;
        while (true)
        {
            var x = random.Next(Map.Width);
            var y = random.Next(Map.Height);

            if (Map.IsCanMove(x, y) && playerPos.x != x && playerPos.y != y)
            {
                enemy.transform.position = new Vector3(x, 0, y);
                break;
            }
        }
    }
}
