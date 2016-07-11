using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialLibrary
{
    /// <summary>
    /// This is an extension class which allow users to know is this array has duplicate elements.
    /// </summary>
    public static class ExtensionInt32Array
    {
        /// <summary>
        /// This method finds duplicate elements into array. Returns true if it has duplicate elements.
        /// </summary>
        /// <param name="array">Input array</param>
        /// <returns>Returns true if array has duplicate elements.</returns>
        public static bool HasDuplicateValues(this int[] array) => array.Distinct().Count() != array.Length;
    }
}
