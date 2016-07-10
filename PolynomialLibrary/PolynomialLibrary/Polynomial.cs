using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialLibrary
{
    public class Polynomial : IComparable
    {
        public int Power { get; }
        private PolynomialElement[] polynomialElements;

        public Polynomial() { }

        public Polynomial(int[] coefficientsArray)
        {
            if (coefficientsArray.Length == 0)
                throw new ArgumentException("Coefficients array can't be empty.");

            Power = coefficientsArray.Length - 1;
            int length = 0;

            for (int i = 0; i < coefficientsArray.Length; i++)
                if (coefficientsArray[i] != 0)
                    length++;

            polynomialElements = new PolynomialElement[length];

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

        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            int[] resultCoefficients;

            if (p1.Power > p2.Power)
                resultCoefficients = new int[p1.Power + 1];
            else
                resultCoefficients = new int[p2.Power + 1];

            for (int i = 0; i < resultCoefficients.Length; i++)
            {
                if (p1.IndexOfPower(i) != -1)
                {
                    if (p2.IndexOfPower(i) != -1)
                        resultCoefficients[i] = p1.polynomialElements[p1.IndexOfPower(i)].Coefficient + p2.polynomialElements[p2.IndexOfPower(i)].Coefficient;
                    else
                        resultCoefficients[i] = p1.polynomialElements[p1.IndexOfPower(i)].Coefficient;
                }
                else
                {
                    if (p2.IndexOfPower(i) != -1)
                        resultCoefficients[i] = p2.polynomialElements[p2.IndexOfPower(i)].Coefficient;
                    else
                        resultCoefficients[i] = 0;
                }
            }

            int[] buff = new int[resultCoefficients.Length];

            for (int i = resultCoefficients.Length - 1, j = 0; i >= 0; i--, j++)
                buff[j] = resultCoefficients[i];

            return new Polynomial(buff);
        }

        public static Polynomial operator -(Polynomial p1, Polynomial p2)
        {
            int[] resultCoefficients;

            if (p1.Power > p2.Power)
                resultCoefficients = new int[p1.Power + 1];
            else
                resultCoefficients = new int[p2.Power + 1];

            for (int i = 0; i < resultCoefficients.Length; i++)
            {
                if (p1.IndexOfPower(i) != -1)
                {
                    if (p2.IndexOfPower(i) != -1)
                        resultCoefficients[i] = p1.polynomialElements[p1.IndexOfPower(i)].Coefficient - p2.polynomialElements[p2.IndexOfPower(i)].Coefficient;
                    else
                        resultCoefficients[i] = p1.polynomialElements[p1.IndexOfPower(i)].Coefficient;
                }
                else
                {
                    if (p2.IndexOfPower(i) != -1)
                        resultCoefficients[i] = -p2.polynomialElements[p2.IndexOfPower(i)].Coefficient;
                    else
                        resultCoefficients[i] = 0;
                }
            }

            int[] buff = new int[resultCoefficients.Length];

            for (int i = resultCoefficients.Length - 1, j = 0; i >= 0; i--, j++)
                buff[j] = resultCoefficients[i];

            return new Polynomial(buff);
        }

        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            int[] resultCoefficients = new int[p1.Power + p2.Power + 1];

            for (int i = 0; i < p1.polynomialElements.Length; i++)
            {
                for (int j = 0; j < p2.polynomialElements.Length; j++)
                {

                    int resPower = p1.polynomialElements[i].Power + p2.polynomialElements[j].Power;
                    int resCoeff = p1.polynomialElements[i].Coefficient * p2.polynomialElements[j].Coefficient;

                    resultCoefficients[resPower] += resCoeff;
                }
            }

            int[] buff = new int[resultCoefficients.Length];

            for (int i = resultCoefficients.Length - 1, j = 0; i >= 0; i--, j++)
                buff[j] = resultCoefficients[i];

            return new Polynomial(buff);
        }

        public static bool operator ==(Polynomial p1, Polynomial p2)
        {
            if (p1.Power != p2.Power)
                return false;

            for (int i = 0; i < p1.polynomialElements.Length; i++)
            {
                if ((p1.polynomialElements[i].Power != p2.polynomialElements[i].Power) ||
                    (p1.polynomialElements[i].Coefficient != p2.polynomialElements[i].Coefficient))
                    return false;
            }

            return true;
        }

        public static bool operator !=(Polynomial p1, Polynomial p2)
        {
            if (p1.Power != p2.Power)
                return true;

            for (int i = 0; i < p1.polynomialElements.Length; i++)
            {
                if ((p1.polynomialElements[i].Power != p2.polynomialElements[i].Power) ||
                    (p1.polynomialElements[i].Coefficient != p2.polynomialElements[i].Coefficient))
                    return true;
            }

            return false;
        }

        private int IndexOfPower(int power)
        {
            for (int i = 0; i < polynomialElements.Length; i++)
                if (polynomialElements[i].Power == power)
                    return i;

            return -1;
        }

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

        public override bool Equals(object obj)
        {
            Polynomial p = (Polynomial)obj;

            if (Power != p.Power)
                return false;

            for (int i = 0; i < polynomialElements.Length; i++)
            {
                if ((polynomialElements[i].Power != p.polynomialElements[i].Power) ||
                    (polynomialElements[i].Coefficient != p.polynomialElements[i].Coefficient))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public struct PolynomialElement
    {
        public int Power { get; set; }
        public int Coefficient { get; set; }

        public PolynomialElement(int power, int coefficient)
        {
            Power = power;
            Coefficient = coefficient;
        }
    }
}
