using System;

namespace School
{
    public class Printer : Surface.IExprVisitor<object>
    {
        public Printer() { }

        public void Print(Surface.Expr expr)
        {
            expr.Accept(this);
        }

        object Surface.IExprVisitor<object>.Visit(Surface.Number number)
        {
            Console.Write(number.Value);
            return null;
        }

        private void PrintBinaryOperator(Surface.BinaryOperator binOp, char opChar)
        {
            Console.Write("(");
            binOp.Left.Accept(this);
            Console.Write(" {0} ", opChar);
            binOp.Right.Accept(this);
            Console.Write(")");
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
            Console.Write(idExpr.Id);
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.FunAbs funAbs)
        {
            Console.Write("fun ");
            foreach (var argId in funAbs.ArgIds)
            {
                Console.Write(argId);
                Console.Write(" ");
            }
            Console.Write("-> ");
            funAbs.BodyExpr.Accept(this);
            Console.Write(" end");
            return null;
        }

        object Surface.IExprVisitor<object>.Visit(Surface.FunApp funApp)
        {
            funApp.Fun.Accept(this);
            Console.Write(" ");
            funApp.Arg.Accept(this);
            return null;
        }
    }
}
