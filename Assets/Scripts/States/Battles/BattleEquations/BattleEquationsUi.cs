using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Battles.BattleEquations.Common;
using Assets.Scripts.StatesMachine;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquations
{
    public class BattleEquationsUi : BaseBattleUiComponent
    {
        [SerializeField]
        private ButtonComponent checkButton;
        [SerializeField]
        private BEEquationComponent equationPrefab;
        [SerializeField]
        private ObjectPoolComponent equationPool;

        private const int equationsCount = 3;
        private BEEquationComponent[] equations = new BEEquationComponent[equationsCount];

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            equationPool.SetPrefab(equationPrefab.gameObject);
            equationPool.Init(equationsCount);

            var verticalOffset = 110f;
            var localScale = new Vector3(1f, 1f, 1f);

            for (int i = 0; i < equationsCount; i++)
            {
                var obj = equationPool.GetObject();
                var y = i % 3 - 1;
                obj.transform.localPosition = new Vector3(0, y * verticalOffset, 0);
                obj.transform.localScale = localScale;

                var equation = equations[i] = obj.GetComponent<BEEquationComponent>();
                equation.GenerateTask();
                obj.SetActive(true);
            }

            checkButton.OnClick.AddListener(() =>
            {
                setAccumulateTimeFlag(false);

                var isCorrect = equations.Select(e => e.CheckCorection()).Aggregate((a, b) => a && b);
                Root.Data.BattleResult.IsPlayerWin = isCorrect;
                if (isCorrect)
                {
                    ShowResultUi("Верно");
                }
                else
                {
                    ShowResultUi("Неверно");
                }
            });
        }

        public override void OnClose()
        {
            checkButton.OnClick.RemoveAllListeners();
            foreach (var equation in equations)
            {
                equationPool.FreeObject(equation.gameObject);
            }
        }
    }
}
