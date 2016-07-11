using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialLibrary
{
    /// <summary>
    /// This class represents polynomial of N-th degree and basical operation to work with it.
    /// </summary>
    public class Polynomial : IComparable, ICloneable, IQueryable
    {
        /// <summary>
        /// Power of polynomial.
        /// </summary>
        public int Power { get; }
        public Expression Expression { get; }
        public Type ElementType { get; }
        public IQueryProvider Provider { get; }

        //private static double accuracy;
        /// <summary>
        /// It's compact representation of polynomial. This array keeps nonzeros elements of polynomial.
        /// </summary>
        private readonly PolynomialElement[] polynomialElements;

        /// <summary>
        /// This structure represents summand of polynomial. 
        /// </summary>
        private struct PolynomialElement
        {
            public int Power { get; set; }
            public double Coefficient { get; set; }
        }

        #region constructors
        /// <summary>
        /// Standard constructor.
        /// </summary>
        public Polynomial() { }

        /// <summary>
        /// This constructor create a polynomial using array of coefficients and array of powers.
        /// </summary>
        /// <param name="coefficientsArray">Array of coefficients of polynomial summands.</param>
        /// <param name="powersArray">Array of powers of polynomial summands.</param>
        public Polynomial(double[] coefficientsArray, int[] powersArray)
        {
            if (coefficientsArray.Length == 0 || powersArray.Length == 0)
                throw new ArgumentException("Array of coefficients or array of powers can't be empty.");

            if (coefficientsArray.Length != powersArray.Length)
                throw new ArgumentException("The array of coefficients must have the same dimension as the array of powers.");

            foreach (int power in powersArray)
                if (power < 0)
                    throw new ArgumentException("The polynomial can't have negative power.");

            if (powersArray.HasDuplicateValues())
                throw new ArgumentException("Array of powers can't have duplicates values.");

            Power = powersArray.Max();
            int countOfNonzerosElements = 0;

            foreach (int coefficient in coefficientsArray)
                if (coefficient != 0)
                    countOfNonzerosElements++;

            polynomialElements = new PolynomialElement[countOfNonzerosElements];

            for (int i = 0, j = 0; i < coefficientsArray.Length; i++)
            {
                if (coefficientsArray[i] != 0)
                {
                    polynomialElements[j].Coefficient = coefficientsArray[i];
                    polynomialElements[j].Power = powersArray[i];
                    j++;
                }
            }
        }

        /// <summary>
        /// This constructors create a polynomial using array of coefficients. Power of polynomial will be equal to lenght of array.
        /// </summary>
        /// <param name="coefficientsArray">Array of coefficients of polynomial summands.</param>
        public Polynomial(double[] coefficientsArray)
        {
            if (coefficientsArray.Length == 0)
                throw new ArgumentException("Array of coefficients can't be empty.");

            Power = coefficientsArray.Length - 1;
            int countOfNonzerosElements = 0;

            foreach (int coefficient in coefficientsArray)
                if (coefficient != 0.0)
                    countOfNonzerosElements++;

            polynomialElements = new PolynomialElement[countOfNonzerosElements];

            for (int i = 0, j = 0; i < coefficientsArray.Length; i++)
            {
                if (coefficientsArray[i] != 0)
                {
                    polynomialElements[j].Power = coefficientsArray.Length - 1 - i;
                    polynomialElements[j].Coefficient = coefficientsArray[i];
                    j++;
                }
            }
        }
        #endregion

        #region overloadable operators
        /// <summary>
        /// Sum of two polynomials.
        /// </summary>
        /// <param name="p1">The first term. First polynomial.</param>
        /// <param name="p2">The second term. Second Polynomial.</param>
        /// <returns>Returns new polynomial which represents sum.</returns>
        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            double[] resultCoefficients = new double[Math.Max(p1.Power, p2.Power) + 1];

            for (int i = 0; i < resultCoefficients.Length; i++)
            {
                if (p1.IndexOfPower(i) != -1)
                {
                    if (p2.IndexOfPower(i) != -1)
                        resultCoefficients[i] = p1.GetCoefficientOfPower(i) + p2.GetCoefficientOfPower(i);
                    else
                        resultCoefficients[i] = p1.GetCoefficientOfPower(i);
                }
                else
                {
                    if (p2.IndexOfPower(i) != -1)
                        resultCoefficients[i] = p2.GetCoefficientOfPower(i);
                    else
                        resultCoefficients[i] = 0;
                }
            }

            Array.Reverse(resultCoefficients);

            return new Polynomial(resultCoefficients);
        }

        /// <summary>
        /// Difference of two polynomials.
        /// </summary>
        /// <param name="p1">The first term. First polynomial.</param>
        /// <param name="p2">The second term. Second Polynomial.</param>
        /// <returns>Returns new polynomial which represents difference.</returns>
        public static Polynomial operator -(Polynomial p1, Polynomial p2) => p1 + (-p2);

        /// <summary>
        /// This operator return inverse polynomial which is an original polynomial with inverse coefficients.
        /// </summary>
        /// <param name="polynomial">Original polynomial</param>
        /// <returns>Returns inverse polynomial.</returns>
        public static Polynomial operator -(Polynomial polynomial)
        {
            int[] powers = new int[polynomial.polynomialElements.Length];
            double[] coefficients = new double[polynomial.polynomialElements.Length];

            for (int i = 0; i < polynomial.polynomialElements.Length; i++)
            {
                powers[i] = polynomial.polynomialElements[i].Power;
                coefficients[i] = -polynomial.polynomialElements[i].Coefficient;
            }

            return new Polynomial(coefficients, powers);
        }

        /// <summary>
        /// This operator return result of multiplying of two polynomials. 
        /// </summary>
        /// <param name="p1">The first term. First polynomial.</param>
        /// <param name="p2">The second term. Second Polynomial.</param>
        /// <returns>Returns result of multiplication which represents by new polinomial.</returns>
        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            double[] resultCoefficients = new double[p1.Power + p2.Power + 1];

            for (int i = 0; i < p1.polynomialElements.Length; i++)
            {
                for (int j = 0; j < p2.polynomialElements.Length; j++)
                {

                    int resPower = p1.polynomialElements[i].Power + p2.polynomialElements[j].Power;
                    double resCoeff = p1.polynomialElements[i].Coefficient * p2.polynomialElements[j].Coefficient;

                    resultCoefficients[resPower] += resCoeff;
                }
            }

            Array.Reverse(resultCoefficients);

            return new Polynomial(resultCoefficients);
        }

        /// <summary>
        /// This method compares the two polynomials.
        /// </summary>
        /// <param name="p1">The first term. First polynomial.</param>
        /// <param name="p2">The second term. Second Polynomial.</param>
        /// <returns>Returns true if polynomials are equal.</returns>
        public static bool operator ==(Polynomial p1, Polynomial p2) => p1.Equals(p2);

        /// <summary>
        /// This method compares the two polynomials.
        /// </summary>
        /// <param name="p1">The first term. First polynomial.</param>
        /// <param name="p2">The second term. Second Polynomial.</param>
        /// <returns>Returns true if polynomials are not equal.</returns>
        public static bool operator !=(Polynomial p1, Polynomial p2) => !(p1.Equals(p2));
        #endregion

        #region private helper functions
        /// <summary>
        /// This method return index of power if it's exists or -1 otherwise.
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        private int IndexOfPower(int power)
        {
            for (int i = 0; i < polynomialElements.Length; i++)
                if (polynomialElements[i].Power == power)
                    return i;

            return -1;
        }

        /// <summary>
        /// This method returns coefficient of given power. If it power doesn't exist you will give ArgumentException.
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        private double GetCoefficientOfPower(int power)
        {
            int indexOfPower = IndexOfPower(power);

            if (indexOfPower == -1)
                throw new ArgumentException("This power doesn't exist.");

            return polynomialElements[indexOfPower].Coefficient;
        }
        #endregion

        /// <summary>
        /// This method gives string formula representation of polynomial.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < polynomialElements.Length; i++)
            {
                if (polynomialElements[i].Power == 1)
                {
                    if (polynomialElements[i].Coefficient < 0)
                        result.Append(" " + polynomialElements[i].Coefficient + "x");
                    else
                        result.Append(" +" + polynomialElements[i].Coefficient + "x");
                }

                if (polynomialElements[i].Power == 0)
                {
                    if (polynomialElements[i].Coefficient < 0)
                        result.Append(" " + polynomialElements[i].Coefficient);
                    else
                        result.Append(" +" + polynomialElements[i].Coefficient);
                }

                if (polynomialElements[i].Power == Power)
                {
                    if (polynomialElements[i].Coefficient < 0)
                        result.Append(" " + polynomialElements[i].Coefficient + "x^(" + polynomialElements[i].Power + ")");
                    else
                        result.Append(polynomialElements[i].Coefficient + "x^(" + polynomialElements[i].Power + ")");
                }

                if (polynomialElements[i].Power < Power && polynomialElements[i].Power > 1)
                {
                    if (polynomialElements[i].Coefficient < 0)
                        result.Append(" " + polynomialElements[i].Coefficient + "x^(" + polynomialElements[i].Power + ")");
                    else
                        result.Append(" +" + polynomialElements[i].Coefficient + "x^(" + polynomialElements[i].Power + ")");
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// T
        /// </summary>
        /// <param name="polynomial"></param>
        /// <returns></returns>
        public bool Equals(Polynomial polynomial)
        {
            if (this.Power != polynomial.Power)
                return false;

            for (int i = 0; i < polynomialElements.Length; i++)
                if (polynomialElements[i].Power != polynomial.polynomialElements[i].Power ||
                    polynomialElements[i].Coefficient != polynomial.polynomialElements[i].Coefficient)
                    return false;

            return true;
        }

        /// <summary>
        /// This method compares current polynomial to each other which you gives as argument.
        /// </summary>
        /// <returns>Returns true if polynomials are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Polynomial))
                throw new ArgumentException("Argument isn't Polynomial type.");

            return this.Equals((Polynomial)obj);
        }

        /// <summary>
        /// This method returns a hash code for the current object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => this.ToString().GetHashCode();

        /// <summary>
        /// This method retuns new object which is an exact copy of the current object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            int[] powers = new int[polynomialElements.Length];
            double[] coefficients = new double[polynomialElements.Length];

            for (int i = 0; i < polynomialElements.Length; i++)
            {
                powers[i] = polynomialElements[i].Power;
                coefficients[i] = polynomialElements[i].Coefficient;
            }

            return new Polynomial(coefficients, powers);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
