using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseModel : MonoBehaviour
    {
        public Root Root => Root.Instance;
    }
}
