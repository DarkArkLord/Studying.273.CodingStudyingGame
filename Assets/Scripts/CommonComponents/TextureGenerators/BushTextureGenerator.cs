using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.TextureGenerators
{
    public class BushTextureGenerator : BaseTextureGenerator
    {
        [SerializeField]
        private Color[] branchColors;

        [SerializeField]
        private int branchesCountFrom = 5;

        [SerializeField]
        private int branchesCountTo = 20;

        public override Texture2D GenerateTexture2D(int width, int height)
        {
            var random = RandomUtils.Random;

            var texture = new Texture2D(width, height);
            texture.filterMode = filterMode;

            FillBack(texture, random);

            var branchesCount = random.Next(branchesCountFrom, branchesCountTo);
            PrintBranches(texture, random, branchesCount);

            texture.Apply();

            return texture;
        }

        private void FillBack(Texture2D texture, System.Random random)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    var index = random.Next(colors.Length);
                    var color = colors[index];
                    texture.SetPixel(x, y, color);
                }
            }
        }

        private void PrintBranches(Texture2D texture, System.Random random, int branchesCount)
        {
            var branchWidth = texture.width / branchesCount;
            for (int i = 0; i < branchesCount; i++)
            {
                var brabchLeftBorder = branchWidth * i;
                var branchRightBorder = branchWidth * (i + 1);
                var branchStartPoint = random.Next(brabchLeftBorder, branchRightBorder);

                PrintBranch(texture, random, branchStartPoint);
            }
        }

        private void PrintBranch(Texture2D texture, System.Random random, int branchStartPoint)
        {
            var endBottomBorder = texture.height / 2;
            var endTopBorder = texture.height / 10 * 8;
            var endBorder = random.Next(endBottomBorder, endTopBorder);

            int x = branchStartPoint;
            int y = 0;
            while (y < endBorder)
            {
                var step = random.Next(100) + 1;

                if (step <= 20)
                {
                    if (x - 1 < 0) continue;

                    if(random.Next() % 20 == 0)
                    {
                        PrintSubBranch(texture, random, x, y, endBorder);
                    }

                    x--;
                }
                else if (step >= 80)
                {
                    if (x + 1 >= texture.width) continue;

                    if (random.Next() % 20 == 0)
                    {
                        PrintSubBranch(texture, random, x, y, endBorder);
                    }

                    x++;
                }
                else
                {
                    y++;
                }

                var colorIndex = random.Next(branchColors.Length);
                texture.SetPixel(x, y, branchColors[colorIndex]);
            }
        }

        private void PrintSubBranch(Texture2D texture, System.Random random, int startPointX, int startPointY, int endPoint)
        {
            var heightPart = texture.height / 10;
            var endPointOffset = random.Next(-heightPart, heightPart);
            endPoint += endPointOffset;

            int x = startPointX;
            int y = startPointY;
            while (y < endPoint)
            {
                var step = random.Next(100) + 1;

                if (step <= 20)
                {
                    if (x - 1 < 0) continue;

                    x--;
                }
                else if (step >= 80)
                {
                    if (x + 1 >= texture.width) continue;

                    x++;
                }
                else
                {
                    y++;
                }

                var colorIndex = random.Next(branchColors.Length);
                texture.SetPixel(x, y, branchColors[colorIndex]);
            }
        }
    }
}
