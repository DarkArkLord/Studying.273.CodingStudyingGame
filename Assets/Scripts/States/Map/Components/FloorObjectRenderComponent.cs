using UnityEngine;

namespace Assets.Scripts.States.Map.Components
{
    public class FloorObjectRenderComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] objects;

        public void SetMaterial(Material material)
        {
            foreach (var obj in objects)
            {
                var rendererComponent = obj.GetComponent<Renderer>();
                if (rendererComponent is null)
                {
                    rendererComponent = obj.AddComponent<Renderer>();
                }

                rendererComponent.material = material;
            }
        }
    }
}
