using System;
using System.Collections.Generic;
using System.Linq;

namespace School.Evaluator
{
    public static class Prelude
    {
        private static Dictionary<string, Value> Dict;

        public static Value Lookup(string name)
        {
            if (Dict.ContainsKey(name))
                return Dict[name];
            else
                return UnitValue.Singleton;
        }

        public static void Register(string name, Func<Value, Value> fun)
        {
            Dict.Add(name, new FunValue1(fun));
        }

        public static void Register(string name, Func<Value, Value, Value> fun)
        {
            Dict.Add(name, new FunValue2(fun));
        }

        public static void Register(string name, Func<Value, Value, Value, Value> fun)
        {
            Dict.Add(name, new FunValue3(fun));
        }

        private static Value WriteLine(Value value)
        {
            Console.WriteLine(value.ToString());
            return UnitValue.Singleton;
        }

        private static Value ReadInt(Value value)
        {
            string line = Console.ReadLine();
            return new IntValue(Int32.Parse(line));
        }

        private static Value Pow(Value aValue, Value bValue)
        {
            IntValue a = aValue as IntValue;
            IntValue b = bValue as IntValue;
            if (a == null || b == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue((int)Math.Pow(a.Value, b.Value));
        }

        private static Value Fold(Value listValue, Value seedValue, Value funValue)
        {
            ListValue list = listValue as ListValue;
            if (list == null)
                throw new RuntimeTypeError("list expected");

            FunValue fun = funValue as FunValue;
            if (fun == null)
                throw new RuntimeTypeError("fun expected");

            return list.Elements.Aggregate(seedValue, fun.Apply);
        }

        static Prelude()
        {
            Dict = new Dictionary<string, Value>();
            Register("writeLine", WriteLine);
            Register("readInt", ReadInt);
            Register("pow", Pow);
            Register("fold", Fold);
        }
    }
}
