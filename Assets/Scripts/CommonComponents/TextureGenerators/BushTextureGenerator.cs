using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.TextureGenerators
{
    public class BushTextureGenerator : BaseTextureGenerator
    {
        [SerializeField]
        private ColorsConfigSO branchColorsConfig;
        public IReadOnlyList<Color> BranchColors => branchColorsConfig.Colors;

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
                    var index = random.Next(Colors.Count);
                    var color = Colors[index];
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

                var subBranchLin = (int)System.Math.Round((endBorder - y) * random.Next(50, 100) / 100.0);

                if (step <= 20)
                {
                    if (x - 1 >= 0)
                    {
                        if (random.Next() % 10 == 0)
                        {
                            PrintSubBranch(texture, random, x, y, subBranchLin, 1);
                        }

                        x--;
                        PrintBranchPixel(texture, random, x, y);
                    }
                }
                else if (step >= 80)
                {
                    if (x + 1 < texture.width)
                    {
                        if (random.Next() % 10 == 0)
                        {
                            PrintSubBranch(texture, random, x, y, subBranchLin, -1);
                        }

                        x++;
                        PrintBranchPixel(texture, random, x, y);
                    }
                }
                else
                {
                    y++;
                    PrintBranchPixel(texture, random, x, y);
                }

                y++;
                PrintBranchPixel(texture, random, x, y);
            }
        }

        private void PrintBranchPixel(Texture2D texture, System.Random random, int x, int y)
        {
            var colorIndex = random.Next(BranchColors.Count);
            texture.SetPixel(x, y, BranchColors[colorIndex]);
        }

        private void PrintSubBranch(Texture2D texture, System.Random random, int startPointX, int startPointY, int length, int xDirection)
        {
            int x = startPointX;
            int y = startPointY;
            while (length > 0)
            {
                var step = random.Next(100) + 1;

                if (step <= 50)
                {
                    if (x + xDirection >= 0 && x + xDirection < texture.width)
                    {
                        x += xDirection;
                        PrintBranchPixel(texture, random, x, y);
                        length--;
                    }
                }
                else if (step >= 80)
                {
                    if (x - xDirection >= 0 && x - xDirection < texture.width)
                    {
                        x -= xDirection;
                        PrintBranchPixel(texture, random, x, y);
                        length--;
                    }
                }
                else
                {
                    y++;
                    PrintBranchPixel(texture, random, x, y);
                    length--;
                }

                if (random.Next() % 2 == 0 && x + xDirection >= 0 && x + xDirection < texture.width)
                {
                    x += xDirection;
                    PrintBranchPixel(texture, random, x, y);
                    length--;
                }
                else
                {
                    y++;
                    PrintBranchPixel(texture, random, x, y);
                    length--;
                }
            }
        }
    }
}
