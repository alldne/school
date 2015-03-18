using System;

namespace School.Test
{
    using NUnit.Framework;
    using School.Evaluator;

    [TestFixture]
    public class SchoolTest
    {
        private Value Evaluate(string code)
        {
            var evaluator = new Evaluator();
            return evaluator.Evaluate(code);
        }

        [Test]
        public void TestBooleanLiteral()
        {
            BooleanValue trueValue = Evaluate("true") as BooleanValue;
            Assert.NotNull(trueValue);
            Assert.AreEqual(true, trueValue.Value);

            BooleanValue falseValue = Evaluate("false") as BooleanValue;
            Assert.NotNull(falseValue);
            Assert.AreEqual(false, falseValue.Value);
        }

        [Test]
        public void TestIntLiteral()
        {
            IntValue value = Evaluate("1") as IntValue;
            Assert.NotNull(value);
            Assert.AreEqual(1, value.Value);
        }

        [Test]
        public void TestAdd()
        {
            IntValue value1 = Evaluate("1 + 2") as IntValue;
            Assert.NotNull(value1);
            Assert.AreEqual(3, value1.Value);

            IntValue value2 = Evaluate("1 + 2 + 3") as IntValue;
            Assert.NotNull(value2);
            Assert.AreEqual(6, value2.Value);
        }

        [Test]
        public void TestSub()
        {
            IntValue value1 = Evaluate("2 - 1") as IntValue;
            Assert.NotNull(value1);
            Assert.AreEqual(1, value1.Value);

            IntValue value2 = Evaluate("3 - 2 - 1") as IntValue;
            Assert.NotNull(value2);
            Assert.AreEqual(0, value2.Value);
        }

        [Test]
        public void TestMul()
        {
            IntValue value1 = Evaluate("2 * 3") as IntValue;
            Assert.NotNull(value1);
            Assert.AreEqual(6, value1.Value);

            IntValue value2 = Evaluate("2 * 3 * 4") as IntValue;
            Assert.NotNull(value2);
            Assert.AreEqual(24, value2.Value);
        }

        [Test]
        public void TestDiv()
        {
            IntValue value1 = Evaluate("6 / 2") as IntValue;
            Assert.NotNull(value1);
            Assert.AreEqual(3, value1.Value);

            IntValue value2 = Evaluate("12 / 4 / 3") as IntValue;
            Assert.NotNull(value2);
            Assert.AreEqual(1, value2.Value);
        }

        [Test]
        public void TestArithOperatorPrecedence()
        {
            IntValue value1 = Evaluate("2 + 3 * 4") as IntValue;
            Assert.NotNull(value1);
            Assert.AreEqual(14, value1.Value);

            IntValue value2 = Evaluate("8 - 4 / 2") as IntValue;
            Assert.NotNull(value2);
            Assert.AreEqual(6, value2.Value);
        }

        [Test]
        [ExpectedException(typeof(DivideByZeroException))]
        public void TestDivByZero()
        {
            Evaluate("1 / 0");
        }

        [Test]
        public void TestFunAbs()
        {
            FunValue fun = Evaluate("fun x -> x end") as FunValue;
            Assert.NotNull(fun);
            IntValue arg = new IntValue(1);
            IntValue result = fun.Apply(arg) as IntValue;
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void TestProgramWithNoExpr()
        {
            Value value1 = Evaluate("") as Value;
            Assert.NotNull(value1);
            Assert.AreEqual(UnitValue.Singleton, value1);

            Value value2 = Evaluate("let add x y = x + y end") as Value;
            Assert.NotNull(value2);
            Assert.AreEqual(UnitValue.Singleton, value2);
        }

        [Test]
        public void TestNamedFunAbs()
        {
            IntValue value = Evaluate(
                @"let add x y = x + y end
                  add 1 2") as IntValue;
            Assert.NotNull(value);
            Assert.AreEqual(3, value.Value);
        }

        [Test]
        public void TestTwoNamedFunAbs()
        {
            IntValue value = Evaluate(
                @"let add x y = x + y end
                  let sub x y = x - y end
                  sub (add 2 3) (add 1 2)") as IntValue;
            Assert.NotNull(value);
            Assert.AreEqual(2, value.Value);
        }

        [Test]
        public void TestFunApp()
        {
            IntValue value = Evaluate("(fun x -> x end) 1") as IntValue;
            Assert.NotNull(value);
            Assert.AreEqual(1, value.Value);
        }

        [Test]
        public void TestMultiArgFun()
        {
            IntValue value = Evaluate("(fun x y -> x + y end) 1 2") as IntValue;
            Assert.NotNull(value);
            Assert.AreEqual(3, value.Value);
        }

        [Test]
        public void TestCurryingFun()
        {
            IntValue value = Evaluate("(fun x -> fun y -> x + y end end) 1 2") as IntValue;
            Assert.NotNull(value);
            Assert.AreEqual(3, value.Value);

            FunValue fun = Evaluate("(fun x -> fun y -> x + y end end) 1") as FunValue;
            Assert.NotNull(fun);
            IntValue arg = new IntValue(2);
            IntValue result = fun.Apply(arg) as IntValue;
            Assert.NotNull(result);
            Assert.AreEqual(3, result.Value);
        }

        [Test]
        public void TestIf()
        {
            IntValue value1 = Evaluate("if true then 1 else 2") as IntValue;
            Assert.NotNull(value1);
            Assert.AreEqual(1, value1.Value);

            IntValue value2 = Evaluate("if false then 1 else 2") as IntValue;
            Assert.NotNull(value2);
            Assert.AreEqual(2, value2.Value);
        }

        [Test]
        public void TestUnit()
        {
            Value value = Evaluate("()");
            Assert.AreEqual(UnitValue.Singleton, value);
        }

        [Test]
        public void TestUnitSingleton()
        {
            Value value = Evaluate("()");
            Assert.AreSame(UnitValue.Singleton, value);
        }

        [Test]
        public void TestSemicolonOperator()
        {
            IntValue value = Evaluate("1;2;3") as IntValue;
            Assert.NotNull(value);
            Assert.AreEqual(3, value.Value);
        }

        [Test]
        public void TestEmptyList()
        {
            ListValue value = Evaluate("[]") as ListValue;
            Assert.NotNull(value);
            Assert.AreEqual(0, value.Elements.Count);
        }

        [Test]
        public void TestList()
        {
            ListValue value = Evaluate("[1,2,3]") as ListValue;
            Assert.NotNull(value);
            Assert.AreEqual(1, (value.Elements[0] as IntValue).Value);
            Assert.AreEqual(2, (value.Elements[1] as IntValue).Value);
            Assert.AreEqual(3, (value.Elements[2] as IntValue).Value);
        }

        [Test]
        public void TestHeterogeneousList()
        {
            ListValue value = Evaluate("[1,false,()]") as ListValue;
            Assert.NotNull(value);
            Assert.AreEqual(1, (value.Elements[0] as IntValue).Value);
            Assert.AreEqual(false, (value.Elements[1] as BooleanValue).Value);
            Assert.AreEqual(UnitValue.Singleton, value.Elements[2] as UnitValue);
        }
    }
}
