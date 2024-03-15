using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States.Battles.BattleEquations.Common
{
    public enum OperatorType
    {
        Value = 0,
        Plus = 1,
        Minus = 2,
        Mult = 3,
    }

    public abstract class BaseOperator
    {
        public abstract int Result { get; }
        public abstract OperatorType OperType { get; }
    }

    public class ValueOperator : BaseOperator
    {
        private int value;
        public ValueOperator(int value) { this.value = value; }
        public override int Result => value;
        public override OperatorType OperType => OperatorType.Value;
        public override string ToString() => value.ToString();
    }

    public class PlusOperator : BaseOperator
    {
        private BaseOperator a, b;
        public PlusOperator(BaseOperator a, BaseOperator b) { this.a = a; this.b = b; }
        public override int Result => a.Result + b.Result;
        public override OperatorType OperType => OperatorType.Plus;
        public override string ToString() => $"{a} + {b}";
    }

    public class MinusOperator : BaseOperator
    {
        private BaseOperator a, b;
        public MinusOperator(BaseOperator a, BaseOperator b) { this.a = a; this.b = b; }
        public override int Result => a.Result - b.Result;
        public override OperatorType OperType => OperatorType.Minus;
        public override string ToString() => $"{a} - {b}";
    }

    public class MultOperator : BaseOperator
    {
        private BaseOperator a, b;
        public MultOperator(BaseOperator a, BaseOperator b) { this.a = a; this.b = b; }
        public override int Result => a.Result * b.Result;
        public override OperatorType OperType => OperatorType.Mult;
        public override string ToString() => $"{a} * {b}";
    }

    public static class OperationsParser
    {
        public static BaseOperator Parse(int[] values, OperatorType[] operators)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (operators == null) throw new ArgumentNullException(nameof(operators));
            if (values.Length < 1) throw new ArgumentException($"Values has no values");
            if (operators.Length < 1) throw new ArgumentException($"Operators has no values");
            if (values.Length != operators.Length + 1) throw new ArgumentException($"values.Length != operators.Length + 1");
            if (values.Length == 1) return new ValueOperator(values[0]);

            BaseOperator[] tempValues = values.Select(x => new ValueOperator(x)).ToArray();
            tempValues = ParseHighPriorityOperators(tempValues, operators).ToArray();
            var tempOperators = operators.Where(o => o != OperatorType.Mult).ToArray();
            return BuildTree(tempValues, tempOperators);
        }

        private static IEnumerable<BaseOperator> ParseHighPriorityOperators(BaseOperator[] values, OperatorType[] operators)
        {
            var prevValue = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (operators[i - 1] == OperatorType.Mult)
                {
                    var mult = new MultOperator(prevValue, values[i]);
                    prevValue = mult;
                }
                else
                {
                    yield return prevValue;
                    prevValue = values[i];
                }
            }

            yield return prevValue;
        }

        private static BaseOperator BuildTree(BaseOperator[] values, OperatorType[] operators)
        {
            if (values.Length == 1) return values[0];

            var tree = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                switch (operators[i - 1])
                {
                    case OperatorType.Plus:
                        tree = new PlusOperator(tree, values[i]);
                        break;
                    case OperatorType.Minus:
                        tree = new MinusOperator(tree, values[i]);
                        break;
                    default:
                        throw new ArgumentException($"{operators[i - 1]} not + or -");
                }
            }

            return tree;
        }
    }
}
