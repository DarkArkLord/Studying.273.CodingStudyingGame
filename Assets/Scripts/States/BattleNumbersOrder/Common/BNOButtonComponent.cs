﻿using Assets.Scripts.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.States.BattleNumbersOrder.Common
{
    public class BNOButtonComponent : ButtonComponent
    {
        [SerializeField]
        private TMP_Text _valueText;
        //public TMP_Text ValueText => _valueText;

        [SerializeField]
        private TMP_Text _indexText;
        //public TMP_Text IndexText => _indexText;

        public int Value { get; private set; }
        public int Index { get; private set; }

        public void SetValue(int value)
        {
            Value = value;
            _valueText.text = value.ToString();
        }

        public void SetIndex(int index)
        {
            Index = index;
            _indexText.text = index.ToString();
        }

        public void SetValueColor(Color color)
        {
            _valueText.color = color;
        }

        public void SetIndexColor(Color color)
        {
            _indexText.color = color;
        }
    }
}