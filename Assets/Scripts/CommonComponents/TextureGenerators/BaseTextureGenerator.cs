using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.TextureGenerators
{
    public abstract class BaseTextureGenerator : MonoBehaviour
    {
        [SerializeField]
        private Color[] colors;

        [SerializeField]
        private ColorsConfigComponent colorsConfig;

        public IReadOnlyList<Color> Colors
            => colors != null && colors.Length > 0
            ? colors
            : colorsConfig.Colors;

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
