using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

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
        public const int KEYWORD = 10;
        public const int ARROW = 11;
        public const int UNIT = 12;
        public const int LBRACKET = 13;
        public const int RBRACKET = 14;
        public const int COMMNA = 15;
        public const int SEMICOLON = 16;
        public const int EQUAL = 17;
        public const int RIGHT_COMPOSITION = 18;
        public const int GT = 19;
        public const int LT = 20;
        public const int GTE = 21;
        public const int LTE = 22;
        public static readonly string[] tokenNames =
            { "n/a", "<EOF>", "ADD", "SUB", "MUL", "DIV", "LPAREN", "RPAREN",
                "NUM", "ID", "KEYWORDS", "ARROW", "UNIT", "LBRACKET", "RBRACKET",
                "COMMA", "SEMICOLON", "EQUAL", "RIGHT_COMPOSITION", "GT", "LT", "GTE", "LTE" };

        private static readonly IImmutableSet<string> keywords;

        static SchoolLexer()
        {
            string[] ks = { "let", "fun", "end", "true", "false", "if", "then", "else" };
            var builder = ImmutableHashSet.CreateBuilder<string>();
            foreach (var k in ks)
                builder.Add(k);
            keywords = builder.ToImmutableHashSet();
        }

        public override String GetTokenName(int x) { return tokenNames[x]; }

        public SchoolLexer(StreamReader reader) : base(reader) { }

        public override Token NextToken()
        {
            while (!IsEOF())
            {
                switch (LookAhead)
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
                        if (LookAhead == '>')
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
                        if (LookAhead == ')')
                        {
                            Consume();
                            return new Token(UNIT, "()");
                        }
                        return new Token(LPAREN, "(");
                    case ')':
                        Consume();
                        return new Token(RPAREN, ")");
                    case '[':
                        Consume();
                        return new Token(LBRACKET, "[");
                    case ']':
                        Consume();
                        return new Token(RBRACKET, "]");
                    case ',':
                        Consume();
                        return new Token(COMMNA, ",");
                    case ';':
                        Consume();
                        return new Token(SEMICOLON, ";");
                    case '=':
                        Consume();
                        return new Token(EQUAL, "=");
                    case '>':
                        Consume();
                        if (LookAhead == '>')
                        {
                            Consume();
                            return new Token(RIGHT_COMPOSITION, ">>");
                        }
                        else if (LookAhead == '=')
                        {
                            Consume();
                            return new Token(GTE, ">=");
                        }
                        return new Token(GT, ">");
                    case '<':
                        Consume();
                        if (LookAhead == '=')
                        {
                            Consume();
                            return new Token(LTE, "<=");
                        }
                        return new Token(LT, "<");
                    default:
                        if (IsLetter())
                            return IDENTIFIER_OR_KEYWORDS();
                        if (IsDigit())
                            return NUMBER();
                        throw new LexerException("invalid character: " + LookAhead);
                }
            }

            return new Token(EOF_TYPE, "<EOF>");
        }
            
        private bool IsDigit()
        {
            return LookAhead >= '0' && LookAhead <= '9';
        }

        private bool IsLetter()
        {
            return Char.IsLetter(LookAhead);
        }

        /** DIGIT   : '0'..'9'; // define what a digit is */
        private void DIGIT()
        {
            if (IsDigit())
                Consume();
            else
                throw new LexerException("expecting DIGIT; found " + LookAhead);
        }

        private void LETTER()
        {
            if (IsLetter())
                Consume();
            else
                throw new LexerException("expecting LETTER; found " + LookAhead);
        }

        /** ID : LETTER+ ; // ID is sequence of >= 1 letter */
        private Token IDENTIFIER_OR_KEYWORDS()
        {
            var buf = new StringBuilder();
            do
            {
                buf.Append(LookAhead);
                LETTER();
            } while (IsLetter());

            string str = buf.ToString();
            if (keywords.Contains(str))
                return new Token(KEYWORD, str);
            else 
                return new Token(ID, str);
        }

        /** NUMBER : DIGIT+ ; // NUMBER is sequence of >=1 digit */
        private Token NUMBER()
        {
            var buf = new StringBuilder();
            do
            {
                buf.Append(LookAhead);
                DIGIT();
            } while (IsDigit());
            return new Token(NUM, buf.ToString());
        }

        /** WS : (' '|'\t'|'\n'|'\r')* ; // ignore any whitespace */
        private void WS()
        {
            while (LookAhead == ' ' || LookAhead == '\t' || LookAhead == '\n' || LookAhead == '\r')
                Consume();
        }
    }
}
