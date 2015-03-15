using System;
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
