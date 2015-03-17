using System;

namespace School
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message) { }
    }

    public abstract class Parser
    {
        private Lexer input;        // from where do we get tokens?
        private Token lookAhead;

        protected Token LookAhead
        {
            get { return lookAhead; }
        }

        public Parser(Lexer input)
        {
            this.input = input;
            Consume();
        }

        public void Match(int x)
        {
            if (LookAhead.Type == x)
                Consume();
            else
                throw new ParserException("expecting " + input.GetTokenName(x) + 
                    "; found " + LookAhead);
        }

        public void Consume()
        {
            lookAhead = input.NextToken();
        }
    }
}
