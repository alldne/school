using School;
using School.Surface;
using System;

namespace SchoolLint
{
    public class SchoolLint
    {
        public SchoolLint()
        {
        }

        public string PrettyPrint(string code)
        {
            var lexer = new SchoolLexer(code);
            var parser = new SchoolParser(lexer);
            Expr expr = parser.Parse();

            var prettyPrinter = new Printer();
            return prettyPrinter.Print(expr);
        }
    }
}

