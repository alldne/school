using System;

namespace School
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            REPL.REPL repl = new REPL.REPL();
            repl.Run();
        }
    }
}
