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
    }
}
