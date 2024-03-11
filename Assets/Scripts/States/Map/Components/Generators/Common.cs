using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States.Map.Components.Generators
{
    public static class Common
    {
        //
    }

    public class MapPathConfig
    {
        public MapCellContent[,] Map { get; set; }
        public Vector2Int InputPosition { get; set; }
        public Vector2Int OutputPosition { get; set; }
    }

    public enum MapCellContent
    {
        Input = 0,
        Path = 1,
        Wall = 2,
        Output = 3,
    }
}
