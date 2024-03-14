using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Assets.Scripts.States.Map.Components.Generators
{
    public class Forest_2_Generator : BaseMapGenerator
    {
        public override MapPathConfig GeneratePathMap(int width, int height)
        {
            var random = RandomUtils.Random;

            var map = new MapCellContent[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (random.Next() % 5 == 0)
                    {
                        map[x, y] = MapCellContent.Wall;
                    }
                    else
                    {
                        map[x, y] = MapCellContent.Path;
                    }
                }
            }

            var input = GenerateInputPoint(random, width, height);
            map[input.x, input.y] = MapCellContent.Input;

            var output = GenerateOutputPoint(random, width, height);
            map[output.x, output.y] = MapCellContent.Output;

            while (true)
            {
                var hasPath = BfsHasPath(map, input);
                bool incorrectMap = false;
                for (int x = 0; x < width && !incorrectMap; x++)
                {
                    for (int y = 0; y < height && !incorrectMap; y++)
                    {
                        incorrectMap = map[x, y].IsMoveble() && !hasPath[x, y];
                    }
                }

                if (incorrectMap)
                {
                    CorrectMap(map, hasPath);
                }
                else
                {
                    break;
                }
            }

            return new MapPathConfig()
            {
                Width = width,
                Height = height,
                Map = map,
                InputPosition = input,
                OutputPosition = output,
            };
        }

        private static bool[,] BfsHasPath(MapCellContent[,] field, Vector2Int input)
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);

            var hasPath = new bool[width, height];
            var wasChecked = new bool[width, height];
            var queue = new Queue<Vector2Int>();

            wasChecked[input.x, input.y] = true;
            queue.Enqueue(input);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                hasPath[current.x, current.y] = true;
                CheckCell(field, wasChecked, current.x + 1, current.y, queue);
                CheckCell(field, wasChecked, current.x - 1, current.y, queue);
                CheckCell(field, wasChecked, current.x, current.y + 1, queue);
                CheckCell(field, wasChecked, current.x, current.y - 1, queue);
            }

            return hasPath;
        }

        private static void CheckCell(MapCellContent[,] field, bool[,] wasChecked, int x, int y, Queue<Vector2Int> queue)
        {
            if (IsCellHasPath(field, x, y) && !wasChecked[x, y])
            {
                wasChecked[x, y] = true;
                queue.Enqueue(new Vector2Int(x, y));
            }
        }

        private static bool IsCellHasPath(MapCellContent[,] field, int x, int y)
        {
            return x >= 0
                && y >= 0
                && x < field.GetLength(0)
                && y < field.GetLength(1)
                && field[x, y].IsMoveble();
        }

        private static void CorrectMap(MapCellContent[,] field, bool[,] hasPath)
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (field[x, y].IsMoveble() && !hasPath[x, y])
                    {
                        FindCorrectinoPathForCell(field, hasPath, x, y);
                        return;
                    }
                }
            }
        }

        private static void FindCorrectinoPathForCell(MapCellContent[,] field, bool[,] hasPath, int cellX, int cellY)
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);
            int stepsCount = (width + height) / 4;

            for (int i = 1; i < stepsCount; i++)
            {
                if (IsCellHasPath(field, cellX + i, cellY) && hasPath[cellX + i, cellY])
                {
                    CorrectPathToCell(field, cellX + i, cellY, cellX, cellY);
                    return;
                }
                else if (IsCellHasPath(field, cellX - i, cellY) && hasPath[cellX - i, cellY])
                {
                    CorrectPathToCell(field, cellX - i, cellY, cellX, cellY);
                    return;
                }
                else if (IsCellHasPath(field, cellX, cellY + i) && hasPath[cellX, cellY + i])
                {
                    CorrectPathToCell(field, cellX, cellY + i, cellX, cellY);
                    return;
                }
                else if (IsCellHasPath(field, cellX, cellY - i) && hasPath[cellX, cellY - i])
                {
                    CorrectPathToCell(field, cellX, cellY - i, cellX, cellY);
                    return;
                }
            }
        }

        private static void CorrectPathToCell(MapCellContent[,] field, int fromX, int fromY, int toX, int toY)
        {
            int startX = Math.Min(fromX, toX);
            int startY = Math.Min(fromY, toY);
            int endX = Math.Max(fromX, toX);
            int endY = Math.Max(fromY, toY);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (!field[x, y].IsMoveble())
                    {
                        field[x, y] = MapCellContent.Path;
                    }
                }
            }
        }
    }
}
