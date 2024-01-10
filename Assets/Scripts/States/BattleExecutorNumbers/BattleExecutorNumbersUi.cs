using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.BattleExecutorNumbers.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        private BEN_ExecutionContext executionContext = new BEN_ExecutionContext();

        [SerializeField]
        private BEN_Info_Column inputInfoColumn;
        [SerializeField]
        private BEN_Info_Column outputInfoColumn;
        [SerializeField]
        private BEN_Info_Column memInfoColumn;

        [SerializeField]
        private TMP_Text taskText;
        [SerializeField]
        private ButtonComponent runButton;

        public void OnInit()
        {
            // Task configure
            ConfigTask1();
            // End task configure

            InitInfoColumns();

            codeElementsTypeSelector.OnInit();
            InitCodeTree();

            runButton.OnClick.AddListener(() =>
            {
                StartCoroutine(RunExecuteCorutine());
            });
            runButton.SetButtonActive(true);
        }

        private void ConfigTask1()
        {
            taskText.text = "Просто выведи эти цифры, пожалуйста...";
            var input = new int[] { 1, };
            executionContext.OnInit(input, input);
        }

        private void InitInfoColumns()
        {
            inputInfoColumn.OnInit();
            foreach (var input in executionContext.Inputs)
            {
                inputInfoColumn.AddItem(input.ToString());
            }

            outputInfoColumn.OnInit();
            foreach (var output in executionContext.Outputs)
            {
                outputInfoColumn.AddItem($"?={output}");
            }

            memInfoColumn.OnInit();
            UpdateMemInfo();
        }

        private void UpdateMemInfo()
        {
            for (int i = 0; i < executionContext.Memory.Length; i++)
            {
                memInfoColumn.AddItem($"M{i}={executionContext.Memory[i]}");
            }
        }

        private void InitCodeTree()
        {
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
            element.InitType(type, executionContext);
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

        private IEnumerator RunExecuteCorutine()
        {
            runButton.SetButtonActive(false);
            codeElementsTypeSelector.SetButtonsActive(false);
            MoveTreeToRoot();
            for (var node = codeElementsTree; node != null; node = node.ListNextNode)
            {
                node.SetElementsActive(false);
            }

            yield return null;

            ShowResultUi("123\n321");
        }

        public void OnClose()
        {
            runButton.OnClick.RemoveAllListeners();

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
