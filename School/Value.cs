using System;

namespace School
{
    public abstract class Value
    {
    }

    public class IntValue : Value
    {
        private readonly int value;

        public int Value
        {
            get { return value; }
        }

        public IntValue(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class FunValue : Value
    {
        private readonly Func<Value, Value> value;

        public Func<Value, Value> Value
        {
            get { return value; }
        }

        public FunValue(Func<Value, Value> value)
        {
            this.value = value;
        }

        public Value Apply(Value arg)
        {
            return value(arg);
        }
    }
}
