using School;
using School.Surface;
using System;
using System.IO;

namespace SchoolLint
{
    public class SchoolLint
    {
        public SchoolLint()
        {
        }

        public string PrettyPrint(StreamReader reader)
        {
            var parser = new SchoolParser(reader);
            Expr expr = parser.Parse();

            var prettyPrinter = new Printer();
            return prettyPrinter.Print(expr);
        }
    }
}

