using System;
using System.IO;

namespace School
{
    public class LexerException : Exception
    {
        public LexerException(string message) : base(message) { }
    }

    public abstract class Lexer
    {
        public const char EOF = '\x1a';      // represent end of file char
        public const int EOF_TYPE = 1;       // represent EOF token type

        private readonly StreamReader reader;
        protected char LookAhead
        {
            get
            {
                int c = reader.Peek();
                if (c != -1)
                    return (char)c;
                else
                    return EOF;
            }
        }

        public Lexer(StreamReader reader)
        {
            this.reader = reader;
        }

        public void Consume()
        {
            reader.Read();
        }

        public void Match(char x)
        {
            if (LookAhead == x)
                Consume();
            else
                throw new LexerException("expecting " + x + "; found " + LookAhead);
        }

        public abstract Token NextToken();
        public abstract String GetTokenName(int tokenType);
    }
}
