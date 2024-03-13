using Assets.Scripts.CommonComponents;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public class BEN_Info_Column : MonoBehaviour
    {
        [SerializeField]
        private BEN_Info_Element infoElementPrefab;
        [SerializeField]
        private ObjectPoolComponent elementsPool;

        public List<BEN_Info_Element> InfoElements { get; private set; }

        public void OnInit()
        {
            elementsPool.SetPrefab(infoElementPrefab.gameObject);
            elementsPool.Init();
            InfoElements = new List<BEN_Info_Element>();
        }

        public void OnClose()
        {
            foreach (var element in InfoElements)
            {
                element.OnClose();
                elementsPool.FreeObject(element.gameObject);
            }
            InfoElements.Clear();
        }

        public BEN_Info_Element AddItem(string text)
        {
            var obj = elementsPool.GetObject();
            var element = obj.GetComponent<BEN_Info_Element>();

            element.Text.text = text;
            element.OnInit();
            element.gameObject.SetActive(true);

            InfoElements.Add(element);

            return element;
        }
    }
}
