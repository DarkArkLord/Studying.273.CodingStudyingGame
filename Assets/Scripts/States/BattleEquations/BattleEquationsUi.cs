﻿using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.BattleEquations.Common;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.States.BattleEquations
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

        public void OnInit()
        {
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
                var isCorrect = equations.Select(e => e.CheckCorection()).Aggregate((a, b) => a && b);
                Root.Data.Battle.IsPlayerWin = isCorrect;
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

        public void OnClose()
        {
            checkButton.OnClick.RemoveAllListeners();
            foreach (var equation in equations)
            {
                equationPool.FreeObject(equation.gameObject);
            }
        }
    }
}
