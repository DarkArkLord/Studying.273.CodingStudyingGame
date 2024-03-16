using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.TextureGenerators
{
    public class ColorsConfigComponent : MonoBehaviour
    {
        [SerializeField]
        private Color[] colors;
        public IReadOnlyList<Color> Colors => colors;
    }
}
