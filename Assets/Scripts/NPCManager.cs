using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public int EnemiesCount;

    public ObjertPoolManager ObjectPool;
    public MapController Map;

    public NPCController[] Enemies;

    private System.Random random = new System.Random(666);

    // Start is called before the first frame update
    void Start()
    {
        Enemies = new NPCController[EnemiesCount];
        Map.enemies = new Vector2Int[EnemiesCount];
        for (int i = 0; i < Enemies.Length; i++)
        {
            var obj = ObjectPool.GetObject();
            Enemies[i] = obj.GetComponent<NPCController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
