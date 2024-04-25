using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Battles.BattleEquationsWithLetters.Common;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquationsWithLetters
{
    public class BattleEquationsWithLettersUi : BaseBattleUiComponent
    {
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

                equation.EquationText.text = equationsListConfig.Equations[i].ToEqualString();
                equation.EquationText.color = Color.black;

                obj.SetActive(true);
            }

            var variablesCount = equationsListConfig.Variables.Length;
            inputPool.Init(variablesCount);
            inputs = new BEWLInputComponent[variablesCount];
            for (int i = 0; i < variablesCount; i++)
            {
                var obj = inputPool.GetObject();
                var input = inputs[i] = obj.GetComponent<BEWLInputComponent>();

                var name = equationsListConfig.Variables[i].Name;
                input.LeftSideText.text = $"{name} = ";
                input.InputField.text = string.Empty;

                obj.SetActive(true);
            }
        }

        public override void OnClose()
        {
            base.OnClose();

            equationsPool.FreeAllObjects();
            inputPool.FreeAllObjects();
        }
    }
}
