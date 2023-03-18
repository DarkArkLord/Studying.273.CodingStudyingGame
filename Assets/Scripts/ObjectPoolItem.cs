using UnityEngine;

namespace Assets.Scripts
{
    internal class ObjectPoolItem
    {
        public bool IsFree { get; set; }
        public GameObject Item { get; set; }

        public ObjectPoolItem(GameObject item)
        {
            IsFree = true;
            Item = item;
        }
    }
}
