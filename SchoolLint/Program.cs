using System;
using System.IO;

namespace SchoolLint
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: SchoolLint schoolfile");
                Environment.Exit(0);
            }

            string path = args[0];
            using (StreamReader sr = File.OpenText(path))
            {
                string code = sr.ReadToEnd();
                SchoolLint lint = new SchoolLint();
                string prettified = lint.PrettyPrint(code);
                Console.WriteLine(prettified);
            }
        }
    }
}
