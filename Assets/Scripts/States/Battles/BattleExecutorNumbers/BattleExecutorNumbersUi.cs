using Assets.Scripts.CommonComponents;
using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.States.Battles.BattleExecutorNumbers.Common;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleExecutorNumbers
{
    public class BattleExecutorNumbersUi : BaseBattleUiComponent
    {
        [SerializeField]
        private ObjectPoolComponent codeElementsPool;
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

        private System.Random random = RandomUtils.Random;

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            // Task configure
            ConfigTask();
            // End task configure

            InitInfoColumns();

            codeElementsTypeSelector.OnInit();
            InitCodeTree();

            runButton.OnClick.AddListener(() =>
            {
                setAccumulateTimeFlag(false);
                StartCoroutine(RunExecuteCorutine());
            });
            runButton.SetButtonActive(true);
        }

        private void ConfigTask()
        {
            var configsEazy = new Action[] { ConfigTask_Easy_0, ConfigTask_Easy_1, ConfigTask_Easy_2, ConfigTask_Easy_3, ConfigTask_Easy_4, ConfigTask_Easy_5 };
            var configsMeduim = new Action[] { ConfigTask_Medium_0, ConfigTask_Medium_1, ConfigTask_Medium_2, ConfigTask_Medium_3 };
            var configsHard = new Action[] { ConfigTask_Hard_0, ConfigTask_Hard_1, ConfigTask_Hard_2 };

            var configs = new List<Action>();

            if (DifficultyLevel == BattleDifficultyLevel.Easy)
            {
                configs.AddRange(configsEazy);
                configs.AddRange(configsEazy);
                configs.AddRange(configsEazy);
                configs.AddRange(configsMeduim);
            }
            else if (DifficultyLevel == BattleDifficultyLevel.Hard)
            {
                configs.AddRange(configsMeduim);
                configs.AddRange(configsHard);
                configs.AddRange(configsHard);
                configs.AddRange(configsHard);
            }
            else
            {
                configs.AddRange(configsEazy);
                configs.AddRange(configsMeduim);
                configs.AddRange(configsMeduim);
                configs.AddRange(configsMeduim);
                configs.AddRange(configsHard);
            }

            var config = configs[random.Next(configs.Count)];
            config.Invoke();
        }

        private void ConfigTask_Easy_0()
        {
            taskText.text = "Задание: вывести поступающие на вход цифры.";
            var input = new int[] { 1, 2, 3, 4, 5, };
            executionContext.OnInit(input, input);
        }

        private void ConfigTask_Easy_1()
        {
            taskText.text = "Задание: считать попарно числа А и Б, вывести их сумму.";
            var input = new int[] { 1, 2, 6, 3, 7, 3 };
            var output = new int[] { 3, 9, 10 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Easy_2()
        {
            taskText.text = "Задание: считать попарно числа А и Б, вывести их разность.";
            var input = new int[] { 1, 2, 6, 3, 7, 3 };
            var output = new int[] { -1, 3, 4 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Easy_3()
        {
            taskText.text = "Задание: считать попарно числа А и Б, вывести их произведение.";
            var input = new int[] { 1, 2, 6, 3, 7, 3 };
            var output = new int[] { 2, 18, 21 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Easy_4()
        {
            taskText.text = "Задание: считать число, вывести его удвоенное значение.";
            var input = new int[] { 1, 2, 3, 4, 5, 6 };
            var output = new int[] { 2, 4, 6, 8, 10, 12 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Easy_5()
        {
            taskText.text = "Задание: считать попарно числа А и Б, вывести их в обратном порядке (Б и А).";
            var input = new int[] { 1, 2, 6, 3, 7, 3 };
            var output = new int[] { 2, 18, 21 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Medium_0()
        {
            taskText.text = "Задание: считать попарно числа А и Б, вывести их сумму и разность.";
            var input = new int[] { 1, 2, 6, 3, 7, 3 };
            var output = new int[] { 3, -1, 9, 3, 10, 4 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Medium_1()
        {
            taskText.text = "Задание: считать попарно числа А и Б, вывести их произведение, частное и остаток.";
            var input = new int[] { 1, 2, 6, 3, 7, 3 };
            var output = new int[] { 2, 0, 1, 18, 2, 0, 21, 2, 1 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Medium_2()
        {
            taskText.text = "Задание: считать попарно числа А и Б, вывести их сумму, разность, произведение, частное и остаток.";
            var input = new int[] { 1, 2, 6, 3, 7, 3 };
            var output = new int[] { 3, -1, 2, 0, 1, 9, 3, 18, 2, 0, 10, 4, 21, 2, 1 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Medium_3()
        {
            taskText.text = "Задание: считать три числа, вывести их сумму.";
            var input = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var output = new int[] { 6, 15, 24 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Hard_0()
        {
            taskText.text = "Задание: считать числа А, Б и В, вывести значение выражения А - Б * В.";
            var input = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var output = new int[] { -5, -26, -65 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Hard_1()
        {
            taskText.text = "Задание: считать числа А и Б, вывести значение выражения (А^2 - Б^2)^2.";
            var input = new int[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 2, 3, 3 };
            var output = new int[] { 0, 9, 0, 25, 25, 0 };
            executionContext.OnInit(input, output);
        }

        private void ConfigTask_Hard_2()
        {
            taskText.text = "Задание: считать числа А, Б, В и Г, вывести значение выражения (А - Б) * (В - Г).";
            var input = new int[] { 1, 1, 1, 1, 1, 2, 3, 4, 4, 3, 2, 1, 4, 2, 3, 1 };
            var output = new int[] { 0, 1, 1, 4 };
            executionContext.OnInit(input, output);
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
            codeElementsPool.Init();

            var inputCodeElement = InitCodeElement(BEN_CodeElementType.IO_ReadInput);
            var outputCodeElement = InitCodeElement(BEN_CodeElementType.IO_WriteOutput);

            inputCodeElement.ListPrevNode = null;
            inputCodeElement.ListNextNode = outputCodeElement;
            inputCodeElement.SetButtonsActivity();

            outputCodeElement.ListPrevNode = inputCodeElement;
            outputCodeElement.ListNextNode = null;
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
            var t = 1000;
            while (codeElementsTree.ListPrevNode != null && t-- > 0)
            {
                codeElementsTree = codeElementsTree.ListPrevNode;
            }
        }

        public override void OnClose()
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

            Root.Data.BattleResult.IsPlayerWin = false;

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

            Root.Data.BattleResult.IsPlayerWin = true;
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
