using System;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.Renderers
{
    public class SomeObjectsRenderComponent : MonoBehaviour
    {
        [SerializeField]
        private ObjectGroupRenderComponent[] objects;

        public void SetMaterial(int index, Material material)
        {
            if (index < 0 || index >= objects.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            objects[index].SetMaterial(material);
        }

        public void SetTexture(int index, Texture texture)
        {
            if (index < 0 || index >= objects.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            objects[index].SetTexture(texture);
        }

        public void SetColor(int index, Color color)
        {
            if (index < 0 || index >= objects.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            objects[index].SetColor(color);
        }
    }
}
