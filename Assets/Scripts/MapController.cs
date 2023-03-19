using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour
{
    private int[,] map;
    private Vector2 player;

    // Start is called before the first frame update
    void Start()
    {
        map = new int[10, 10]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 2, 1, 1, 2, 2, 1, 1, 2, 1 },
            { 1, 1, 2, 3, 1, 1, 3, 2, 1, 1 },
            { 1, 1, 3, 2, 3, 3, 2, 3, 1, 1 },
            { 1, 2, 1, 3, 2, 2, 3, 1, 2, 1 },
            { 1, 2, 1, 3, 2, 2, 3, 1, 2, 1 },
            { 1, 1, 3, 2, 3, 3, 2, 3, 1, 1 },
            { 1, 1, 2, 3, 1, 1, 3, 2, 1, 1 },
            { 1, 2, 1, 1, 2, 2, 1, 1, 2, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        };
        player = new Vector2(3, 3);
    }

    public int GetMapCell(int x, int y) => map[x, y];

    public int GetRelativeMapCell(int x, int y)
    {
        var tx = (int)(player.x + x);
        var ty = (int)(player.y + y);
        if (tx < 0 || tx >= map.GetLength(0) || ty < 0 || ty >= map.GetLength(1))
        {
            return 0;
        }
        return map[tx, ty];
    }

    public Vector2 GetPlayerPosition() => player;

    public bool MovePlayer(MoveDirection direction)
    {
        var temp = player + direction.DirectionToVector();
        var tx = (int)temp.x;
        var ty = (int)temp.y;
        if (tx < 0 || tx >= map.GetLength(0) || ty < 0 || ty >= map.GetLength(1) || map[tx, ty] < 1)
        {
            return false;
        }
        else
        {
            player = temp;
            return true;
        }
    }
}
