using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.TextureGenerators
{
    public class PointedTextureGenerator : BaseTextureGenerator
    {
        public override Texture2D GenerateTexture2D(int width, int height)
        {
            var random = RandomUtils.Random;

            var texture = new Texture2D(width, height);
            texture.filterMode = filterMode;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var index = random.Next(colors.Length);
                    var color = colors[index];
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            return texture;
        }
    }
}
