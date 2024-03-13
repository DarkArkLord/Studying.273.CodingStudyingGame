using Assets.Scripts.States.Map.Components;
using Assets.Scripts.States.Map.Components.Generators;
using UnityEngine;

namespace Assets.Scripts.States.Map.Controllers
{
    public class FloorController
    {
        public MapController Map { get; private set; }

        private FloorElementsKeeper floorElementsKeeper;

        const int VerticalOffset = -1;

        private int[,] floorElement;
        private GameObject[,] floor;

        public FloorController(MapController mapController, FloorElementsKeeper floorElementsKeeper)
        {
            Map = mapController;
            this.floorElementsKeeper = floorElementsKeeper;
            floorElement = Common.GenerateMapObjectIndexes(mapController.MapConfig,
                floorElementsKeeper.PathMaterials.Length,
                floorElementsKeeper.WallMaterials.Length);

            floorElementsKeeper.ObjectPool.Init();

            Clear();
            Redraw();
        }

        public void Redraw()
        {
            for (int x = 0; x < floor.GetLength(0); x++)
            {
                for (int y = 0; y < floor.GetLength(1); y++)
                {
                    var obj = floorElementsKeeper.ObjectPool.GetObject();
                    floor[x, y] = obj;

                    var cell = Map.GetMapCell(x, y);

                    var rendererComponent = obj.GetComponent<Renderer>();
                    if (rendererComponent is null)
                    {
                        rendererComponent = obj.AddComponent<Renderer>();
                    }

                    rendererComponent.material = GetFloorElementMaterial(cell, floorElement[x, y]);
                    obj.transform.position = new Vector3(x, VerticalOffset, y);
                    obj.SetActive(true);
                }
            }
        }

        private Material GetFloorElementMaterial(MapCellContent cell, int materialIndex)
        {
            switch (cell)
            {
                case MapCellContent.None: return floorElementsKeeper.NoneMaterial;
                case MapCellContent.Input: return floorElementsKeeper.InputMaterial;
                case MapCellContent.Output: return floorElementsKeeper.OutputMaterial;
                case MapCellContent.Path: return floorElementsKeeper.PathMaterials[materialIndex];
                case MapCellContent.Wall: return floorElementsKeeper.WallMaterials[materialIndex];
                default: return null;
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
                            floorElementsKeeper.ObjectPool.FreeObject(obj);
                            floor[x, y] = null;
                        }
                    }
                }
            }
        }

        public void SetFloorActive(bool active)
        {
            floorElementsKeeper.ObjectPool.gameObject.SetActive(active);
        }
    }
}