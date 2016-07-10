using System;
using NUnit.Framework;

namespace PolynomialLibrary.Tests
{
    [TestFixture]
    public class Polynomial_Tests
    {
        [TestCase(new int[] {7,12,6,9,4,0,0,3,1}, new int[] {2,0,-3,0,0,2,9}, TestName = "SumOfTwoplynomials1", ExpectedResult = "7x^(8) +12x^(7) +8x^(6) +9x^(5) +1x^(4) +5x +10")]
        public string TestPolynomialSum(int[] coeffArr, int[] coeffBrr)
        {
            return (new Polynomial(coeffArr) + new Polynomial(coeffBrr)).ToString();
        }
    }
}
