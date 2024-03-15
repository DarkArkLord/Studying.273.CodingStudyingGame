using Assets.Scripts.CommonComponents;
using Assets.Scripts.CommonComponents.TextureGenerators;
using Assets.Scripts.States.Map.Components;
using Assets.Scripts.States.Map.Components.MapGenerators;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.States.Map.Controllers
{
    public class FloorController
    {
        public MapController Map { get; private set; }

        private FloorElementsKeeper floorElementsKeeper;

        const int VerticalOffset = -1;

        private const int texturesCount = 10;

        private Texture[] pathTextures;
        private Texture[] wallTextures;

        private GameObject[,] floor;

        public FloorController(MapController mapController, FloorElementsKeeper floorElementsKeeper)
        {
            Map = mapController;
            this.floorElementsKeeper = floorElementsKeeper;

            floorElementsKeeper.PathObjectPool.Init();
            floorElementsKeeper.WallObjectPool.Init();

            GenerateTextures();

            Clear();
            Redraw();
        }

        private void GenerateTextures()
        {
            pathTextures = new Texture[texturesCount];
            wallTextures = new Texture[texturesCount];

            for (int i = 0; i < texturesCount; i++)
            {
                pathTextures[i] = floorElementsKeeper.PathTextureGenerator.GenerateTexture2D();
                wallTextures[i] = floorElementsKeeper.WallTextureGenerator.GenerateTexture2D();
            }
        }

        public void Redraw()
        {
            var random = RandomUtils.Random;

            for (int x = 0; x < floor.GetLength(0); x++)
            {
                for (int y = 0; y < floor.GetLength(1); y++)
                {
                    var cell = Map.GetMapCell(x, y);
                    var pool = GetPool(cell);

                    var obj = floor[x, y] = pool.GetObject();
                    var rendererComponent = obj.GetComponent<FloorObjectRenderComponent>();

                    SetRenderContent(cell, rendererComponent, random);

                    obj.transform.position = new Vector3(x, VerticalOffset, y);
                    obj.SetActive(true);
                }
            }
        }

        private ObjectPoolComponent GetPool(MapCellContent cell)
        {
            if (cell == MapCellContent.Wall)
            {
                return floorElementsKeeper.WallObjectPool;
            }

            return floorElementsKeeper.PathObjectPool;
        }

        private void SetRenderContent(MapCellContent cell, FloorObjectRenderComponent obj, System.Random random)
        {
            var textureIndex = random.Next(texturesCount);
            switch (cell)
            {
                case MapCellContent.Path:
                    obj.SetTexture(pathTextures[textureIndex]);
                    return;
                case MapCellContent.Wall:
                    obj.SetTexture(wallTextures[textureIndex]);
                    return;
                case MapCellContent.Input:
                    obj.SetColor(floorElementsKeeper.InputColor);
                    return;
                case MapCellContent.Output:
                    obj.SetColor(floorElementsKeeper.OutputColor);
                    return;
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
                            var cell = Map.GetMapCell(x, y);
                            var pool = GetPool(cell);

                            var obj = floor[x, y];
                            pool.FreeObject(obj);

                            floor[x, y] = null;
                        }
                    }
                }
            }
        }

        public void SetFloorActive(bool active)
        {
            floorElementsKeeper.PathObjectPool.gameObject.SetActive(active);
            floorElementsKeeper.WallObjectPool.gameObject.SetActive(active);
        }
    }
}