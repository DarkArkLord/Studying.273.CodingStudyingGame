using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public ObjertPoolManager ObjectPool;
    public GameObject Anchor;
    public int WidthRadius;
    public int HeightRadius;

    private GameObject[,] floor;

    void Start()
    {
        var centerPosition = Anchor.transform.position;
        var diameterX = WidthRadius + 1 + WidthRadius;
        var diameterY = HeightRadius + 1 + HeightRadius;
        var floorObjects = ObjectPool.GetObjects(diameterX * diameterY).ToArray();
        var fi = 0;
        floor = new GameObject[diameterX, diameterY];
        for (int x = -WidthRadius; x <= WidthRadius; x++)
        {
            for (int y = -HeightRadius; y <= HeightRadius; y++)
            {
                var xi = WidthRadius + x;
                var yi = HeightRadius + y;

                var obj = floorObjects[fi];
                fi++;
                floor[xi, yi] = obj;

                obj.transform.position = centerPosition + new Vector3(x, -1, y);
                obj.SetActive(true);
            }
        }
    }
}
