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

            var inputCodeElement = InitCodeElement(BEN_CodeElementType.IO_ReadInput);
            var outputCodeElement = InitCodeElement(BEN_CodeElementType.IO_WriteOutput);

            inputCodeElement.ListNextNode = outputCodeElement;
            inputCodeElement.SetButtonsActivity();
            outputCodeElement.ListPrevNode = inputCodeElement;
            outputCodeElement.SetButtonsActivity();
            codeElementsTree = inputCodeElement;
        }

        private BENCodeElement InitCodeElement(BEN_CodeElementType type)
        {
            var obj = codeElementsPool.GetObject();
            var element = obj.GetComponent<BENCodeElement>();

            element.ListNextNode = element.ListPrevNode = null;
            element.transform.SetParent(codeElementsParent.transform);
            element.InitType(type);
            element.InitButtons(codeElementsParent, codeElementsPool, MoveTreeToRoot, codeElementsTypeSelector);

            element.gameObject.SetActive(true);

            return element;
        }

        private void MoveTreeToRoot()
        {
            while (codeElementsTree.ListPrevNode != null)
            {
                codeElementsTree = codeElementsTree.ListPrevNode;
            }
        }

        public void OnClose()
        {
            MoveTreeToRoot();

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
