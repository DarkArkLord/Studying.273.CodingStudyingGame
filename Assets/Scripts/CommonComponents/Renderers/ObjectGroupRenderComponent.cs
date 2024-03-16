using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CommonComponents.Renderers
{
    public class ObjectGroupRenderComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] objects;

        private IEnumerable<Renderer> GetRenderers()
        {
            foreach (var obj in objects)
            {
                var rendererComponent = obj.GetComponent<Renderer>();
                if (rendererComponent is null)
                {
                    rendererComponent = obj.AddComponent<Renderer>();
                }

                yield return rendererComponent;
            }
        }

        public void SetMaterial(Material material)
        {
            foreach (var renderer in GetRenderers())
            {
                renderer.material = material;
            }
        }

        public void SetTexture(Texture texture)
        {
            foreach (var renderer in GetRenderers())
            {
                renderer.material.mainTexture = texture;
            }
        }

        public void SetColor(Color color)
        {
            foreach (var renderer in GetRenderers())
            {
                renderer.material.color = color;
            }
        }
    }
}
