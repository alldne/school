using System;
using System.Collections.Generic;

namespace School
{
    public static class BuiltinFunctions 
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

        private static Value Add(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value + bValue.Value);
        }

        private static Value Sub(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value - bValue.Value);
        }

        private static Value Mul(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value * bValue.Value);
        }

        private static Value Div(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value / bValue.Value);
        }
    }
}

