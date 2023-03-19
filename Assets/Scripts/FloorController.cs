using UnityEngine;

public class FloorController : MonoBehaviour
{
    public ObjertPoolManager ObjectPool;
    public Material[] Materials;
    public MapController Map;

    public GameObject Anchor;
    public int Radius;

    const int VerticalOffset = -1;

    private GameObject[,] floor;

    void Start()
    {
        Clear();
        Redraw();
    }

    public void Redraw()
    {
        var centerPosition = Anchor.transform.position;
        for (int x = -Radius; x <= Radius; x++)
        {
            for (int y = -Radius; y <= Radius; y++)
            {
                var xi = Radius + x;
                var yi = Radius + y;

                var obj = ObjectPool.GetObject();
                floor[xi, yi] = obj;

                var cell = Map.GetRelativeMapCell(x, y);
                obj.GetComponent<Renderer>().material = Materials[cell];
                obj.transform.position = centerPosition + new Vector3(x, VerticalOffset, y);
                obj.SetActive(true);
            }
        }
    }

    public void Clear()
    {
        if (floor == null)
        {
            var diameter = Radius + 1 + Radius;
            floor = new GameObject[diameter, diameter];
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
}
