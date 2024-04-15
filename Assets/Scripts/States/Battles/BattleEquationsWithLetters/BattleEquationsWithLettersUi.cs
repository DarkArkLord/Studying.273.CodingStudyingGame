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

        private BEWLEquationComponent[] equations;
        private BEWLInputComponent[] inputs;

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            var equationsCount = 3;
            equationsPool.Init(equationsCount);
            //var lettersCount = 3;
            //inputPool.Init(equationsCount);

            var equ = EquationsGenerator.GenerateDisjointEquations(RandomUtils.Random);

            var obj1 = equationsPool.GetObject();
            var equation1 = obj1.GetComponent<BEWLEquationComponent>();
            equation1.EquationText.text = equ.Equations[0].ToString();
            obj1.SetActive(true);

            foreach (var t in equ.Variables)
            {
                t.Value = null;
            }

            var obj2 = equationsPool.GetObject();
            var equation2 = obj2.GetComponent<BEWLEquationComponent>();
            equation2.EquationText.text = equ.Equations[0].ToString();
            obj2.SetActive(true);

            //for (int i = 0; i < equationsCount; i++)
            //{
            //    equationsPool.GetObject().SetActive(true);
            //    inputPool.GetObject().SetActive(true);
            //}
        }

        public override void OnClose()
        {
            base.OnClose();

            equationsPool.FreeAllObjects();
            inputPool.FreeAllObjects();
        }
    }
}
