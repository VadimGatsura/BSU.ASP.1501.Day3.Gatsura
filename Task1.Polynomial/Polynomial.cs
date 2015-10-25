using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Task1.Polynomial {
    public sealed class Polynomial {

        #region Static Members

        private static double Epsilon { get; }

        static Polynomial() {
            //double epsilon;
            //Epsilon = double.TryParse(System.Configuration.ConfigurationManager.AppSettings["epsilon"], out epsilon) ? epsilon : double.Epsilon; //NEED EXE Application
            Epsilon = 0.000001;
        }

        private static double[] NormalizePolynomialArray(double[] array) {
            int size = 0;
            for (int i = array.Length - 1; i >= 0; i--) {
                if (Math.Abs(array[i]) > Epsilon) {
                    size = i + 1;
                    break;
                }
            }

            double[] destArray = new double[size];
            Array.Copy(array, destArray, size);
            return destArray;

        }

        #endregion

        #region Instance members

        #region Public Fields
        /// <summary>
        /// Polynomial's degree <remarks>The maximum degree of lhs variable</remarks>
        /// </summary>
        public int Degree { get; }
        #endregion

        #region Private Fields
        private readonly double[] m_CoefficientArray;

        private double this[int index] {
            get {
                if(index < 0 || index > Degree)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return m_CoefficientArray[index];
            }
        } 

        #endregion

        #region Constructors

        public Polynomial(double[] coefficientArray) {
            if (coefficientArray == null)
                throw new ArgumentNullException(nameof(coefficientArray));

            double[] tempArray = NormalizePolynomialArray(coefficientArray);
            if(tempArray.Length == 0 ) {
                m_CoefficientArray = new[] {.0};
                Degree = 0;
                return;
            }
            m_CoefficientArray = new double[tempArray.Length];
            for (int i = 0; i < tempArray.Length; i++)
                m_CoefficientArray[i] = tempArray[i];

            Degree = m_CoefficientArray.Length - 1;
        }
        public Polynomial(Polynomial obj): this(obj.m_CoefficientArray) { }
        #endregion

        #region Public Methods

        public override string ToString() => $"Polynomial: {GetStringRepresentation()}";

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && EqualsCoefficientArrays(m_CoefficientArray, ((Polynomial)obj).m_CoefficientArray);
        }

        public bool Equals(Polynomial obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return EqualsCoefficientArrays(m_CoefficientArray, obj.m_CoefficientArray);
        }
        public override int GetHashCode() {
            unchecked {
                return m_CoefficientArray.Aggregate(17, (current, item) => current * 23 + item.GetHashCode());
            }
        }
        /// <summary>
        /// Calculate the value of lhs polynomial, if the variable is value
        /// </summary>
        /// <param name="value">Polynomial's variable</param>
        /// <returns>Value of polynomial</returns>
        public double Calculate(double value) {
            double result = 0;
            for( int i = 0; i <= Degree; i++ ) 
                result += Math.Pow(value, i) * this[ i ];
            
            return result;
        }
        /// <summary>
        /// Add monomial to polynomial
        /// </summary>
        /// <param name="coefficient">Coefficient of monomial</param>
        /// <param name="degree">Degree of monomial</param>
        /// <returns>New Polynomial</returns>
        public Polynomial Add(double coefficient, int degree) {
            int tDegree = Math.Max(Degree, degree);
            double[] tempArray = new double[tDegree + 1];

            if(degree > Degree) {
                for (int i = 0; i <= Degree; i++)
                    tempArray[i] = this[i];
                for (int i = Degree + 1; i < degree; i++)
                    tempArray[i] = 0;

                tempArray[degree] = coefficient;
            } else {
                for (int i = 0; i <= Degree; i++) {
                    tempArray[i] = this[i];
                    if (degree == i)
                        tempArray[i] += coefficient;
                }
            }

            return new Polynomial(tempArray);
        }
        /// <summary>
        /// Myltiply polynomial on monomial
        /// </summary>
        /// <param name="coefficient">Coefficient of monomial</param>
        /// <param name="degree">Degree of monomial</param>
        /// <returns>New polynomial</returns>
        public Polynomial Multiply(double coefficient, int degree) {
            int tDegree = Degree + degree;
            double[] tempArray = new double[tDegree + 1];

            for( int i = 0; i <= tDegree; i++ ) {
                if( degree > i )
                    tempArray[ i ] = 0;
                else
                    tempArray[ i ] = this[ i - degree] * coefficient;
            }

            return new Polynomial(tempArray);
        }
        /// <summary>
        /// Calculate the sum of two polynomials 
        /// </summary>
        /// <param name="a">The first summand</param>
        /// <param name="b">The second summand</param>
        /// <returns>The sum of two polynomials</returns>
        public static Polynomial Add(Polynomial a, Polynomial b) {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (b == null)
                throw new ArgumentNullException(nameof(b));
            int newDegree = Math.Max(a.Degree, b.Degree);
            double[] tempArray = new double[newDegree + 1];

            for (int i = 0; i <= newDegree; i++) {
                double temp = 0;
                if (i <= a.Degree)
                    temp += a[i];
                if (i <= b.Degree)
                    temp += b[i];
                tempArray[i] = temp;
            }
            return new Polynomial(tempArray);
        }
        /// <summary>
        /// Calculate the sum of three polynomials 
        /// </summary>
        /// <param name="a">The first summand</param>
        /// <param name="b">The second summand</param>
        /// <param name="c">The third summand</param>
        /// <returns>The sum of three polynomials</returns>
        public static Polynomial Add(Polynomial a, Polynomial b, Polynomial c) => Add(Add(a, b), c);
        public static Polynomial Add(params Polynomial[] array) {
            if(array == null)    
                throw new ArgumentNullException(nameof(array));
            switch(array.Length) {
                case 0:
                    throw new ArgumentException($"Empty argument: {nameof(array)}");
                case 1:
                    return array[0];
                default:
                    Polynomial result = Add(array[0], array[1]);
                    for (int i = 2; i < array.Length - 1; i++)
                        result = Add(result, array[i]);

                    return result;
            }  
        }
        /// <summary>
        /// Calculate the diffrence of two polynomials
        /// </summary>
        /// <param name="a">The minuend</param>
        /// <param name="b">The subtrahend</param>
        /// <returns></returns>
        public static Polynomial Subtraction(Polynomial a, Polynomial b) => a + (-b);
        /// <summary>
        /// Multiplies two polynomials
        /// </summary>
        /// <param name="a">The first multiplier</param>
        /// <param name="b">The second multiplier</param>
        /// <returns></returns>
        public static Polynomial Multiply(Polynomial a, Polynomial b) {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            Polynomial result = new Polynomial(new double[0]);

            for( int i = 0; i <= a.Degree ; i++ ) 
                result += b.Multiply(a[i], i);
            
            return result;
        }
        /// <summary>
        /// Multiplies two polynomials
        /// </summary>
        /// <param name="a">The first multiplier</param>
        /// <param name="b">The second multiplier</param>
        /// <param name="c">The third multiplier</param>
        /// <returns></returns>
        public static Polynomial Multiply(Polynomial a, Polynomial b, Polynomial c) => Multiply(Multiply(a, b), c);
        public static Polynomial Multiply(params Polynomial[] array) {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            switch (array.Length) {
                case 0:
                    throw new ArgumentException($"Empty argument: {nameof(array)}");
                case 1:
                    return array[0];
                default:
                    Polynomial result = Multiply(array[0], array[1]);
                    for (int i = 2; i < array.Length - 1; i++)
                        result = Multiply(result, array[i]);

                    return result;
            }
        }
        #endregion
        
        #region Operators
        public static bool operator ==(Polynomial lhs, Polynomial rhs) {
            if (ReferenceEquals(lhs, rhs)) return true;
            if ((object)lhs == null || (object)rhs == null)
                return false;
            return EqualsCoefficientArrays(lhs.m_CoefficientArray, rhs.m_CoefficientArray);
        }

        public static bool operator !=(Polynomial lhs, Polynomial rhs) => !(lhs == rhs);

        public static Polynomial operator +(Polynomial lhs, Polynomial rhs) => Add(lhs, rhs);

        public static Polynomial operator -(Polynomial a) {
            if(a == null)
                throw new ArgumentNullException(nameof(a));
            double[] tempArray = new double[a.Degree + 1];
            for( int i = 0; i <= a.Degree; i++)
                tempArray[ i ] = a[ i ] * (-1);
            
            return new Polynomial(tempArray);
        }

        public static Polynomial operator -(Polynomial lhs, Polynomial rhs) => Subtraction(lhs, rhs);

        public static Polynomial operator *(Polynomial lhs, Polynomial rhs) => Multiply(lhs, rhs);
        #endregion

        #region Private Methods
        private string GetStringRepresentation() {
            StringBuilder @return = new StringBuilder();

            for(int i = Degree; i >= 0; i--) {
                if(Math.Abs(this[i]) < Epsilon)
                    continue;
                if (i != m_CoefficientArray.Length - 1)
                    @return.Append(" + ");
                @return.Append(this[i]);
                if(i != 0) 
                    @return.Append($"*X^({i})");
            }
            return @return.ToString();
        }

        private static bool EqualsCoefficientArrays(double[] array1, double[] array2) {
            IStructuralEquatable array = array1;

            return array.Equals(array2, StructuralComparisons.StructuralEqualityComparer);
        }
        #endregion

        #endregion  
    }
}
