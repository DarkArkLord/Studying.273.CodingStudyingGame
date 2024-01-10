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
            ConfigTask();
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

        private void ConfigTask()
        {
            // Task 0
            taskText.text = "Задание: вывести поступающие на вход цифры.";
            var input = new int[] { 1, };
            executionContext.OnInit(input, input);

            // Task 1
            //taskText.text = "Задание: вывести поступающие на вход цифры.";
            //var input = new int[] { 1, 2, 3 };
            //executionContext.OnInit(input, input);

            // Task 2
            //taskText.text = "Задание: считать попарно числа А и Б, вывести их сумму, разность, произведение, частное и остаток.";
            //var input = new int[] { 1, 2, 6, 3, 7, 3 };
            //var output = new int[] { 3, -1, 2, 0, 1, 9, 3, 18, 2, 0, 10, 4, 21, 2, 1 };
            //executionContext.OnInit(input, output);

            // Task 3
            //taskText.text = "Задание: считать попарно числа А и Б, вывести их сумму, разность и произведение.";
            //var input = new int[] { 1, 2, 6, 3, 7, 3 };
            //var output = new int[] { 3, -1, 2, 9, 3, 18, 10, 4, 21 };
            //executionContext.OnInit(input, output);
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
            for (int i = 0; i < executionContext.Memory.Length; i++)
            {
                memInfoColumn.AddItem("");
            }
            UpdateMemInfo();
        }

        private void UpdateMemInfo()
        {
            for (int i = 0; i < executionContext.Memory.Length; i++)
            {
                memInfoColumn.InfoElements[i].Text.text = $"M{i}={executionContext.Memory[i]}";
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

            inputInfoColumn.OnClose();
            outputInfoColumn.OnClose();
            memInfoColumn.OnClose();
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

            Root.Data.Battle.IsPlayerWin = false;

            yield return null;

            var currentElement = codeElementsTree;
            while (true)
            {
                if (currentElement == null)
                {
                    if (!executionContext.NextInputExists() && !executionContext.NextOutputExists())
                    {
                        break;
                    }
                    else
                    {
                        currentElement = codeElementsTree;
                    }
                }

                currentElement.ElemBackground.color = Color.yellow;

                var isinInstrucionsLimit = executionContext.NextInstruction();
                if (!isinInstrucionsLimit)
                {
                    currentElement.ElemBackground.color = Color.red;
                    ShowResultUi("Неверно\nПревышено допустимое количество операций");
                    yield break;
                }

                var correctExecution = ExecuteElement(currentElement);
                if (!correctExecution)
                {
                    currentElement.ElemBackground.color = Color.red;
                    yield break;
                }

                yield return new WaitForSeconds(0.3f);

                currentElement.ResetBackGroundColor();
                currentElement = currentElement.ListNextNode;
            }

            Root.Data.Battle.IsPlayerWin = true;
            ShowResultUi("Верно");
        }

        private bool ExecuteElement(BENCodeElement element)
        {
            if (element.CodeElementType == BEN_CodeElementType.IO_ReadInput)
            {
                if (!executionContext.NextInputExists())
                {
                    ShowResultUi("Неверно\nСчитывание из пустого ввода");
                    return false;
                }

                inputInfoColumn.InfoElements[executionContext.NextInputIndex].Text.color = Color.green;
                var inputValue = executionContext.NextInput();

                var memIndex = element.Interface_1.DropDown.value;
                if (memIndex < 0 || memIndex >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex}");
                    return false;
                }

                executionContext.Memory[memIndex] = inputValue;
                UpdateMemInfo();

                return true;
            }
            else if (element.CodeElementType == BEN_CodeElementType.IO_WriteOutput)
            {
                var memIndex = element.Interface_1.DropDown.value;
                if (memIndex < 0 || memIndex >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex}");
                    return false;
                }

                var value = executionContext.Memory[memIndex];

                if (!executionContext.NextOutputExists())
                {
                    var info = outputInfoColumn.AddItem($"{value}=?");
                    info.Text.color = Color.red;
                    ShowResultUi("Неверно\nЛишний вывод");
                    return false;
                }

                var infoElement = outputInfoColumn.InfoElements[executionContext.NextOutputIndex];
                infoElement.Text.text = $"{value}={executionContext.Outputs[executionContext.NextOutputIndex]}";

                if (!executionContext.CheckUserOutput(value))
                {
                    infoElement.Text.color = Color.red;
                    ShowResultUi("Неверно\nНеверный вывод");
                    return false;
                }
                else
                {
                    infoElement.Text.color = Color.green;
                }

                executionContext.ResetInstructions();
                return true;
            }
            else if (element.CodeElementType == BEN_CodeElementType.IO_SetValue)
            {
                var memIndex = element.Interface_2.DropDown.value;
                if (memIndex < 0 || memIndex >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex}");
                    return false;
                }

                var inputText = element.Interface_2.Input.text;
                if (inputText == null || inputText.Length < 1 || !int.TryParse(inputText, out int inputValue))
                {
                    ShowResultUi("Неверно\nВведено некорректное число");
                    return false;
                }

                executionContext.Memory[memIndex] = inputValue;
                UpdateMemInfo();
                return true;
            }
            else if (element.CodeElementType == BEN_CodeElementType.Numeric_Add)
            {
                var memIndex_a = element.Interface_3.DropDownA.value;
                if (memIndex_a < 0 || memIndex_a >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_a}");
                    return false;
                }

                var memIndex_b = element.Interface_3.DropDownB.value;
                if (memIndex_b < 0 || memIndex_b >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_b}");
                    return false;
                }

                var memIndex_res = element.Interface_3.DropDownRes.value;
                if (memIndex_res < 0 || memIndex_res >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_res}");
                    return false;
                }

                var valueA = executionContext.Memory[memIndex_a];
                var valueB = executionContext.Memory[memIndex_b];
                var res = valueA + valueB;
                executionContext.Memory[memIndex_res] = res;
                UpdateMemInfo();
                return true;
            }
            else if (element.CodeElementType == BEN_CodeElementType.Numeric_Sub)
            {
                var memIndex_a = element.Interface_3.DropDownA.value;
                if (memIndex_a < 0 || memIndex_a >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_a}");
                    return false;
                }

                var memIndex_b = element.Interface_3.DropDownB.value;
                if (memIndex_b < 0 || memIndex_b >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_b}");
                    return false;
                }

                var memIndex_res = element.Interface_3.DropDownRes.value;
                if (memIndex_res < 0 || memIndex_res >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_res}");
                    return false;
                }

                var valueA = executionContext.Memory[memIndex_a];
                var valueB = executionContext.Memory[memIndex_b];
                var res = valueA - valueB;
                executionContext.Memory[memIndex_res] = res;
                UpdateMemInfo();
                return true;
            }
            else if (element.CodeElementType == BEN_CodeElementType.Numeric_Mult)
            {
                var memIndex_a = element.Interface_3.DropDownA.value;
                if (memIndex_a < 0 || memIndex_a >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_a}");
                    return false;
                }

                var memIndex_b = element.Interface_3.DropDownB.value;
                if (memIndex_b < 0 || memIndex_b >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_b}");
                    return false;
                }

                var memIndex_res = element.Interface_3.DropDownRes.value;
                if (memIndex_res < 0 || memIndex_res >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_res}");
                    return false;
                }

                var valueA = executionContext.Memory[memIndex_a];
                var valueB = executionContext.Memory[memIndex_b];
                var res = valueA * valueB;
                executionContext.Memory[memIndex_res] = res;
                UpdateMemInfo();
                return true;
            }
            else if (element.CodeElementType == BEN_CodeElementType.Numeric_Div)
            {
                var memIndex_a = element.Interface_3.DropDownA.value;
                if (memIndex_a < 0 || memIndex_a >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_a}");
                    return false;
                }

                var memIndex_b = element.Interface_3.DropDownB.value;
                if (memIndex_b < 0 || memIndex_b >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_b}");
                    return false;
                }

                var memIndex_res = element.Interface_3.DropDownRes.value;
                if (memIndex_res < 0 || memIndex_res >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_res}");
                    return false;
                }

                var valueA = executionContext.Memory[memIndex_a];
                var valueB = executionContext.Memory[memIndex_b];

                if (valueB == 0)
                {
                    ShowResultUi("Неверно\nНельзя делить на ноль");
                    return false;
                }

                var res = valueA / valueB;
                executionContext.Memory[memIndex_res] = res;
                UpdateMemInfo();
                return true;
            }
            else if (element.CodeElementType == BEN_CodeElementType.Numeric_Mod)
            {
                var memIndex_a = element.Interface_3.DropDownA.value;
                if (memIndex_a < 0 || memIndex_a >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_a}");
                    return false;
                }

                var memIndex_b = element.Interface_3.DropDownB.value;
                if (memIndex_b < 0 || memIndex_b >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_b}");
                    return false;
                }

                var memIndex_res = element.Interface_3.DropDownRes.value;
                if (memIndex_res < 0 || memIndex_res >= executionContext.Memory.Length)
                {
                    ShowResultUi($"Неверно\nНекорректная ячейка памяти M{memIndex_res}");
                    return false;
                }

                var valueA = executionContext.Memory[memIndex_a];
                var valueB = executionContext.Memory[memIndex_b];

                if (valueB == 0)
                {
                    ShowResultUi("Неверно\nНельзя делить на ноль");
                    return false;
                }

                var res = valueA % valueB;
                executionContext.Memory[memIndex_res] = res;
                UpdateMemInfo();
                return true;
            }

            ShowResultUi($"Неверно\nНекорректная команда {element.CodeElementType}");
            return false;
        }
    }
}
