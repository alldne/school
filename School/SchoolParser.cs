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
            Surface.Expr expr = ParseExpr();
            if (lookahead.Type != SchoolLexer.EOF_TYPE)
                throw new ParserException("expecting eof; found " + lookahead);
            return expr;
        }

        private Surface.Expr ParseExpr()
        {
            Surface.Expr left = ParseTerm();

            while (lookahead.Type == SchoolLexer.ADD || lookahead.Type == SchoolLexer.SUB)
            {
                int type = lookahead.Type;
                Consume();

                Surface.Expr right = ParseTerm();
                if (type == SchoolLexer.ADD)
                    left = new Surface.Add(left, right);
                else if (type == SchoolLexer.SUB)
                    left = new Surface.Sub(left, right);
                else
                    throw new ParserException("unreachable code");
            }

            return left;
        }

        private Surface.Expr ParseTerm()
        {
            Surface.Expr left = ParseApp();

            while (lookahead.Type == SchoolLexer.MUL || lookahead.Type == SchoolLexer.DIV)
            {
                int type = lookahead.Type;
                Consume();

                Surface.Expr right = ParseApp();
                if (type == SchoolLexer.MUL)
                    left = new Surface.Mul(left, right);
                else if (type == SchoolLexer.DIV)
                    left = new Surface.Div(left, right);
                else
                    throw new ParserException("unreachable code");
            }
                
            return left;
        }

        private bool IsApp()
        {
            int type = lookahead.Type;
            switch (type)
            {
                case SchoolLexer.KEYWORDS:
                    if (lookahead.Text == "fun" || lookahead.Text == "true" || lookahead.Text == "false")
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

            switch (lookahead.Type)
            {
                case SchoolLexer.LPAREN:
                    Match(SchoolLexer.LPAREN);
                    expr = ParseExpr();
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
                    if (lookahead.Text == "true" || lookahead.Text == "false")
                    {
                        bool b = Boolean.Parse(lookahead.Text);
                        expr = new Surface.Boolean(b);
                        Consume();
                    }
                    else
                    {
                        MatchKeyword("fun");

                        IList<Id> argIds = ParseArgIds();

                        if (lookahead.Type != SchoolLexer.ARROW)
                            throw new ParserException("expecting arrow; found " + lookahead);
                        Consume();
                        Surface.Expr bodyExpr = ParseExpr();
                        expr = new Surface.FunAbs(argIds, bodyExpr);

                        MatchKeyword("end");
                    }
                    break;
                default:
                    throw new ParserException("expecting number; found " + lookahead);
            }

            return expr;
        }

        private IList<Id> ParseArgIds()
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

