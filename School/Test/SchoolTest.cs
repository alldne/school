﻿using System;

namespace School
{
    using NUnit.Framework;

    [TestFixture]
    public class SchoolTest
    {
        private Value Evaluate(string code)
        {
            var lexer = new SchoolLexer(code);
            var parser = new SchoolParser(lexer);
            Surface.Expr expr = parser.Parse();

            var desugarer = new Desugarer();
            Core.Expr coreExpr = desugarer.Desugar(expr);

            var evaluator = new Evaluator();
            return evaluator.Evaluate(coreExpr);
        }

        [Test]
        public void TestBooleanLiteral()
        {
            BooleanValue trueValue = Evaluate("true") as BooleanValue;
            Assert.AreEqual(trueValue.Value, true);

            BooleanValue falseValue = Evaluate("false") as BooleanValue;
            Assert.AreEqual(falseValue.Value, false);
        }

        [Test]
        public void TestIntLiteral()
        {
            IntValue value = Evaluate("1") as IntValue;
            Assert.AreEqual(value.Value, 1);
        }

        [Test]
        public void TestAdd()
        {
            IntValue value1 = Evaluate("1 + 2") as IntValue;
            Assert.AreEqual(value1.Value, 3);

            IntValue value2 = Evaluate("1 + 2 + 3") as IntValue;
            Assert.AreEqual(value2.Value, 6);
        }

        [Test]
        public void TestSub()
        {
            IntValue value1 = Evaluate("2 - 1") as IntValue;
            Assert.AreEqual(value1.Value, 1);

            IntValue value2 = Evaluate("3 - 2 - 1") as IntValue;
            Assert.AreEqual(value2.Value, 0);
        }

        [Test]
        public void TestMul()
        {
            IntValue value1 = Evaluate("2 * 3") as IntValue;
            Assert.AreEqual(value1.Value, 6);

            IntValue value2 = Evaluate("2 * 3 * 4") as IntValue;
            Assert.AreEqual(value2.Value, 24);
        }

        [Test]
        public void TestDiv()
        {
            IntValue value1 = Evaluate("6 / 2") as IntValue;
            Assert.AreEqual(value1.Value, 3);

            IntValue value2 = Evaluate("12 / 4 / 3") as IntValue;
            Assert.AreEqual(value2.Value, 1);
        }

        [Test]
        [ExpectedException(typeof(DivideByZeroException))]
        public void TestDivByZero()
        {
            Evaluate("1 / 0");
        }
    }
}
