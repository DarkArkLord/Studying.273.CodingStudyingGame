using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public ObjectPoolComponent ObjectPool;
    public Material[] Materials;
    public MapController Map;

    const int VerticalOffset = -1;

    private GameObject[,] floor;

    void Start()
    {
        Clear();
        Redraw();
    }

    public void Redraw()
    {
        for (int x = 0; x < floor.GetLength(0); x++)
        {
            for (int y = 0; y < floor.GetLength(1); y++)
            {
                var obj = ObjectPool.GetObject();
                floor[x, y] = obj;

                var cell = Map.GetMapCell(x, y);
                obj.GetComponent<Renderer>().material = Materials[cell];
                obj.transform.position = new Vector3(x, VerticalOffset, y);
                obj.SetActive(true);
            }
        }
    }

    public void Clear()
    {
        if (floor == null)
        {
            floor = new GameObject[Map.Width, Map.Height];
        }
        else
        {
            for (int x = 0; x < floor.GetLength(0); x++)
            {
                for (int y = 0; y < floor.GetLength(1); y++)
                {
                    if (floor[x, y] != null)
                    {
                        var obj = floor[x, y];
                        ObjectPool.FreeObject(obj);
                        floor[x, y] = null;
                    }
                }
            }
        }
    }

    public IEnumerable<GameObject> GetObjects()
    {
        for (int x = 0; x < floor.GetLength(0); x++)
        {
            for (int y = 0; y < floor.GetLength(1); y++)
            {
                yield return floor[x, y];
            }
        }
    }
}
