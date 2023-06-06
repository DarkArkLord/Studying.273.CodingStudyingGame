using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public ObjectPoolManager ObjectPool;
    public MapController Map;
    public PlayerMovement Player;

    public UIController UIController;

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
            enemy.SetInBattle(true);
            Player.SetActive(false);
        }

        if (enemy.IsInBattle)
        {
            if (!UIController.IsActive)
            {
                UIController.SetActive(true);
            }
            else
            {
                if (UIController.IsComplete)
                {
                    Player.SetActive(true);
                    enemy.SetVisibility(true);
                }
            }
        }
    }
}
