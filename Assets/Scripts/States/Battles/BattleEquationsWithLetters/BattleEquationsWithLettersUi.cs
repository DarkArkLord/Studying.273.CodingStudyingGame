using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Battles.BattleEquationsWithLetters.Common;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquationsWithLetters
{
    public class BattleEquationsWithLettersUi : BaseBattleUiComponent
    {
        [SerializeField]
        private ButtonComponent checkButton;

        [SerializeField]
        private ObjectPoolComponent equationsPool;
        [SerializeField]
        private ObjectPoolComponent inputPool;

        private EquationsListConfig equationsListConfig;
        private BEWLEquationComponent[] equations;
        private BEWLInputComponent[] inputs;

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            var equationsInitCount = 3;
            equationsListConfig = EquationsGenerator.GenerateDisjointEquations(RandomUtils.Random, equationsInitCount);

            var equationsCount = equationsListConfig.Equations.Length;
            equationsPool.Init(equationsCount);
            equations = new BEWLEquationComponent[equationsCount];
            for (int i = 0; i < equationsCount; i++)
            {
                var obj = equationsPool.GetObject();
                var equation = equations[i] = obj.GetComponent<BEWLEquationComponent>();

                equation.SetEquation(equationsListConfig.Equations[i]);

                obj.SetActive(true);
            }

            var variablesCount = equationsListConfig.Variables.Length;
            inputPool.Init(variablesCount);
            inputs = new BEWLInputComponent[variablesCount];
            for (int i = 0; i < variablesCount; i++)
            {
                var obj = inputPool.GetObject();
                var input = inputs[i] = obj.GetComponent<BEWLInputComponent>();

                input.SetVariableOperator(equationsListConfig.Variables[i]);
                input.SetReadonly(false);

                obj.SetActive(true);
            }

            checkButton.OnClick.AddListener(OnCheckButtonClick);
        }

        private void OnCheckButtonClick()
        {
            foreach (var input in inputs)
            {
                input.SetReadonly(true);
            }

            Root.Data.BattleResult.IsPlayerWin = false;
            var isInputsCorrect = inputs.Select(e => e.TryReadInput()).Aggregate((a, b) => a && b);

            if (!isInputsCorrect)
            {
                ShowResultUi("Неверно");
                return;
            }

            var isEquationsCorrect = equations.Select(e => e.CheckCorrection()).Aggregate((a, b) => a && b);
            Root.Data.BattleResult.IsPlayerWin = isEquationsCorrect;
            if (isEquationsCorrect)
            {
                ShowResultUi("Верно");
            }
            else
            {
                ShowResultUi("Неверно");
            }
        }

        public override void OnClose()
        {
            base.OnClose();

            checkButton.OnClick.RemoveAllListeners();

            equationsPool.FreeAllObjects();
            inputPool.FreeAllObjects();

            equationsListConfig = null;
            equations = null;
            inputs = null;
        }
    }
}
