using System;

namespace School.Evaluator
{
    public class Env
    {
        public class UnknownIdentifierException : Exception
        {
            public UnknownIdentifierException(string message) : base(message)
            {
            }
        }

        public static Env Empty = new Env();
       
        private Id id;
        private Value value;
        private Env oldEnv;

        private Env()
        {
        }

        private Env(Id id, Value value, Env oldEnv)
        {
            this.id = id;
            this.value = value;
            this.oldEnv = oldEnv;
        }
            
        public Env Add(Id id, Value value)
        {
            return new Env(id, value, this);
        }

        public Value Lookup(Id id)
        {
            if (this == Empty)
                throw new UnknownIdentifierException("Unknown identifier " + id.ToString());

            if (id == this.id)
                return value;

            return oldEnv.Lookup(id);
        }
    }
}
