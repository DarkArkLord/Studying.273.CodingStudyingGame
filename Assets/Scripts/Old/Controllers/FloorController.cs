using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class FloorController
    {
        public MapController Map { get; private set; }

        private ObjectPoolComponent objectPool;
        private Material[] materials;

        const int VerticalOffset = -1;

        private GameObject[,] floor;

        public FloorController(MapController mapController, ObjectPoolComponent objectPool, Material[] materials)
        {
            Map = mapController;
            this.objectPool = objectPool;
            this.materials = materials;

            Clear();
            Redraw();
        }

        public void Redraw()
        {
            for (int x = 0; x < floor.GetLength(0); x++)
            {
                for (int y = 0; y < floor.GetLength(1); y++)
                {
                    var obj = objectPool.GetObject();
                    floor[x, y] = obj;

                    var cell = Map.GetMapCell(x, y);

                    var rendererComponent = obj.GetComponent<Renderer>();
                    if (rendererComponent is null)
                    {
                        rendererComponent = obj.AddComponent<Renderer>();
                    }

                    rendererComponent.material = materials[cell];
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
                            objectPool.FreeObject(obj);
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
}