using System;
using System.IO;
using System.Reflection;

namespace Scope.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns an assembly's containing folder on disk
        /// </summary>
        /// <param name="assembly">Assembly whose location will be determined</param>
        /// <returns>The location of the assembly on disk or null if it
        /// cannot be determined.</returns>
        public static string GetAssemblyFolder(this Assembly assembly)
        {
            try
            {
                if (!string.IsNullOrEmpty(assembly.Location))
                {
                    return Path.GetDirectoryName(assembly.Location);
                }

                if (string.IsNullOrEmpty(assembly.CodeBase))
                {
                    return null;
                }

                var uri = new Uri(assembly.CodeBase);
                if (!uri.IsFile)
                {
                    return null;
                }

                return Path.GetDirectoryName(uri.LocalPath);
            }
            catch (NotSupportedException)
            {
                // Dynamic assembly generated with Reflection.Emit
                return null;
            }
        }
    }
}
