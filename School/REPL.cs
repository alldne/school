using System;

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
            string line;

            Console.WriteLine("School REPL:");
            do
            {
                Console.Write("> ");
                line = Console.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    try {
                        Value value = evaluator.Evaluate(line);
                        Console.WriteLine(value);
                    } catch (Exception e) {
                        Console.WriteLine(e);
                    }
                }
            } while (line != null);
        }
    }
}

