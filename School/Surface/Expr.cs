using System;
using System.Collections.Generic;

namespace School.Surface
{
    public interface IExprVisitor<R>
    {
        R Visit(Unit unit);
        R Visit(Number number);
        R Visit(Boolean b);
        R Visit(List list);
        R Visit(Add add);
        R Visit(Sub sub);
        R Visit(Mul mul);
        R Visit(Div div);
        R Visit(IdExpr idExpr);
        R Visit(FunAbs funAbs);
        R Visit(FunApp funApp);
        R Visit(IfExpr ifExpr);
        R Visit(ExprList exprs);
    }

    public abstract class Expr
    {
        public abstract R Accept<R>(IExprVisitor<R> visitor);
    }

    public class ExprList : Expr
    {
        private IList<Expr> exprs;

        public IList<Expr> Exprs
        {
            get { return exprs; }
        }

        public ExprList(IList<Expr> exprs)
        {
            this.exprs = exprs;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
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
        private readonly IList<Id> argIds;
        private readonly Expr bodyExpr;

        public IList<Id> ArgIds
        {
            get { return argIds; }
        }

        public Expr BodyExpr
        {
            get { return bodyExpr; }
        }
            
        public FunAbs(IList<Id> argIds, Expr bodyExpr)
        {
            this.argIds = argIds;
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

    public class IfExpr : Expr
    {
        private readonly Expr condExpr;
        private readonly Expr thenExpr;
        private readonly Expr elseExpr;

        public Expr Cond
        {
            get { return condExpr; }
        }

        public Expr Then
        {
            get { return thenExpr; }
        }

        public Expr Else
        {
            get { return elseExpr; }
        }

        public IfExpr(Expr condExpr, Expr thenExpr, Expr elseExpr)
        {
            this.condExpr = condExpr;
            this.thenExpr = thenExpr;
            this.elseExpr = elseExpr;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Unit : Expr
    {
        public static readonly Unit Singleton = new Unit();

        private Unit()
        {
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

    public class List : Expr
    {
        private readonly IList<Expr> elements;

        public IList<Expr> Elements
        {
            get { return elements; }
        }

        public List(IList<Expr> elements)
        {
            this.elements = elements;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public abstract class BinaryOperator : Expr
    {
        private readonly Expr right;
        private readonly Expr left;

        public Expr Right
        {
            get { return right; }
        }

        public Expr Left
        {
            get { return left; }
        }

        public BinaryOperator(Expr left, Expr right)
        {
            this.right = right;
            this.left = left;
        }
    }

    public class Add : BinaryOperator
    {
        public Add(Expr left, Expr right) : base(left, right) { }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Sub : BinaryOperator
    {
        public Sub(Expr left, Expr right) : base(left, right) { }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Mul : BinaryOperator
    {
        public Mul(Expr left, Expr right) : base(left, right) { }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Div : BinaryOperator
    {
        public Div(Expr left, Expr right) : base(left, right) { }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }
}

