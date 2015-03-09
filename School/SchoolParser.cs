﻿using System;
using System.Collections.Generic;

namespace School
{
    // expr    = term  { ("+" | "-") expr }
    // term    = factor  { ("*"|"/") term }
    // factor  = number | "("  expr  ")"
    public class SchoolParser : Parser
    {
        public SchoolParser(Lexer input) : base(input) { }

        public Surface.Expr Parse()
        {
            Surface.Expr expr = Expr();
            if (lookahead.Type != SchoolLexer.EOF_TYPE)
                throw new ParserException("expecting eof; found " + lookahead);
            return expr;
        }

        private Surface.Expr Expr()
        {
            Surface.Expr left = Term();

            int type = lookahead.Type;
            switch (type)
            {
                case SchoolLexer.ADD:
                case SchoolLexer.SUB:
                    Consume();
                    break;
                default:
                    return left;
            }

            Surface.Expr right = Expr();

            Surface.Expr expr;
            switch (type)
            {
                case SchoolLexer.ADD:
                    expr = new Surface.Add(left, right);
                    break;
                case SchoolLexer.SUB:
                    expr = new Surface.Sub(left, right);
                    break;
                default:
                    throw new ParserException("unreachable code");
            }
            return expr;
        }

        private Surface.Expr Term()
        {
            Surface.Expr left = App();

            int type = lookahead.Type;
            switch (type)
            {
                case SchoolLexer.MUL:
                case SchoolLexer.DIV:
                    Consume();
                    break;
                default:
                    return left;
            }

            Surface.Expr right = Term();

            Surface.Expr expr;
            switch (type)
            {
                case SchoolLexer.MUL:
                    expr = new Surface.Mul(left, right);
                    break;
                case SchoolLexer.DIV:
                    expr = new Surface.Div(left, right);
                    break;
                default:
                    throw new ParserException("unreachable code");
            }
            return expr;
        }

        private bool IsApp()
        {
            int type = lookahead.Type;
            switch (type)
            {
                case SchoolLexer.KEYWORDS:
                    if (lookahead.Text == "fun")
                        return true;
                    else
                        return false;
                case SchoolLexer.LPAREN:
                case SchoolLexer.NUM:
                case SchoolLexer.ID:
                    return true;
                default:
                    return false;
            }
        }

        private Surface.Expr App()
        {
            Surface.Expr left = Factor();

            while (IsApp())
            {
                Surface.Expr right = Factor();
                left = new Surface.FunApp(left, right);
            }

            return left;
        }

        private Surface.Expr Factor()
        {
            Surface.Expr expr;

            switch (lookahead.Type)
            {
                case SchoolLexer.LPAREN:
                    Match(SchoolLexer.LPAREN);
                    expr = Expr();
                    Match(SchoolLexer.RPAREN);
                    break;
                case SchoolLexer.NUM:
                    string numberText = lookahead.Text;
                    expr = new Surface.Number(Int32.Parse(numberText));
                    Consume();
                    break;
                case SchoolLexer.ID:
                    string idText = lookahead.Text;
                    expr = new Surface.IdExpr(Id.id(idText));
                    Consume();
                    break;
                case SchoolLexer.KEYWORDS:
                    MatchKeyword("fun");

                    IList<Id> argIds = ArgIds();

                    if (lookahead.Type != SchoolLexer.ARROW)
                        throw new ParserException("expecting arrow; found " + lookahead);
                    Consume();
                    Surface.Expr bodyExpr = Expr();
                    expr = new Surface.FunAbs(argIds, bodyExpr);

                    MatchKeyword("end");
                    break;
                default:
                    throw new ParserException("expecting number; found " + lookahead);
            }

            return expr;
        }

        private IList<Id> ArgIds()
        {
            if (lookahead.Type != SchoolLexer.ID)
                throw new ParserException("expecting id; found " + lookahead);

            var argIds = new List<Id>();
            do
            {
                Id argId = Id.id(lookahead.Text);
                argIds.Add(argId);
                Consume();
            } while (lookahead.Type == SchoolLexer.ID);

            return argIds;
        }

        private void MatchKeyword(string keyword)
        {
            if (lookahead.Type == SchoolLexer.KEYWORDS && lookahead.Text == keyword)
                Consume();
            else
                throw new ParserException("expecting keyword " + keyword + 
                    "; found " + lookahead);
        }
    }
}

