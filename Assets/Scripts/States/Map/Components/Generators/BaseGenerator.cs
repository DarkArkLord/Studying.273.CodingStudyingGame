using UnityEngine;

namespace Assets.Scripts.States.Map.Components.Generators
{
    public abstract class BaseGenerator : MonoBehaviour
    {
        public abstract MapPathConfig GenerateMap(int width, int height);
    }
}
