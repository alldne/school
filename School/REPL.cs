using System;
using Mono.Terminal;

namespace School
{
    public class REPL
    {
        public REPL()
        {
        }

        public void Run()
        {
            Evaluator evaluator = new Evaluator();
            LineEditor editor = new LineEditor("School");

            Console.WriteLine("School REPL:");
            string line;
            while ((line = editor.Edit("> ", "")) != null)
            {
                try {
                    Value value = evaluator.Evaluate(line);
                    Console.WriteLine(value);
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }
    }
}

