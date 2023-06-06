using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public ObjertPoolManager ObjectPool;
    public MapController Map;
    public PlayerMovement Player;

    private GameObject enemyObj;
    private NPCController enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemyObj = ObjectPool.GetObject();
        enemy = enemyObj.GetComponent<NPCController>();
        enemy.Map = Map;
        enemy.Player = Player;
        enemy.SetStartPosition();
        enemyObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.IsActive && enemy.IsAlive && Player.Jumper.Position2D == enemy.Jumper.Position2D)
        {
            enemy.SetActive(false);
            enemy.SetVisibility(false);

            enemy.SetStartPosition();

            enemy.SetVisibility(true);
            enemy.SetActive(true);
        }
    }
}
