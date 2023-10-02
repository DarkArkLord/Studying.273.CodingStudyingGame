using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IObjectPool
    {
        void Init(GameObject prefab, int startCount = 10);
        IEnumerable<GameObject> GetObjects(int count);
        GameObject GetObject();
        void FreeObjects(IEnumerable<GameObject> objects);
        void FreeObject(GameObject obj);
    }
}
