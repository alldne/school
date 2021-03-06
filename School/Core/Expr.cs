﻿using System;
using System.Collections.Generic;

namespace School.Core
{
    public interface IExprVisitor<R>
    {
        R Visit(Unit unit);
        R Visit(Number number);
        R Visit(Boolean b);
        R Visit(List list);
        R Visit(BinaryOperator app);
        R Visit(IdExpr idExpr);
        R Visit(Program program);
        R Visit(NamedFunAbsList funNamedAbsList);
        R Visit(NamedFunAbs funNamedAbs);
        R Visit(FunAbs funAbs);
        R Visit(FunApp funApp);
        R Visit(IfExpr ifExpr);
        R Visit(ExprList exprs);
    }

    public abstract class Expr
    {
        public abstract R Accept<R>(IExprVisitor<R> visitor);
    }

    public class Program : Expr
    {
        private readonly NamedFunAbsList namedFunAbsList;

        private readonly Expr expr;

        public NamedFunAbsList NamedFunAbsList
        {
            get { return namedFunAbsList; }
        }

        public Expr Expr
        {
            get { return expr; }
        }

        public Program(NamedFunAbsList namedFunAbsList, Expr expr)
        {
            this.namedFunAbsList = namedFunAbsList;
            this.expr = expr;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class NamedFunAbsList : Expr
    {
        private readonly IReadOnlyList<Expr> elements;

        public IReadOnlyList<Expr> Elements
        {
            get { return elements; }
        }

        public NamedFunAbsList(IReadOnlyList<Expr> elements)
        {
            this.elements = elements;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ExprList : Expr
    {
        private readonly IReadOnlyList<Expr> exprs;

        public IReadOnlyList<Expr> Exprs
        {
            get { return exprs; }
        }

        public ExprList(IReadOnlyList<Expr> exprs)
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

    public class NamedFunAbs : Expr
    {
        private readonly Id nameId;
        private readonly FunAbs funAbs;

        public Id NameId
        {
            get { return nameId; }
        }

        public FunAbs FunAbs
        {
            get { return funAbs; }
        }

        public NamedFunAbs(Id nameId, FunAbs funAbs)
        {
            this.nameId = nameId;
            this.funAbs = funAbs;
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
        private readonly IReadOnlyList<Expr> elements;

        public IReadOnlyList<Expr> Elements
        {
            get { return elements; }
        }

        public List(IReadOnlyList<Expr> elements)
        {
            this.elements = elements;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class BinaryOperator : Expr
    {
        private readonly string name;

        private readonly Expr left;
        private readonly Expr right;

        public string Name
        {
            get { return name; }
        }

        public Expr Left
        {
            get { return left; }
        }

        public Expr Right
        {
            get { return right; }
        }
            
        public BinaryOperator(string name, Expr left, Expr right)
        {
            this.name = name;
            this.left = left;
            this.right = right;
        }

        public override R Accept<R>(IExprVisitor<R> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
