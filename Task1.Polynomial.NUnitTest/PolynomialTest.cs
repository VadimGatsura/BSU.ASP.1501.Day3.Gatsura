using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace Task1.Polynomial.NUnitTest {
    [TestFixture]
    public class PolynomialTest {

        public IEnumerable<TestCaseData> TestDatas {
            get {
                yield return new TestCaseData(new[] { 1 }).Returns($"Polynomial: 1");
                yield return new TestCaseData(new[] { 1, 2, 3, 4 }).Returns($"Polynomial: 4*X^(3) + 3*X^(2) + 2*X^(1) + 1");
            }
        }
        [Test, TestCaseSource(nameof(TestDatas))]
        public string ToString_Test(int[] coeffArray) => new Polynomial(coeffArray).ToString();

        public IEnumerable<TestCaseData> ExceptionTestDatas {
            get {
                yield return new TestCaseData(new int[] { }).Throws(typeof(ArgumentException));
                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));
            }
        }
        [Test, TestCaseSource(nameof(ExceptionTestDatas))]
        public void Constructors_Exceptions_Test(int[] array) {
            Polynomial polynomial = new Polynomial(array);
        }

        public IEnumerable<TestCaseData> EqualsTestDatas {
            get {
                yield return new TestCaseData(new Polynomial(new[] { 1, 2, 3 }), new Polynomial(new[] { 4, 5, 6 }));
                yield return new TestCaseData(new Polynomial(new[] { 4, 5, 6, 7 }), new Polynomial(new[] { 4, 5, 6, 7 }));
            }
        }
        [Test, TestCaseSource(nameof(EqualsTestDatas))]
        public void Equals_Test(Polynomial a, Polynomial b) {
            Polynomial c = new Polynomial(new[] { 4, 5, 6, 7 });
            Assert.AreEqual(a.Equals(a), true);
            Assert.AreEqual(a.Equals(b), b.Equals(a));
            Assert.AreEqual(a.Equals(null), false);

            if (a.Equals(b) && b.Equals(c))
                Assert.AreEqual(a.Equals(c), true);

            Assert.AreEqual(a.Equals((object)a), true);
            Assert.AreEqual(a.Equals((object)b), b.Equals((object)a));
            Assert.AreEqual(a.Equals((object)null), false);

            if (a.Equals((object)b) && b.Equals((object)c))
                Assert.AreEqual(a.Equals((object)c), true);
        }
        [Test, TestCaseSource(nameof(EqualsTestDatas))]
        public void EqualsOperator_Test(Polynomial a, Polynomial b) {
            Assert.AreEqual(a == a, true);
            Assert.AreEqual(a == b, b == a);
            Assert.AreEqual(a == null, false);
        }

        public IEnumerable<TestCaseData> AddTwoDatas {
            get {
                yield return new TestCaseData( new Polynomial(new[] {1, 2, 3, 4}), new Polynomial(new[] {1, 0, -2}) ).Returns(new Polynomial(new[] {2, 2, 1, 4}));
                yield return new TestCaseData(new Polynomial(new[] {1, 2, 3, 4}), new Polynomial(new[] {0, 0, 0, 0, 5})).Returns(new Polynomial(new[] { 1, 2, 3, 4, 5 }));
            }
        }
        [Test, TestCaseSource(nameof(AddTwoDatas))]
        public Polynomial Add_TwoArgs_Test(Polynomial a, Polynomial b) => Polynomial.Add(a, b);

        public IEnumerable<TestCaseData> AddThreeDatas {
            get {
                yield return new TestCaseData(new Polynomial(new[] { 1, 2, 3, 4 }), new Polynomial(new[] { 1, 0, -2 })).Returns(new Polynomial(new[] { 2, 2, 1, 4 }));
                yield return new TestCaseData(new Polynomial(new[] { 1, 2, 3, 4 }), new Polynomial(new[] { 0, 0, 0, 0, 5 })).Returns(new Polynomial(new[] { 1, 2, 3, 4, 5 }));
            }
        }
        [Test, TestCaseSource(nameof(AddTwoDatas))]
        public Polynomial Add_ThreeArgs_Test(Polynomial a, Polynomial b, Polynomial c) => Polynomial.Add(a, b, c);

        public Polynomial Sub_Test(Polynomial a, Polynomial b)  => a - b;

        public Polynomial Multiply_Test(params Polynomial[] array) => Polynomial.Multiply(array);
    }
}
