using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.TextureGenerators
{
    [CreateAssetMenu(fileName = "New ColorsConfig", menuName = "Configs/Colors Config", order = 50)]
    public class ColorsConfigSO : ScriptableObject
    {
        [SerializeField]
        private Color[] colors;
        public IReadOnlyList<Color> Colors => colors;
    }
}
