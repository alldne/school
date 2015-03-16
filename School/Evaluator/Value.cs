using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace School.Evaluator
{
    public abstract class Value
    {
    }

    public sealed class UnitValue : Value
    {
        public static readonly UnitValue Singleton = new UnitValue();

        private UnitValue()
        {
        }

        public override string ToString()
        {
            return "()";
        }
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

    public sealed class BooleanValue : Value
    {
        public static readonly BooleanValue True = new BooleanValue(true);
        public static readonly BooleanValue False = new BooleanValue(false);

        private readonly bool value;

        public bool Value
        {
            get { return value; }
        }

        private BooleanValue(bool value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value ? "true" : "false";
        }
    }

    public abstract class FunValue : Value
    {
        public abstract Value Apply(Value arg);

        public Value Apply(Value arg1, Value arg2)
        {
            FunValue fun = Apply(arg1) as FunValue;
            if (fun == null)
                throw new RuntimeTypeError("fun expected");
            return fun.Apply(arg2);
        }
    }

    public class FunValue1 : FunValue
    {
        private readonly Func<Value, Value> value;

        public FunValue1(Func<Value, Value> value)
        {
            this.value = value;
        }

        public override Value Apply(Value arg)
        {
            return value(arg);
        }
    }

    public class FunValue2 : FunValue
    {
        private readonly Func<Value, Value, Value> value;

        public FunValue2(Func<Value, Value, Value> value)
        {
            this.value = value;
        }

        public override Value Apply(Value arg1)
        {
            return new FunValue1((arg2) => value(arg1, arg2));
        }
    }

    public class FunValue3 : FunValue
    {
        private readonly Func<Value, Value, Value, Value> value;

        public FunValue3(Func<Value, Value, Value, Value> value)
        {
            this.value = value;
        }

        public override Value Apply(Value arg1)
        {
            return new FunValue2((arg2, arg3) => value(arg1, arg2, arg3));
        }
    }

    public class ListValue : Value
    {
        private readonly IReadOnlyList<Value> elements;

        public IReadOnlyList<Value> Elements
        {
            get { return elements; }
        }

        public ListValue(IReadOnlyList<Value> elements)
        {
            this.elements = elements;
        }

        public override string ToString()
        {
            return String.Format("[{0}]", String.Join(",", elements.Select(e => e.ToString())));
        }
    }
}
