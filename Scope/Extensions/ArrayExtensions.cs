using System;
using System.Collections.Generic;

namespace Scope.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Parses argument values from the array based on the argument name.
        /// </summary>
        /// <param name="args">The array of all arguments and values</param>
        /// <param name="name">The name of the argument to parse (-a, -b, -c, ...)</param>
        /// <returns>Arguments, or null if name not found</returns>
        public static List<string> Parse(this string[] args, string name)
        {
            var values = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (name.Equals(args[i], StringComparison.CurrentCultureIgnoreCase))
                {
                    while (++i < args.Length && !args[i].StartsWith("-"))
                    {
                        values.Add(args[i]);
                    }

                    return values;
                }
            }

            return values;
        }

        /// <summary>
        /// Determines whether the specified array contains the element
        /// </summary>
        /// <param name="args">The array of all arguments and values</param>
        /// <param name="name">The name of the element to search for</param>
        /// <returns></returns>
        public static bool Exists(this string[] args, string name)
        {
            return Array.Exists(args, p => p.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
