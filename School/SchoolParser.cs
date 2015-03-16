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
            Surface.Expr expr = ParseExprList();
            if (lookahead.Type != SchoolLexer.EOF_TYPE)
                throw new ParserException("expecting eof; found " + lookahead);
            return expr;
        }

        private Surface.Expr ParseExprList()
        {
            IList<Surface.Expr> elements = new List<Surface.Expr>();
            elements.Add(ParseExpr());
            while (lookahead.Type == SchoolLexer.SEMICOLON)
            {
                Consume();
                elements.Add(ParseExpr());
            }
            return elements.Count == 1 ? elements[0] : new Surface.ExprList(elements);
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
                case SchoolLexer.KEYWORD:
                    if (lookahead.Text == "fun" || lookahead.Text == "true" || lookahead.Text == "false" || lookahead.Text == "if")
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

            switch (lookahead.Type)
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
                    string numberText = lookahead.Text;
                    expr = new Surface.Number(Int32.Parse(numberText));
                    Consume();
                    break;
                case SchoolLexer.ID:
                    expr = ParseIdExpr();
                    break;
                case SchoolLexer.KEYWORD:
                    if (lookahead.Text == "true" || lookahead.Text == "false")
                        expr = ParseBoolean();
                    else if (lookahead.Text == "if")
                        expr = ParseIf();
                    else
                        expr = ParseFunAbs();
                    break;
                default:
                    throw new ParserException("expecting number; found " + lookahead);
            }

            return expr;
        }

        private Surface.Expr ParseIdExpr()
        {
            string idText = lookahead.Text;
            Surface.Expr expr = new Surface.IdExpr(Id.id(idText));
            Consume();
            return expr;
        }

        private Surface.Expr ParseList()
        {
            Match(SchoolLexer.LBRACKET);
            IList<Surface.Expr> elements = new List<Surface.Expr>();
            if (lookahead.Type != SchoolLexer.RBRACKET)
            {
                elements.Add(ParseExprList());
                while (lookahead.Type == SchoolLexer.COMMNA)
                {
                    Consume();
                    elements.Add(ParseExprList());
                }
            }
            Surface.Expr expr = new Surface.List(elements);
            Match(SchoolLexer.RBRACKET);
            return expr;
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
            Surface.Expr expr;

            MatchKeyword("fun");

            IList<Id> argIds = ParseArgIds();

            if (lookahead.Type != SchoolLexer.ARROW)
                throw new ParserException("expecting arrow; found " + lookahead);
            Consume();
            Surface.Expr bodyExpr = ParseExprList();
            expr = new Surface.FunAbs(argIds, bodyExpr);

            MatchKeyword("end");

            return expr;
        }

        private Surface.Expr ParseBoolean()
        {
            if (lookahead.Text != "true" && lookahead.Text != "false")
                throw new ParserException("expecting boolean; found " + lookahead);

            bool b = Boolean.Parse(lookahead.Text);
            Consume();
            return new Surface.Boolean(b);
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
            if (lookahead.Type == SchoolLexer.KEYWORD && lookahead.Text == keyword)
                Consume();
            else
                throw new ParserException("expecting keyword " + keyword + 
                    "; found " + lookahead);
        }
    }
}

