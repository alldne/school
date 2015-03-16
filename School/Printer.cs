using System;
using System.Linq;
using System.Text;

namespace School
{
    public class Printer : Surface.IExprVisitor<object>
    {
        private StringBuilder builder = new StringBuilder();

        public Printer() { }

        public string Print(Surface.Expr expr)
        {
            expr.Accept(this);
            return builder.ToString();
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Unit unit)
        {
            builder.Append("()");
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Number number)
        {
            builder.Append(number.Value.ToString());
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Boolean b)
        {
            builder.Append(b.Value.ToString());
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.List list)
        {
            builder.Append("[");
            var e = list.Elements.GetEnumerator();
            if (e.MoveNext())
            {
                var item = e.Current;
                while (e.MoveNext())
                {
                    item.Accept(this);
                    builder.Append(",");
                }
                item.Accept(this); // Last item
            }
            builder.Append("]");
            return null;
        }

        private void PrintBinaryOperator(Surface.BinaryOperator binOp, char opChar)
        {
            builder.Append("(");
            binOp.Left.Accept(this);
            builder.AppendFormat(" {0} ", opChar);
            binOp.Right.Accept(this);
            builder.Append(")");
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Add add)
        {
            PrintBinaryOperator(add, '+');
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Sub sub)
        {
            PrintBinaryOperator(sub, '-');
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Mul mul)
        {
            PrintBinaryOperator(mul, '*');
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Div div)
        {
            PrintBinaryOperator(div, '/');
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.IdExpr idExpr)
        {
            builder.Append(idExpr.Id.ToString());
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.FunAbs funAbs)
        {
            builder.Append("fun ");
            foreach (var argId in funAbs.ArgIds)
            {
                builder.Append(argId);
                builder.Append(" ");
            }
            builder.Append("-> ");
            funAbs.BodyExpr.Accept(this);
            builder.Append(" end");
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.FunApp funApp)
        {
            funApp.Fun.Accept(this);
            builder.Append(" ");
            funApp.Arg.Accept(this);
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.IfExpr ifExpr)
        {
            builder.Append("if ");
            ifExpr.Cond.Accept(this);
            builder.Append(" then ");
            ifExpr.Then.Accept(this);
            builder.Append(" else ");
            ifExpr.Else.Accept(this);
            return null;
        }
    }
}
