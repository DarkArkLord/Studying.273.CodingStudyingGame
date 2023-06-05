using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour
{
    public int Width = 20, Height = 20, StartX = 3, StartY = 3;

    private int[,] map;
    private Vector2Int player;

    public Vector2Int[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        map = MapGenerator.GenerateMap(Width, Height, 4, StartX, StartY);
        player = new Vector2Int(StartX, StartY);
    }

    public int GetPlayerCell() => GetMapCell(player.x, player.y);

    public int GetMapCell(int x, int y) => map[x, y];

    public int GetMapCell(float x, float y) => map[(int)x, (int)y];

    public int GetRelativeMapCell(int x, int y)
    {
        var tx = player.x + x;
        var ty = player.y + y;
        if (tx < 0 || tx >= map.GetLength(0) || ty < 0 || ty >= map.GetLength(1))
        {
            return 0;
        }
        return map[tx, ty];
    }

    public int GetRelativeMapCell(float x, float y) => GetRelativeMapCell((int)x, (int)y);

    public Vector2Int GetPlayerPosition() => player;

    public bool MovePlayer(MoveDirection direction)
    {
        var temp = player + direction.DirectionToVectorInt();
        if (temp.x < 0 || temp.x >= map.GetLength(0) || temp.y < 0 || temp.y >= map.GetLength(1) || map[temp.x, temp.y] < 1)
        {
            return false;
        }
        else
        {
            player = temp;
            return true;
        }
    }

    public bool MovePlayer(Vector2Int offset)
    {
        var temp = player + offset;
        if (temp.x < 0 || temp.x >= map.GetLength(0) || temp.y < 0 || temp.y >= map.GetLength(1) || map[temp.x, temp.y] < 1)
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
