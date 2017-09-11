using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Scope
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Returns an assembly's containing folder on disk
        /// </summary>
        /// <param name="assembly">Assembly whose location will be determined</param>
        /// <returns>The location of the assembly on disk or null if it
        /// cannot be determined.</returns>
        public static string GetAssemblyFolder(Assembly assembly)
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
