using System;

namespace School
{
    public class RuntimeTypeError : Exception
    {
        public RuntimeTypeError(string message) : base(message)
        {
        }
    }
}

