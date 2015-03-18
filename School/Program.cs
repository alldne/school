using System;
using System.IO;

namespace School
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                REPL.REPL repl = new REPL.REPL();
                repl.Run();
            }
            else
            {
                string path = args[0];
                using (StreamReader sr = File.OpenText(path))
                {
                    var evaluator = new Evaluator.Evaluator();
                    evaluator.Evaluate(sr);
                }
            }
        }
    }
}
