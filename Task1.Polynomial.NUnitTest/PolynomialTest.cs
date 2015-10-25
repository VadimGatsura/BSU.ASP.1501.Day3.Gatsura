using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Task1.Polynomial.NUnitTest {
    [TestFixture]
    public class PolynomialTest {

        public IEnumerable<TestCaseData> NormalizeTestDatas {
            get {
                yield return new TestCaseData(new double[] { 1, 0, 0, 0, 0 }, new double[] {1});
                yield return new TestCaseData(new double[] { 1, 2, 3, 4 }, new double[] { 1, 2, 3, 4 });
            }
        }

        [Test, TestCaseSource(nameof(NormalizeTestDatas))] 
        public void Normalize_Test(double[] coeffArray, double[] normalizeArray) {
            Polynomial a = new Polynomial(coeffArray), b = new Polynomial(normalizeArray);
            Assert.AreEqual(a == b, true);
        }


        public IEnumerable<TestCaseData> TestDatas {
            get {
                yield return new TestCaseData(new double[] { 1 }).Returns("Polynomial: 1");
                yield return new TestCaseData(new double[] { 1, 2, 3, 4 }).Returns("Polynomial: 4*X^(3) + 3*X^(2) + 2*X^(1) + 1");
            }
        }
        [Test, TestCaseSource(nameof(TestDatas))]
        public string ToString_Test(double[] coeffArray) => new Polynomial(coeffArray).ToString();

        public IEnumerable<TestCaseData> ExceptionTestDatas {
            get {
                yield return new TestCaseData(null).Throws(typeof(ArgumentNullException));
            }
        }
        [Test, TestCaseSource(nameof(ExceptionTestDatas))]
        public void Constructors_Exceptions_Test(double[] array) {
            Polynomial polynomial = new Polynomial(array);
        }

        public IEnumerable<TestCaseData> EqualsTestDatas {
            get {
                yield return new TestCaseData(new Polynomial(new double[] { 1, 2, 3 }), new Polynomial(new double[] { 4, 5, 6 }));
                yield return new TestCaseData(new Polynomial(new double[] { 4, 5, 6, 7 }), new Polynomial(new double[] { 4, 5, 6, 7 }));
            }
        }
        [Test, TestCaseSource(nameof(EqualsTestDatas))]
        public void Equals_Test(Polynomial a, Polynomial b) {
            Polynomial c = new Polynomial(new double[] { 4, 5, 6, 7 });
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
                yield return new TestCaseData( new Polynomial(new double[] {1, 2, 3, 4}), new Polynomial(new double[] {1, 0, -2}) ).Returns(new Polynomial(new double[] {2, 2, 1, 4}));
                yield return new TestCaseData(new Polynomial(new double[] {1, 2, 3, 4}), new Polynomial(new double[] {0, 0, 0, 0, 5})).Returns(new Polynomial(new double[] { 1, 2, 3, 4, 5 }));
            }
        }
        [Test, TestCaseSource(nameof(AddTwoDatas))]
        public Polynomial Add_TwoArgs_Test(Polynomial a, Polynomial b) => Polynomial.Add(a, b);

        public IEnumerable<TestCaseData> AddThreeDatas {
            get {
                yield return new TestCaseData(new Polynomial(new double[] { 1, 2, 3, 4 }), new Polynomial(new double[] { 1, 0, -2 }), new Polynomial(new double[] { 1, 0, -2 })).Returns(new Polynomial(new double[] { 3, 2, -1, 4 }));
                yield return new TestCaseData(new Polynomial(new double[] { 1, 2, 3, 4 }), new Polynomial(new double[] { 1, 0, -2 }),  new Polynomial(new double[] { 0, 0, 0, 0, 5 })).Returns(new Polynomial(new double[] { 2, 2, 1, 4, 5 }));
            }
        }
        [Test, TestCaseSource(nameof(AddThreeDatas))]
        public Polynomial Add_ThreeArgs_Test(Polynomial a, Polynomial b, Polynomial c) => Polynomial.Add(a, b, c);

        public IEnumerable<TestCaseData> AddMonomialDatas {
            get {
                yield return new TestCaseData(new Polynomial(new double[] { 1, 2, 3, 4 }), 2, 0).Returns(new Polynomial(new double[] { 3, 2, 3, 4 }));
                yield return new TestCaseData(new Polynomial(new double[] { 1, 2, 3, 4 }), -10, 5).Returns(new Polynomial(new double[] { 1, 2, 3, 4, 0, -10 }));
            }
        }
        [Test, TestCaseSource(nameof(AddMonomialDatas))]
        public Polynomial AddMonomial_Test(Polynomial a, int coeff, int degree) => a.Add(coeff, degree);

        public IEnumerable<TestCaseData> SubDatas {
            get {
                yield return new TestCaseData(new Polynomial(new double[] { 1, 2, 3, 4 }), new Polynomial(new double[] { 1, 0, -2 })).Returns(new Polynomial(new double[] { 0, 2, 5, 4 }));
                yield return new TestCaseData(new Polynomial(new double[] { 1, 2, 3, 4 }), new Polynomial(new double[] { 0, 0, 0, 0, 5 })).Returns(new Polynomial(new double[] { 1, 2, 3, 4, -5 }));
            }
        }
        [Test, TestCaseSource(nameof(SubDatas))]
        public Polynomial Sub_Test(Polynomial a, Polynomial b)  => a - b;

        public IEnumerable<TestCaseData> MultiplyDatas {
            get {
                yield return new TestCaseData(new Polynomial(new double[] { 0, 9, 2 }), new Polynomial(new double[] {4, 3})).Returns(new Polynomial(new double[] { 0, 36, 35, 6}));
                yield return new TestCaseData(new Polynomial(new double[] { -1, 9, 2 }), new Polynomial(new double[] {2, 1, 5 })).Returns(new Polynomial(new double[] { -2, 17, 8, 47, 10 }));
                //yield return new TestCaseData(new Polynomial(new[] { 1, 2, 3, 4 }), -10, 5).Returns(new Polynomial(new[] { 1, 2, 3, 4, 0, -10 }));
            }
        }
        [Test, TestCaseSource(nameof(MultiplyDatas))]
        public Polynomial Multiply_Test(Polynomial a, Polynomial b) => Polynomial.Multiply(a, b);

        public IEnumerable<TestCaseData> MultiplyMonomialDatas {
            get {
                yield return new TestCaseData(new Polynomial(new double[] { 0, 4, 3 }), 5, 3).Returns(new Polynomial(new double[] { 0, 0, 0, 0, 20, 15 }));
                yield return new TestCaseData(new Polynomial(new double[] { 0, 4, 3 }), -5, 3).Returns(new Polynomial(new double[] { 0, 0, 0, 0, -20, -15 }));
            }
        }
        [Test, TestCaseSource(nameof(MultiplyMonomialDatas))]
        public Polynomial MultiplyMonomial_Test(Polynomial a, int coeff, int degree) => a.Multiply(coeff, degree);

        public IEnumerable<TestCaseData> CalculateDatas {
            get {
                yield return new TestCaseData(new Polynomial(new double[] { 0, 4, 3 }), 5).Returns(95);
                yield return new TestCaseData(new Polynomial(new double[] { 0, 4, 3 }), -5).Returns(55);
            }
        }
        [Test, TestCaseSource(nameof(CalculateDatas))]
        public double CalculatePolynomial_Test(Polynomial a, double variable) => a.Calculate(variable);
    }
}
