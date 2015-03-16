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
            { "div", Div }
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
    }
}

