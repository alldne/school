﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace School
{
    public class Desugarer : Surface.IExprVisitor<Core.Expr>
    {
        public Desugarer() { }

        public Core.Expr Desugar(Surface.Expr expr)
        {
            return expr.Accept(this);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.NamedFunAbsList namedFunAbsList)
        {
            List<Core.Expr> coreExprs = namedFunAbsList.Elements.Select(e => e.Accept(this)).ToList();
            return new Core.NamedFunAbsList(coreExprs);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Program program)
        {
            Core.NamedFunAbsList namedFunAbsList = program.NamedFunAbsList.Accept(this) as Core.NamedFunAbsList;
            Core.Expr expr = program.Expr.Accept(this);
            return new Core.Program(namedFunAbsList, expr);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.ExprList exprs)
        {
            List<Core.Expr> coreExprs = exprs.Exprs.Select(e => e.Accept(this)).ToList();
            return new Core.ExprList(coreExprs);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Unit unit)
        {
            return Core.Unit.Singleton;
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Number number)
        {
            return new Core.Number(number.Value);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Boolean b)
        {
            return new Core.Boolean(b.Value);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.List list)
        {
            List<Core.Expr> elements = list.Elements.Select(e => e.Accept(this)).ToList();
            return new Core.List(elements);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Equal add)
        {
            Core.Expr left = add.Left.Accept(this);
            Core.Expr right = add.Right.Accept(this);
            return new Core.BinaryOperator("equal", left, right);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Add add)
        {
            Core.Expr left = add.Left.Accept(this);
            Core.Expr right = add.Right.Accept(this);
            return new Core.BinaryOperator("add", left, right);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Sub sub)
        {
            Core.Expr left = sub.Left.Accept(this);
            Core.Expr right = sub.Right.Accept(this);
            return new Core.BinaryOperator("sub", left, right);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Mul mul)
        {
            Core.Expr left = mul.Left.Accept(this);
            Core.Expr right = mul.Right.Accept(this);
            return new Core.BinaryOperator("mul", left, right);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.Div div)
        {
            Core.Expr left = div.Left.Accept(this);
            Core.Expr right = div.Right.Accept(this);
            return new Core.BinaryOperator("div", left, right);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.IdExpr idExpr)
        {
            return new Core.IdExpr(idExpr.Id);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.FunAbs funAbs)
        {
            Core.Expr bodyExpr = funAbs.BodyExpr.Accept(this);
            return funAbs.ArgIds.Reverse().Aggregate(bodyExpr, (body, id) => new Core.FunAbs(id, body));
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.NamedFunAbs namedFunAbs)
        {
            Core.FunAbs funAbs = namedFunAbs.FunAbs.Accept(this) as Core.FunAbs;
            return new Core.NamedFunAbs(namedFunAbs.NameId, funAbs);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.FunApp funApp)
        {
            Core.Expr fun = funApp.Fun.Accept(this);
            Core.Expr arg = funApp.Arg.Accept(this);
            return new Core.FunApp(fun, arg);
        }

        Core.Expr Surface.IExprVisitor<Core.Expr>.Visit(Surface.IfExpr ifExpr)
        {
            Core.Expr condExpr = ifExpr.Cond.Accept(this);
            Core.Expr thenExpr = ifExpr.Then.Accept(this);
            Core.Expr elseExpr = ifExpr.Else.Accept(this);
            return new Core.IfExpr(condExpr, thenExpr, elseExpr);
        }
    }
}
