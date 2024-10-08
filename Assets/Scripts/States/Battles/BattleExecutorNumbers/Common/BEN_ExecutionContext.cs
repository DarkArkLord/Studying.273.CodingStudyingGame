﻿using System.Collections.Generic;
using TMPro;

namespace Assets.Scripts.States.Battles.BattleExecutorNumbers.Common
{
    public class BEN_ExecutionContext
    {
        public const int MemoryCellsCount = 10;
        public int[] Memory { get; private set; } = new int[MemoryCellsCount];
        private TMP_Dropdown.OptionData[] memoryOptions = new TMP_Dropdown.OptionData[MemoryCellsCount];

        public int[] Inputs { get; private set; }
        public int NextInputIndex { get; private set; }

        public int[] Outputs { get; private set; }
        public int NextOutputIndex { get; private set; }

        public const int MaxInstructionBeforeOutput = 1000;
        public int CurrentInstruction { get; private set; }

        public void OnInit(int[] inputs, int[] outputs)
        {
            for (int i = 0; i < MemoryCellsCount; i++)
            {
                Memory[i] = 0;
                memoryOptions[i] = new TMP_Dropdown.OptionData { text = $"M{i}" };
            }

            Inputs = inputs;
            NextInputIndex = 0;

            Outputs = outputs;
            NextOutputIndex = 0;

            CurrentInstruction = 0;
        }

        public bool NextInputExists() => NextInputIndex < Inputs.Length;

        public int NextInput() => Inputs[NextInputIndex++];

        public bool NextOutputExists() => NextOutputIndex < Outputs.Length;

        public bool CheckUserOutput(int output)
        {
            var isEqual = Outputs[NextOutputIndex] == output;
            NextOutputIndex++;
            return isEqual;
        }

        public bool NextInstruction() => ++CurrentInstruction < MaxInstructionBeforeOutput;

        public void ResetInstructions() => CurrentInstruction = 0;

        public void SetMemoryDropDownOptions(List<TMP_Dropdown.OptionData> options)
        {
            options.AddRange(memoryOptions);
        }
    }
}
