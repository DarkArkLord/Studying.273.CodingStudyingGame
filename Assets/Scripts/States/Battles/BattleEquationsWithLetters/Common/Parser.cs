using Assets.Scripts.States.Battles.BattleEquations.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assets.Scripts.States.Battles.BattleEquationsWithLetters.Common
{
    public enum OperatorType
    {
        Value = 0,
        Variable = 1,
        Assemblable = 2,
        Plus = 3,
        Minus = 4,
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

    public class VariableOperator : BaseOperator
    {
        public int? Value { get; set; } = null;
        public bool HasValue => Value != null;
        public string Name { get; set; }

        public VariableOperator(string name) { this.Name = name; }
        public override int Result => Value ?? 0;
        public override OperatorType OperType => OperatorType.Variable;
        public override string ToString() => Value?.ToString() ?? Name;
    }

    public class AssemblableOperator : BaseOperator
    {
        public BaseOperator[] items;
        public AssemblableOperator(params BaseOperator[] items) { this.items = items; }
        public override int Result => items.Aggregate(0, (acc, x) => acc * 10 + x.Result);
        public override OperatorType OperType => OperatorType.Assemblable;
        public override string ToString() => string.Join<BaseOperator>("", items);
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

    public class EquationOfOperators
    {
        private BaseOperator a, b;
        public EquationOfOperators(BaseOperator a, BaseOperator b) { this.a = a; this.b = b; }
        public bool IsCorrect => a.Result == b.Result;
        public override string ToString() => $"{a} {(IsCorrect ? "=" : "!=")} {b}";
        public string ToEqualString() => $"{a} = {b}";
    }

    public class EquationConfig
    {
        public EquationOfOperators Equation { get; set; }
        public VariableOperator[] Variables { get; set; }
    }

    public class EquationsListConfig
    {
        public EquationOfOperators[] Equations { get; set; }
        public VariableOperator[] Variables { get; set; }
    }

    public static class EquationsGenerator
    {
        public static EquationsListConfig GenerateDisjointEquations(Random random, int count)
        {
            var temp = GenerateVariableEquation(random);

            var configs = new EquationConfig[count];
            for (int i = 0; i < count; i++)
            {
                configs[i] = GenerateVariableEquation(random);
            }

            var result = new EquationsListConfig
            {
                Equations = configs.Select(x => x.Equation).ToArray(),
                Variables = configs.SelectMany(x => x.Variables).ToArray(),
            };

            for (int i = 0; i < result.Variables.Length; i++)
            {
                result.Variables[i].Name = Convert.ToChar('A' + i).ToString();
                result.Variables[i].Value = null;
            }

            return result;
        }

        private static EquationConfig GenerateVariableEquation(Random random)
        {
            var a = random.Next(100, 999);
            var b = random.Next(100, 999);
            var sum = a + b;

            while (sum > 999)
            {
                a = random.Next(100, 999);
                b = random.Next(100, 999);
                sum = a + b;
            }

            var aElements = SplitValue(a);
            var bElements = SplitValue(b);
            var sumElements = SplitValue(sum);

            VariableOperator[] variables;

            var rnd = random.Next(3);
            if (rnd == 0)
            {
                variables = SetVariables(random, aElements, bElements);
            }
            else if (rnd == 1)
            {
                variables = SetVariables(random, bElements, sumElements);
            }
            else
            {
                variables = SetVariables(random, aElements, sumElements);
            }

            var aOperator = new AssemblableOperator(aElements);
            var bOperator = new AssemblableOperator(bElements);
            var sumOperator = new PlusOperator(aOperator, bOperator);

            var resultOperator = new AssemblableOperator(sumElements);

            var equation = new EquationOfOperators(sumOperator, resultOperator);

            return new EquationConfig
            {
                Equation = equation,
                Variables = variables,
            };
        }

        private static BaseOperator[] SplitValue(int value)
        {
            var stack = new Stack<int>();
            while (value > 0)
            {
                stack.Push(value % 10);
                value /= 10;
            }

            var result = new BaseOperator[stack.Count];
            for (int i = 0; i < result.Length; i++)
            {
                var v = stack.Pop();
                result[i] = new ValueOperator(v);
            }

            return result;
        }

        private static VariableOperator[] SetVariables(Random random, BaseOperator[] value1, BaseOperator[] value2)
        {
            var var1index = random.Next(value1.Length);
            var var2index = random.Next(value2.Length);

            var var1value = value1[var1index].Result;
            var var2value = value2[var2index].Result;

            if (var1value == var2value)
            {
                var v = new VariableOperator("VAR") { Value = var1value };
                value1[var1index] = (BaseOperator)v;
                value2[var2index] = (BaseOperator)v;

                return new VariableOperator[] { v };
            }
            else
            {
                var va = new VariableOperator("VAR") { Value = var1value };
                value1[var1index] = (BaseOperator)va;
                var vb = new VariableOperator("VAR") { Value = var2value };
                value2[var2index] = (BaseOperator)vb;

                return new VariableOperator[] { va, vb };
            }
        }
    }
}
