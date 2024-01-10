using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CommonComponents
{
    public class ObjectPoolComponent : MonoBehaviour
    {
        private class ObjectPoolItem
        {
            public bool IsFree { get; set; }
            public GameObject Item { get; set; }

            public ObjectPoolItem(GameObject item)
            {
                IsFree = true;
                Item = item;
            }
        }

        public bool IsInited { get; private set; } = false;

        private GameObject _prefab;
        private int _startCount;

        private List<ObjectPoolItem> items;

        public void Init(GameObject prefab, int startCount = 10)
        {
            _prefab = prefab;
            _startCount = startCount;

            items = new List<ObjectPoolItem>(_startCount);
            for (int i = 0; i < _startCount; i++)
            {
                AddItem();
            }

            IsInited = true;
        } 

        private ObjectPoolItem AddItem()
        {
            var obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
            obj.transform.SetParent(transform);

            var item = new ObjectPoolItem(obj);
            items.Add(item);
            return item;
        }

        public IEnumerable<GameObject> GetObjects(int count)
        {
            var findedCount = 0;
            foreach (var obj in items)
            {
                if (obj.IsFree)
                {
                    obj.IsFree = false;
                    findedCount++;
                    yield return obj.Item;

                    if (findedCount >= count)
                    {
                        yield break;
                    }
                }
            }

            if (findedCount < count)
            {
                for (int i = findedCount; i < count; i++)
                {
                    var item = AddItem();
                    item.IsFree = false;
                    yield return item.Item;
                }
            }
        }

        public GameObject GetObject()
        {
            foreach (var obj in items)
            {
                if (obj.IsFree)
                {
                    obj.IsFree = false;
                    return obj.Item;
                }
            }

            var item = AddItem();
            item.IsFree = false;
            return item.Item;
        }

        public void FreeObjects(IEnumerable<GameObject> objects)
        {
            foreach (var obj in objects)
            {
                FreeObject(obj);
            }
        }

        public void FreeObject(GameObject obj)
        {
            var id = obj.GetInstanceID();
            var item = items.Find(x => x.Item.GetInstanceID() == id);
            if (item != null)
            {
                item.IsFree = true;
                item.Item.SetActive(false);
            }
            else
            {
                Debug.LogError($"No object with id {id} for freeing");
            }
        }
    }
}