using System;
using System.Collections.Generic;

namespace School.Evaluator
{
    public class RuntimeTypeError : Exception
    {
        public RuntimeTypeError(string message) : base(message)
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
            var lexer = new SchoolLexer(code);
            var parser = new SchoolParser(lexer);
            Surface.Expr expr = parser.Parse();

            var desugarer = new Desugarer();
            Core.Expr coreExpr = desugarer.Desugar(expr);

            return Evaluate(coreExpr);
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

        Value Core.IExprVisitor<Value>.Visit(Core.FunAbs funAbs)
        {
            Env closure = this.env;
            Func<Value, Value> fun = a =>
            {
                    Env oldEnv = this.env;
                    this.env = closure.Add(funAbs.ArgId, a);
                    Value result = funAbs.BodyExpr.Accept(this);
                    this.env = oldEnv;
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
