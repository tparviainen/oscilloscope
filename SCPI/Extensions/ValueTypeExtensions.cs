using System.Globalization;

namespace SCPI.Extensions
{
    public static class ValueTypeExtensions
    {
        /// <summary>
        /// Checks that the value is within specified range
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="minValue">The inclusive lower bound</param>
        /// <param name="maxValue">The inclusive upper bound</param>
        /// <returns>True if the value is within range otherwise false</returns>
        public static bool IsWithin(this int value, int minValue, int maxValue)
        {
            var ret = false;

            if (value >= minValue && value <= maxValue)
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// Converts the string representation of a number to float
        /// </summary>
        /// <param name="value">A string representing a number to convert</param>
        /// <param name="result">When this method returns, contains the single-precision floating-point
        /// number equivalent to the numeric value or symbol contained in value, if the conversion succeeded</param>
        /// <returns>True if value was converted successfully; otherwise, false</returns>
        public static bool ScientificNotationStringToFloat(this string value, out float result)
        {
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }
    }
}
