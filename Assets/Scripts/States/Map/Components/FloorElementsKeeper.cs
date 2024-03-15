using Assets.Scripts.CommonComponents;
using Assets.Scripts.CommonComponents.TextureGenerators;
using UnityEngine;

namespace Assets.Scripts.States.Map.Components
{
    public class FloorElementsKeeper : MonoBehaviour
    {
        [SerializeField]
        private ObjectPoolComponent pathObjectPool;
        public ObjectPoolComponent PathObjectPool => pathObjectPool;

        [SerializeField]
        private ObjectPoolComponent wallObjectPool;
        public ObjectPoolComponent WallObjectPool => wallObjectPool;

        [SerializeField]
        private BaseTextureGenerator pathTextureGenerator;
        public BaseTextureGenerator PathTextureGenerator => pathTextureGenerator;

        [SerializeField]
        private BaseTextureGenerator wallTextureGenerator;
        public BaseTextureGenerator WallTextureGenerator => wallTextureGenerator;

        [SerializeField]
        private Color inputColor;
        public Color InputColor => inputColor;

        [SerializeField]
        private Color outputColor;
        public Color OutputColor => outputColor;
    }
}
