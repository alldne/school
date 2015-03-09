using System;
using System.Collections.Generic;

namespace School
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

        Value Core.IExprVisitor<Value>.Visit(Core.Number number)
        {
            return new IntValue(number.Value);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.Boolean b)
        {
            return new BooleanValue(b.Value);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.BuiltinFunApp app)
        {
            // FIXME: Currently, we assume that all builtin functions are binary.
            Value arg0 = app.Args[0].Accept(this);
            Value arg1 = app.Args[1].Accept(this);

            Func<Value, Value, Value> func = BuiltinFunctions.Lookup(app.Name);
            return func(arg0, arg1);
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
            return new FunValue(fun);
        }

        Value Core.IExprVisitor<Value>.Visit(Core.FunApp funApp)
        {
            FunValue funValue = funApp.Fun.Accept(this) as FunValue;
            if (funValue == null)
                throw new RuntimeTypeError("function expected");
            Value argValue = funApp.Arg.Accept(this);

            return funValue.Apply(argValue);
        }
    }
}
