using School.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace School.Evaluator
{
    public class RuntimeError : Exception
    {
        public RuntimeError(string message) : base(message)
        {
        }
    }

    public class Evaluator : Core.IExprVisitor<Value>
    {
        private Env env = Env.Empty;

        public Evaluator() { }

        public Value Evaluate(Core.Expr expr)
        {
            return expr.Accept(this);
        }

        public Value Evaluate(string code)
        {
            var parser = new SchoolParser(code.ToStreamReader());
            Surface.Expr expr = parser.Parse();

            var desugarer = new Desugarer();
            Core.Expr coreExpr = desugarer.Desugar(expr);

            return Evaluate(coreExpr);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.Program program)
        {
            program.NamedFunAbsList.Accept(this);
            return program.Expr.Accept(this);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.NamedFunAbsList namedFunAbsList)
        {
            foreach (var e in namedFunAbsList.Elements)
                e.Accept(this);
            return UnitValue.Singleton;
        }

        Value Core.IExprVisitor<Value>.Visit(Core.ExprList exprs)
        {
            Value result = UnitValue.Singleton;
            foreach (var expr in exprs.Exprs)
                result = expr.Accept(this);
            return result;
        }

        Value Core.IExprVisitor<Value>.Visit(Core.Unit unit)
        {
            return UnitValue.Singleton;
        }

        Value Core.IExprVisitor<Value>.Visit(Core.Number number)
        {
            return new IntValue(number.Value);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.Boolean b)
        {
            return b.Value ? BooleanValue.True : BooleanValue.False;
        }

        Value Core.IExprVisitor<Value>.Visit(Core.List list)
        {
            List<Value> values = list.Elements.Select(e => e.Accept(this)).ToList();
            return new ListValue(values);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.BinaryOperator app)
        {
            Value left = app.Left.Accept(this);
            Value right = app.Right.Accept(this);

            Func<Value, Value, Value> func = BinaryOperators.Lookup(app.Name);
            return func(left, right);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.IdExpr idExpr)
        {
            return env.Lookup(idExpr.Id);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.NamedFunAbs namedFunAbs)
        {
            FunValue1 funValue = new FunValue1();
            this.env = env.Add(namedFunAbs.NameId, funValue);
            funValue.Value = (namedFunAbs.FunAbs.Accept(this) as FunValue1).Value;

            return funValue;
        }

        Value Core.IExprVisitor<Value>.Visit(Core.FunAbs funAbs)
        {
            Env closure = this.env;
            Func<Value, Value> fun = a =>
            {
                    Env oldEnv = this.env;
                    this.env = closure.Add(funAbs.ArgId, a);
                    Value result;
                    try {
                        result = funAbs.BodyExpr.Accept(this);
                    } finally {
                        this.env = oldEnv;
                    }
                    return result;
            };
            return new FunValue1(fun);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.FunApp funApp)
        {
            FunValue funValue = funApp.Fun.Accept(this) as FunValue;
            if (funValue == null)
                throw new RuntimeTypeError("function expected");
            Value argValue = funApp.Arg.Accept(this);

            return funValue.Apply(argValue);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.IfExpr ifExpr)
        {
            BooleanValue cond = ifExpr.Cond.Accept(this) as BooleanValue;
            if (cond.Value)
                return ifExpr.Then.Accept(this);
            else
                return ifExpr.Else.Accept(this);
        }
    }
}
