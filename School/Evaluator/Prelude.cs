using System;
using System.Collections.Generic;

namespace School.Evaluator
{
    public static class Prelude
    {
        private static Dictionary<string, Value> Dict = new Dictionary<string, Value>()
        {
            { "writeLine", new FunValue1(WriteLine) },
            { "readInt", new FunValue1(ReadInt) },
            { "pow", new FunValue2(Pow) }
        };

        public static Value Lookup(string name)
        {
            if (Dict.ContainsKey(name))
                return Dict[name];
            else
                return UnitValue.Singleton;
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

        private static Value Pow(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue((int)Math.Pow(aValue.Value, bValue.Value));
        }
    }
}

