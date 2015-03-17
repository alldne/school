using System;
using System.Collections.Generic;
using System.IO;

namespace School
{
    public class SchoolParser : Parser
    {
        public SchoolParser(Lexer input) : base(input) { }
        public SchoolParser(StreamReader reader) : base(new SchoolLexer(reader))
        {
        }

        public Surface.Expr Parse()
        {
            Surface.Expr expr = ParseProgram();
            if (LookAhead.Type != SchoolLexer.EOF_TYPE)
                throw new ParserException("expecting eof; found " + LookAhead);
            return expr;
        }

        private Surface.Expr ParseProgram()
        {
            Surface.NamedFunAbsList namedFunAbsList = ParseNamedFunAbsList();
            Surface.Expr expr;
            if (LookAhead.Type != SchoolLexer.EOF_TYPE)
                expr = ParseExprList();
            else
                expr = Surface.Unit.Singleton;
            return new Surface.Program(namedFunAbsList, expr);
        }

        private Surface.NamedFunAbsList ParseNamedFunAbsList()
        {
            List<Surface.Expr> namedFunAbsList = new List<Surface.Expr>();
            if (IsNamedFunAbs())
            {
                namedFunAbsList.Add(ParseNamedFunAbs());
                while (IsNamedFunAbs())
                    namedFunAbsList.Add(ParseNamedFunAbs());
            }
            return new Surface.NamedFunAbsList(namedFunAbsList);
        }

        private Surface.Expr ParseExprList()
        {
            List<Surface.Expr> elements = new List<Surface.Expr>();
            elements.Add(ParseExpr());
            while (LookAhead.Type == SchoolLexer.SEMICOLON)
            {
                Consume();
                elements.Add(ParseExpr());
            }
            return elements.Count == 1 ? elements[0] : new Surface.ExprList(elements);
        }

        private Surface.Expr ParseComparisonExpr()
        {
            Surface.Expr left = ParseArithmeticExpr();
            if (LookAhead.Type == SchoolLexer.GT || LookAhead.Type == SchoolLexer.LT)
            {
                int type = LookAhead.Type;
                Consume();

                Surface.Expr right = ParseComparisonExpr();

                if (type == SchoolLexer.GT)
                    left = new Surface.Gt(left, right);
                else if (type == SchoolLexer.LT)
                    left = new Surface.Lt(left, right);
                else
                    throw new ParserException("unreachable code");
            }

            return left;
        }
        private Surface.Expr ParseExpr()
        {
            Surface.Expr left = ParseComparisonExpr();

            while (LookAhead.Type == SchoolLexer.EQUAL)
            {
                int type = LookAhead.Type;
                Consume();

                Surface.Expr right = ParseComparisonExpr();
                left = new Surface.Equal(left, right);
            }

            return left;
        }

        private Surface.Expr ParseArithmeticExpr()
        {
            Surface.Expr left = ParseTerm();

            while (LookAhead.Type == SchoolLexer.ADD || LookAhead.Type == SchoolLexer.SUB)
            {
                int type = LookAhead.Type;
                Consume();

                Surface.Expr right = ParseTerm();
                if (type == SchoolLexer.ADD)
                    left = new Surface.Add(left, right);
                else if (type == SchoolLexer.SUB)
                    left = new Surface.Sub(left, right);
                else if (type == SchoolLexer.GT)
                    left = new Surface.Gt(left, right);
                else if (type == SchoolLexer.LT)
                    left = new Surface.Lt(left, right);
                else
                    throw new ParserException("unreachable code");
            }

            return left;
        }
 
        private Surface.Expr ParseTerm()
        {
            Surface.Expr left = ParseComposition();

            while (LookAhead.Type == SchoolLexer.MUL || LookAhead.Type == SchoolLexer.DIV)
            {
                int type = LookAhead.Type;
                Consume();

                Surface.Expr right = ParseComposition();
                if (type == SchoolLexer.MUL)
                    left = new Surface.Mul(left, right);
                else if (type == SchoolLexer.DIV)
                    left = new Surface.Div(left, right);
                else
                    throw new ParserException("unreachable code");
            }
                
            return left;
        }

        private Surface.Expr ParseComposition()
        {
            Surface.Expr left = ParseApp();

            while (LookAhead.Type == SchoolLexer.RIGHT_COMPOSITION)
            {
                int type = LookAhead.Type;
                Consume();

                Surface.Expr right = ParseApp();
                left = new Surface.Composition(left, right);
            }

            return left;
        }

        private bool IsApp()
        {
            int type = LookAhead.Type;
            switch (type)
            {
                case SchoolLexer.KEYWORD:
                    if (LookAhead.Text == "fun" || LookAhead.Text == "let" || LookAhead.Text == "true" || LookAhead.Text == "false" || LookAhead.Text == "if")
                        return true;
                    else
                        return false;
                case SchoolLexer.LPAREN:
                case SchoolLexer.LBRACKET:
                case SchoolLexer.UNIT:
                case SchoolLexer.NUM:
                case SchoolLexer.ID:
                    return true;
                default:
                    return false;
            }
        }

        private Surface.Expr ParseApp()
        {
            Surface.Expr left = ParseFactor();

            while (IsApp())
            {
                Surface.Expr right = ParseFactor();
                left = new Surface.FunApp(left, right);
            }

            return left;
        }

        private Surface.Expr ParseFactor()
        {
            Surface.Expr expr;

            switch (LookAhead.Type)
            {
                case SchoolLexer.LBRACKET:
                    expr = ParseList();
                    break;
                case SchoolLexer.LPAREN:
                    Match(SchoolLexer.LPAREN);
                    expr = ParseExprList();
                    Match(SchoolLexer.RPAREN);
                    break;
                case SchoolLexer.UNIT:
                    expr = Surface.Unit.Singleton;
                    Consume();
                    break;
                case SchoolLexer.NUM:
                    expr = ParseNumber();
                    break;
                case SchoolLexer.ID:
                    expr = ParseIdExpr();
                    break;
                case SchoolLexer.KEYWORD:
                    if (IsBoolean())
                        expr = ParseBoolean();
                    else if (IsIf())
                        expr = ParseIf();
                    else
                        expr = ParseFunAbs();
                    break;
                default:
                    throw new ParserException("expecting number; found " + LookAhead);
            }

            return expr;
        }

        private Surface.Expr ParseNumber()
        {
            string numberText = LookAhead.Text;
            Surface.Expr expr = new Surface.Number(Int32.Parse(numberText));
            Consume();
            return expr;
        }

        private Surface.Expr ParseIdExpr()
        {
            string idText = LookAhead.Text;
            Surface.Expr expr = new Surface.IdExpr(Id.id(idText));
            Consume();
            return expr;
        }

        private Surface.Expr ParseList()
        {
            Match(SchoolLexer.LBRACKET);
            List<Surface.Expr> elements = new List<Surface.Expr>();
            if (LookAhead.Type != SchoolLexer.RBRACKET)
            {
                elements.Add(ParseExprList());
                while (LookAhead.Type == SchoolLexer.COMMNA)
                {
                    Consume();
                    elements.Add(ParseExprList());
                }
            }
            Surface.Expr expr = new Surface.List(elements);
            Match(SchoolLexer.RBRACKET);
            return expr;
        }

        private bool IsIf()
        {
            return LookAhead.Text == "if";
        }

        private Surface.Expr ParseIf()
        {
            MatchKeyword("if");
            Surface.Expr condExpr = ParseExprList();
            MatchKeyword("then");
            Surface.Expr thenExpr = ParseExprList();
            MatchKeyword("else");
            Surface.Expr elseExpr = ParseExprList();

            return new Surface.IfExpr(condExpr, thenExpr, elseExpr);
        }

        private Surface.Expr ParseFunAbs()
        {
            MatchKeyword("fun");
            IReadOnlyList<Id> argIds = ParseArgIds();
            Match(SchoolLexer.ARROW);
            Surface.Expr bodyExpr = ParseExprList();
            MatchKeyword("end");

            return new Surface.FunAbs(argIds, bodyExpr);
        }

        private bool IsNamedFunAbs()
        {
            return LookAhead.Text == "let";
        }

        private Id ParseId()
        {
            if (LookAhead.Type != SchoolLexer.ID)
                throw new ParserException("expecting id; found " + LookAhead);

            Id id = Id.id(LookAhead.Text);
            Consume();
            return id;
        }

        private Surface.Expr ParseNamedFunAbs()
        {
            MatchKeyword("let");
            Id nameId = ParseId();
            IReadOnlyList<Id> argIds = ParseArgIds();
            Match(SchoolLexer.EQUAL);
            Surface.Expr bodyExpr = ParseExprList();
            MatchKeyword("end");

            return new Surface.NamedFunAbs(nameId, new Surface.FunAbs(argIds, bodyExpr));
        }

        private bool IsBoolean()
        {
            return LookAhead.Text == "true" || LookAhead.Text == "false";
        }

        private Surface.Expr ParseBoolean()
        {
            if (LookAhead.Text != "true" && LookAhead.Text != "false")
                throw new ParserException("expecting boolean; found " + LookAhead);

            bool b = Boolean.Parse(LookAhead.Text);
            Consume();
            return new Surface.Boolean(b);
        }

        private IReadOnlyList<Id> ParseArgIds()
        {
            if (LookAhead.Type != SchoolLexer.ID)
                throw new ParserException("expecting id; found " + LookAhead);

            var argIds = new List<Id>();
            do
            {
                Id argId = Id.id(LookAhead.Text);
                argIds.Add(argId);
                Consume();
            } while (LookAhead.Type == SchoolLexer.ID);

            return argIds;
        }

        private void MatchKeyword(string keyword)
        {
            if (LookAhead.Type == SchoolLexer.KEYWORD && LookAhead.Text == keyword)
                Consume();
            else
                throw new ParserException("expecting keyword " + keyword + 
                    "; found " + LookAhead);
        }
    }
}

