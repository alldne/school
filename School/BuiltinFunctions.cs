using System;

namespace School
{
    public static class BuiltinFunctions 
    {
        public static Value Add(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value + bValue.Value);
        }

        public static Value Sub(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value - bValue.Value);
        }

        public static Value Mul(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value * bValue.Value);
        }

        public static Value Div(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value / bValue.Value);
        }
    }
}

