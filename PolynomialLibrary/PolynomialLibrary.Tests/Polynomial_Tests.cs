using System;
using NUnit.Framework;

namespace PolynomialLibrary.Tests
{
    [TestFixture]
    public class Polynomial_Tests
    {
        [TestCase(new double[] {7,12,6,9,4,0,0,3,1}, new double[] {2,0,-3,0,0,2,9}, TestName = "SumOfTwoplynomials1", ExpectedResult = "7x^(8) +12x^(7) +8x^(6) +9x^(5) +1x^(4) +5x +10")]
        public string TestPolynomialSum(double[] coeffArr, double[] coeffBrr)
        {
            return (new Polynomial(coeffArr) + new Polynomial(coeffBrr)).ToString();
        }
    }
}
