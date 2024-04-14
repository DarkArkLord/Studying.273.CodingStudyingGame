﻿using Assets.Scripts.CommonComponents;
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

            for (int i = 0; i < equationsCount; i++)
            {
                var obj = equationPool.GetObject();
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
            equationPool.FreeAllObjects();
        }
    }
}
