using System;
using System.Collections.Generic;

namespace School.Evaluator
{
    public static class BinaryOperators 
    {
        private static readonly Dictionary<string, Func<Value, Value, Value>> Dict = new Dictionary<string, Func<Value, Value, Value>>
        {
            { "add", Add },
            { "sub", Sub },
            { "mul", Mul },
            { "div", Div },
            { "equal", Equal },
            { "compose", Compose },
            { "gt", Gt },
            { "lt", Lt },
            { "gte", Gte },
            { "lte", Lte }
        };

        public static Func<Value, Value, Value> Lookup(string name)
        {
            return Dict[name];
        }

        private static Value Add(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(a.Value + b.Value);
        }

        private static Value Sub(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(a.Value - b.Value);
        }

        private static Value Mul(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(a.Value * b.Value);
        }

        private static Value Div(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(a.Value / b.Value);
        }

        private static Value Equal(Value aValue, Value bValue)
        {
            return aValue.Equals(bValue) ? BooleanValue.True : BooleanValue.False;
        }

        private static Value Compose(Value aValue, Value bValue)
        {
            FunValue f = aValue as FunValue;
            FunValue g = bValue as FunValue;
            if (f == null || g == null)
                throw new RuntimeTypeError("fun expected");

            return new FunValue1(x => g.Apply(f.Apply(x)));
        }

        private static Value Gt(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return a.Value > b.Value ? BooleanValue.True : BooleanValue.False;
        }

        private static Value Lt(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return a.Value < b.Value ? BooleanValue.True : BooleanValue.False;
        }

        private static Value Gte(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return a.Value >= b.Value ? BooleanValue.True : BooleanValue.False;
        }

        private static Value Lte(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return a.Value <= b.Value ? BooleanValue.True : BooleanValue.False;
        }
    }
}
