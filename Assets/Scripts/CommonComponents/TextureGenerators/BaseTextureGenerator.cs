using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.TextureGenerators
{
    public abstract class BaseTextureGenerator : MonoBehaviour
    {
        [SerializeField]
        private ColorsConfigSO colorsConfig;
        public IReadOnlyList<Color> Colors => colorsConfig.Colors;

        [SerializeField]
        protected FilterMode filterMode;

        [SerializeField]
        private int width;
        [SerializeField]
        private int height;

        public abstract Texture2D GenerateTexture2D(int width, int height);

        public Texture2D GenerateTexture2D() => GenerateTexture2D(width, height);
    }
}
