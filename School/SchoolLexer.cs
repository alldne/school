using System;
using System.Text;
using System.Collections.Generic;

namespace School
{
    public class SchoolLexer : Lexer
    {
        public const int ADD = 2;
        public const int SUB = 3;
        public const int MUL = 4;
        public const int DIV = 5;
        public const int LPAREN = 6;
        public const int RPAREN = 7;
        public const int NUM = 8;
        public const int ID = 9;
        public const int KEYWORDS = 10;
        public const int ARROW = 11;
        public static readonly string[] tokenNames =
            { "n/a", "<EOF>", "ADD", "SUB", "MUL", "DIV", "LPAREN", "RPAREN", "NUM", "ID", "KEYWORDS", "ARROW" };
        private static readonly ISet<string> keywords = new HashSet<string>() { "fun", "end", "true", "false" };

        public override String GetTokenName(int x) { return tokenNames[x]; }

        public SchoolLexer(string input) : base(input) { }

        public override Token NextToken()
        {
            while (c != EOF)
            {
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r':
                        WS();
                        continue;
                    case '+':
                        Consume();
                        return new Token(ADD, "+");
                    case '-':
                        Consume();
                        if (c == '>')
                        {
                            Consume();
                            return new Token(ARROW, "->");
                        }
                        return new Token(SUB, "-");
                    case '*':
                        Consume();
                        return new Token(MUL, "*");
                    case '/':
                        Consume();
                        return new Token(DIV, "/");
                    case '(':
                        Consume();
                        return new Token(LPAREN, "(");
                    case ')':
                        Consume();
                        return new Token(RPAREN, ")");
                    default:
                        if (IsLetter())
                            return IDENTIFIER_OR_KEYWORDS();
                        if (IsDigit())
                            return NUMBER();
                        throw new LexerException("invalid character: " + c);
                }
            }

            return new Token(EOF_TYPE, "<EOF>");
        }
            
        private bool IsDigit()
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLetter()
        {
            return Char.IsLetter(c);
        }

        /** DIGIT   : '0'..'9'; // define what a digit is */
        private void DIGIT()
        {
            if (IsDigit())
                Consume();
            else
                throw new LexerException("expecting DIGIT; found " + c);
        }

        private void LETTER()
        {
            if (IsLetter())
                Consume();
            else
                throw new LexerException("expecting LETTER; found " + c);
        }

        /** ID : LETTER+ ; // ID is sequence of >= 1 letter */
        private Token IDENTIFIER_OR_KEYWORDS()
        {
            var buf = new StringBuilder();
            do
            {
                buf.Append(c);
                LETTER();
            } while (IsLetter());

            string str = buf.ToString();
            if (keywords.Contains(str))
                return new Token(KEYWORDS, str);
            else 
                return new Token(ID, str);
        }

        /** NUMBER : DIGIT+ ; // NUMBER is sequence of >=1 digit */
        private Token NUMBER()
        {
            var buf = new StringBuilder();
            do
            {
                buf.Append(c);
                DIGIT();
            } while (IsDigit());
            return new Token(NUM, buf.ToString());
        }

        /** WS : (' '|'\t'|'\n'|'\r')* ; // ignore any whitespace */
        private void WS()
        {
            while (c == ' ' || c == '\t' || c == '\n' || c == '\r')
                Consume();
        }
    }
}
