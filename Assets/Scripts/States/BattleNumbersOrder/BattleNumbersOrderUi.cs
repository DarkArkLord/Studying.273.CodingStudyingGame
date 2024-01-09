using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.BattleNumbersOrder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.States.BattleNumbersOrder
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

        public void OnInit()
        {
            // add variants
            _battleHeader.text = "Выберите числа в порядке возрастания";
            _valuesComparator = (a, b) => a < b;

            if (!_buttonsPool.IsInited)
            {
                _buttonsPool.Init(_buttonPrefab.gameObject, buttonsCount);
            }

            var buttonLocalScale = new Vector3(1f, 1f, 1f);
            var buttonOffset = 215f;
            var verticalOffset = 50f;

            for (int i = 0; i < buttonsCount; i++)
            {
                var obj = _buttonsPool.GetObject();
                var x = i / 3 - 1;
                var y = i % 3 - 1;
                obj.transform.localPosition = new Vector3(x * buttonOffset, y * buttonOffset - verticalOffset, 0);
                obj.transform.localScale = buttonLocalScale;

                var button = buttons[i] = obj.GetComponent<BNOButtonComponent>();
                button.SetValue(i + 1);
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
        }

        private void SetDefaultButtonIndexAndCollors(BNOButtonComponent button)
        {
            button.SetValueColor(Color.white);
            button.SetIndex(-1);
            button.SetIndexColor(Color.gray);
        }

        private void CheckCorrectOrder()
        {
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

            Root.Data.Battle.IsPlayerWin = isCorrect;
            if (isCorrect)
            {
                ShowResultUi("Верно");
            }
            else
            {
                ShowResultUi("Неверно");
            }
        }

        public void OnClose()
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
