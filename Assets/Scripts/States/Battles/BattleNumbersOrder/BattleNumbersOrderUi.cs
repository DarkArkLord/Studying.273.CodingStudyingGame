﻿using Assets.Scripts.CommonComponents;
using Assets.Scripts.DataKeeper.Progress;
using Assets.Scripts.States.Battles.BattleNumbersOrder.Common;
using Assets.Scripts.StatesMachine;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleNumbersOrder
{
    public class BattleNumbersOrderUi : BaseBattleUiComponent
    {
        [SerializeField]
        private BNOButtonComponent _buttonPrefab;
        [SerializeField]
        private ObjectPoolComponent _buttonsPool;

        [SerializeField]
        private TMP_Text _battleHeader;
        private Func<int, int, bool> _valuesComparator;

        private const int buttonsCount = 9;
        private BNOButtonComponent[] buttons = new BNOButtonComponent[buttonsCount];
        private Stack<BNOButtonComponent> pressedButtonsStack = new Stack<BNOButtonComponent>();

        private System.Random _random = RandomUtils.Random;

        public override void OnInit(Action<bool> setAccumulateTimeFlag, MainStateCode currentState)
        {
            base.OnInit(setAccumulateTimeFlag, currentState);

            // variants
            if (_random.Next() % 2 == 0)
            {
                _battleHeader.text = "Выберите числа в порядке возрастания";
                _valuesComparator = (a, b) => a < b;
            }
            else
            {
                _battleHeader.text = "Выберите числа в порядке убывания";
                _valuesComparator = (a, b) => a > b;
            }

            _buttonsPool.Init(buttonsCount);

            var buttonLocalScale = new Vector3(1f, 1f, 1f);
            var buttonOffset = 215f;
            var verticalOffset = 50f;

            var maxOffset = GetMaxOffsetByDifficulty();

            for (int i = 0, buttonValue = _random.Next(1, maxOffset);
                i < buttonsCount;
                i++, buttonValue += _random.Next(1, maxOffset))
            {
                var obj = _buttonsPool.GetObject();
                var x = i / 3 - 1;
                var y = i % 3 - 1;
                obj.transform.localPosition = new Vector3(x * buttonOffset, y * buttonOffset - verticalOffset, 0);
                obj.transform.localScale = buttonLocalScale;

                var button = buttons[i] = obj.GetComponent<BNOButtonComponent>();
                button.SetValue(buttonValue);
                SetDefaultButtonIndexAndCollors(button);

                obj.SetActive(true);

                button.OnClick.AddListener(() =>
                {
                    if (button.Index < 0)
                    {
                        pressedButtonsStack.Push(button);
                        button.SetIndex(pressedButtonsStack.Count);
                        button.SetValueColor(Color.yellow);
                        button.SetIndexColor(Color.yellow);
                    }
                    else if (button.Index == pressedButtonsStack.Count)
                    {
                        SetDefaultButtonIndexAndCollors(button);
                        pressedButtonsStack.Pop();
                    }

                    if (pressedButtonsStack.Count == buttonsCount)
                    {
                        CheckCorrectOrder();
                    }
                });
            }

            MixValuesOnButtons();
        }

        private int GetMaxOffsetByDifficulty()
        {
            if (DifficultyLevel == BattleDifficultyLevel.Easy)
            {
                return 3;
            }

            if (DifficultyLevel == BattleDifficultyLevel.Hard)
            {
                return 10;
            }

            return 5;
        }

        private void SetDefaultButtonIndexAndCollors(BNOButtonComponent button)
        {
            button.SetValueColor(Color.white);
            button.SetIndex(-1);
            button.SetIndexColor(Color.gray);
        }

        private void CheckCorrectOrder()
        {
            setAccumulateTimeFlag(false);

            var buttons = pressedButtonsStack.Reverse().ToArray();
            var isCorrect = true;

            for (int i = 1; i < buttons.Length; i++)
            {
                var a = buttons[i - 1];
                var b = buttons[i];

                if (_valuesComparator.Invoke(a.Value, b.Value))
                {
                    if (isCorrect)
                    {
                        b.SetIndexColor(Color.green);
                        a.SetIndexColor(Color.green);
                    }
                }
                else
                {
                    isCorrect = false;
                    a.SetIndexColor(Color.red);
                    b.SetIndexColor(Color.red);
                }
            }

            Root.Data.BattleResult.IsPlayerWin = isCorrect;
            if (isCorrect)
            {
                ShowResultUi("Верно");
            }
            else
            {
                ShowResultUi("Неверно");
            }
        }

        private void MixValuesOnButtons()
        {
            for (int i = 0; i < buttonsCount * buttonsCount; i++)
            {
                var ai = _random.Next(buttonsCount);
                var bi = _random.Next(buttonsCount);

                var temp = buttons[ai].Value;
                buttons[ai].SetValue(buttons[bi].Value);
                buttons[bi].SetValue(temp);
            }
        }

        public override void OnClose()
        {
            pressedButtonsStack.Clear();
            foreach (var button in buttons)
            {
                button.OnClick.RemoveAllListeners();
                _buttonsPool.FreeObject(button.gameObject);
            }
        }
    }
}
