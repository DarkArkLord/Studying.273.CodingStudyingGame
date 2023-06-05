using System;
using PointsQueue = System.Collections.Generic.Queue<(int x, int y)>;

namespace Assets.Scripts
{
    internal static class MapGenerator
    {
        public static int[,] GenerateMap(int width, int height, int maxCellValue, int playerX, int playerY)
        {
            var random = new System.Random(666);
            int[,] map = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = random.Next(maxCellValue);
                }
            }

            map[playerX, playerY] = 1;

            while (true)
            {
                var hasPath = BfsHasPath(map, playerX, playerY);
                bool incorrectMap = false;
                for (int x = 0; x < width && !incorrectMap; x++)
                {
                    for (int y = 0; y < height && !incorrectMap; y++)
                    {
                        incorrectMap = map[x, y] > 0 && !hasPath[x, y];
                    }
                }

                if (incorrectMap)
                {
                    CorrectMap(map, hasPath);
                    continue;
                }
                break;
            }

            return map;
        }

        private static bool[,] BfsHasPath(int[,] field, int startX, int startY)
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);

            var hasPath = new bool[width, height];
            var wasChecked = new bool[width, height];
            var queue = new PointsQueue();

            wasChecked[startX, startY] = true;
            queue.Enqueue((startX, startY));
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

        private static void CheckCell(int[,] field, bool[,] wasChecked, int x, int y, PointsQueue queue)
        {
            if (IsCellHasPath(field, x, y) && !wasChecked[x, y])
            {
                wasChecked[x, y] = true;
                queue.Enqueue((x, y));
            }
        }

        private static bool IsCellHasPath(int[,] field, int x, int y)
        {
            return x >= 0
                && y >= 0
                && x < field.GetLength(0)
                && y < field.GetLength(1)
                && field[x, y] > 0;
        }

        private static void CorrectMap(int[,] field, bool[,] hasPath)
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (field[x, y] > 0 && !hasPath[x, y])
                    {
                        FindCorrectinoPathForCell(field, hasPath, x, y);
                        return;
                    }
                }
            }
        }

        private static void FindCorrectinoPathForCell(int[,] field, bool[,] hasPath, int cellX, int cellY)
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);
            int stepsCount = (width + height) / 2;

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

        private static void CorrectPathToCell(int[,] field, int fromX, int fromY, int toX, int toY)
        {
            int startX = Math.Min(fromX, toX);
            int startY = Math.Min(fromY, toY);
            int endX = Math.Max(fromX, toX);
            int endY = Math.Max(fromY, toY);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    field[x, y] = 1;
                }
            }
        }
    }
}
