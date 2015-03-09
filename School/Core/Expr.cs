using System;

namespace School.Core
{
    public interface IExprVisitor<R>
    {
        R Visit(Number number);
        R Visit(Boolean b);
        R Visit(BuiltinFunApp app);
        R Visit(IdExpr idExpr);
        R Visit(FunAbs funAbs);
        R Visit(FunApp funApp);
    }

    public abstract class Expr
    {
        public abstract R Accept<R>(IExprVisitor<R> visitor);
    }

    public class IdExpr : Expr 
    {
        private readonly Id id;

        public Id Id
        {
            get { return id; }
        }

        public IdExpr(Id id)
        {
            this.id = id;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class FunAbs : Expr
    {
        private readonly Id argId;
        private readonly Expr bodyExpr;

        public Id ArgId
        {
            get { return argId; }
        }

        public Expr BodyExpr
        {
            get { return bodyExpr; }
        }

        public FunAbs(Id argId, Expr bodyExpr)
        {
            this.argId = argId;
            this.bodyExpr = bodyExpr;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class FunApp : Expr
    {
        private readonly Expr fun;
        private readonly Expr arg;

        public Expr Fun
        {
            get { return fun; }
        }

        public Expr Arg
        {
            get { return arg; }
        }

        public FunApp(Expr fun, Expr arg)
        {
            this.fun = fun;
            this.arg = arg;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Number : Expr
    {
        private readonly int value;

        public int Value
        {
            get { return value; }
        }

        public Number(int value)
        {
            this.value = value;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Boolean : Expr
    {
        private readonly bool value;

        public bool Value
        {
            get { return value; }
        }

        public Boolean(bool value)
        {
            this.value = value;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class BuiltinFunApp : Expr
    {
        private readonly string name;

        private readonly Expr[] args;

        public string Name
        {
            get { return name; }
        }

        public Expr[] Args
        {
            get { return args; }
        }
            
        public BuiltinFunApp(string name, Expr[] args)
        {
            this.name = name;
            this.args = args;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
