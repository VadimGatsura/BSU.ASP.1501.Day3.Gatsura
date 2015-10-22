using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Task1.Polynomial {
    public class Polynomial {

        #region Public Fields
        public int Degree { get; }
        #endregion

        #region Private Fields
        private readonly int[] m_CoefficientArray;

        private int this[int index] => m_CoefficientArray[index];

        #endregion

        #region Constructors
        public Polynomial(int[] coefficientArray) {
            if (coefficientArray == null)
                throw new ArgumentNullException(nameof(coefficientArray));
            if( coefficientArray.Length == 0 ) {
                m_CoefficientArray = new[] {0};
                Degree = 0;
                return;
            }


            m_CoefficientArray = new int[coefficientArray.Length];
            for (int i = 0; i < coefficientArray.Length; i++)
                m_CoefficientArray[i] = coefficientArray[i];

            for (int i = m_CoefficientArray.Length - 1; i >= 0; i--)
                if( this[ i ] != 0 ) {
                    Degree = i;
                    break;
                }

        }
        public Polynomial(Polynomial obj) {
            if(obj == null)
                throw new ArgumentNullException(nameof(obj));
            m_CoefficientArray = new int[obj.Degree + 1];
            for (int i = 0; i < obj.m_CoefficientArray.Length; i++)
                m_CoefficientArray[i] = obj.m_CoefficientArray[i];

            for (int i = m_CoefficientArray.Length - 1; i >= 0; i--)
                if( this[ i ] != 0 ) {
                    Degree = i;
                    break;
                }
        }
        #endregion

        #region Public Methods

        public override string ToString() => $"Polynomial: {GetStringRepresentation()}";

        public override bool Equals(object obj) {
            Polynomial equalObject = obj as Polynomial;
            if ((object) equalObject == null)
                return false;

            return EqualsCoefficientArrays(m_CoefficientArray, ((Polynomial)obj).m_CoefficientArray);
        }

        public bool Equals(Polynomial obj) {
            if ((object)obj == null)
                return false;

            return EqualsCoefficientArrays(m_CoefficientArray, obj.m_CoefficientArray);
        }

        public override int GetHashCode() {
            unchecked {
                return m_CoefficientArray.Aggregate(17, (current, item) => current * 23 + item.GetHashCode());
            }
        }

        /// <summary>
        /// Add monomial to polynomial
        /// </summary>
        /// <param name="coefficient">Coefficient of monomial</param>
        /// <param name="degree">Degree of monomial</param>
        /// <returns>New Polynomial</returns>
        public Polynomial Add(int coefficient, int degree) {
            int tDegree = Math.Max(Degree, degree);
            int[] tempArray = new int[tDegree + 1];

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
        public Polynomial Multiply(int coefficient, int degree) {
            int tDegree = Degree + degree;
            int[] tempArray = new int[tDegree + 1];

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
            int[] tempArray = new int[newDegree + 1];

            for (int i = 0; i <= newDegree; i++) {
                int temp = 0;
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
        public static Polynomial Subtraction(Polynomial a, Polynomial b) {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            int newDegree = Math.Max(a.Degree, b.Degree);
            int[] tempArray = new int[newDegree + 1];

            for (int i = 0; i <= newDegree; i++) {
                if (i <= a.Degree && i <= b.Degree)
                    tempArray[i] = a[i] - b[i];
                else if (i <= a.Degree)
                    tempArray[i] = 0 - b[i];
                else if (i <= b.Degree)
                    tempArray[i] = a[i];

            }
            return new Polynomial(tempArray);
        }

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

            Polynomial result = new Polynomial(new int[0]);

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
        public static bool operator ==(Polynomial a, Polynomial b) {
            if (object.ReferenceEquals(a, b))
                return true;
            if ((object)a == null || (object)b == null)
                return false;
            return EqualsCoefficientArrays(a.m_CoefficientArray, b.m_CoefficientArray);
        }

        public static bool operator !=(Polynomial a, Polynomial b) => !(a == b);

        public static Polynomial operator +(Polynomial a, Polynomial b) => Add(a, b);

        public static Polynomial operator -(Polynomial a, Polynomial b) => Subtraction(a, b);

        public static Polynomial operator *(Polynomial a, Polynomial b) => Multiply(a, b);
        #endregion

        #region Private Methods

        private string GetStringRepresentation() {
            StringBuilder @return = new StringBuilder();

            for(int i = Degree; i >= 0; i--) {
                if(m_CoefficientArray[i] == 0)
                    continue;
                if (i != m_CoefficientArray.Length - 1)
                    @return.Append(" + ");
                @return.Append(this[i]);
                if(i != 0) 
                    @return.Append($"*X^({i})");
            }
            return @return.ToString();
        }

        private static bool EqualsCoefficientArrays(int[] array1, int[] array2) {
            IStructuralEquatable array = array1;

            return array.Equals(array2, StructuralComparisons.StructuralEqualityComparer);
        }
        #endregion
    }
}
