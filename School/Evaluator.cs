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

    public static class BuiltinFunctions 
    {
        public static Value Add(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value + bValue.Value);
        }

        public static Value Sub(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value - bValue.Value);
        }

        public static Value Mul(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value * bValue.Value);
        }

        public static Value Div(Value a, Value b)
        {
            IntValue aValue = a as IntValue;
            IntValue bValue = b as IntValue;
            if (aValue == null || bValue == null)
                throw new RuntimeTypeError("int expected");

            return new IntValue(aValue.Value / bValue.Value);
        }
    }

    public class Evaluator : Core.IExprVisitor<Value>
    {
        private static readonly Dictionary<string, Func<Value, Value, Value>> builtinFunctions = new Dictionary<string, Func<Value, Value, Value>>
        {
            { "add", BuiltinFunctions.Add },
            { "sub", BuiltinFunctions.Sub },
            { "mul", BuiltinFunctions.Mul },
            { "div", BuiltinFunctions.Div }
        };

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

        Value Core.IExprVisitor<Value>.Visit(Core.BuiltinFunApp app)
        {
            // FIXME: Currently, we assume that all builtin functions are binary.
            Value arg0 = app.Args[0].Accept(this);
            Value arg1 = app.Args[1].Accept(this);

            Func<Value, Value, Value> func = builtinFunctions[app.Name];
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
