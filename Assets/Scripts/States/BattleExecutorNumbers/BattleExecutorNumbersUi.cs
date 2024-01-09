using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.BattleExecutorNumbers.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.States.BattleExecutorNumbers
{
    public class BattleExecutorNumbersUi : BaseBattleUiComponent
    {
        //[SerializeField]
        //private ButtonComponent _winButton;
        //public ButtonComponent WinButton => _winButton;

        [SerializeField]
        private ObjectPoolComponent codeElementsPool;
        [SerializeField]
        private BENCodeElement codeElementsPrefab;
        [SerializeField]
        private GameObject codeElementsParent;
        [SerializeField]
        private BEN_CodeType_Selector codeElementsTypeSelector;

        private BENCodeElement codeElementsTree;

        public void OnInit()
        {
            codeElementsTypeSelector.OnInit();
            codeElementsPool.Init(codeElementsPrefab.gameObject, 10);

            var obj = codeElementsPool.GetObject();
            codeElementsTree = obj.GetComponent<BENCodeElement>();

            codeElementsTree.ListNextNode = codeElementsTree.ListPrevNode = null;
            codeElementsTree.transform.SetParent(codeElementsParent.transform);
            codeElementsTree.InitButtons(codeElementsParent, codeElementsPool, () =>
            {
                while (codeElementsTree.ListPrevNode != null)
                {
                    codeElementsTree = codeElementsTree.ListPrevNode;
                }
            });

            codeElementsTree.gameObject.SetActive(true);
        }

        public void OnClose()
        {
            while (codeElementsTree.ListPrevNode != null)
            {
                codeElementsTree = codeElementsTree.ListPrevNode;
            }

            while (codeElementsTree != null)
            {
                var current = codeElementsTree;
                codeElementsTree = codeElementsTree.ListNextNode;

                current.OnElementDestory();
                current.transform.SetParent(codeElementsPool.transform);
                codeElementsPool.FreeObject(current.gameObject);
            }

            codeElementsTypeSelector.OnClose();
        }
    }
}
