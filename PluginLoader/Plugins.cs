using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace PluginLoader
{
    public static class Plugins<T> where T : class
    {
        /// <summary>
        /// Loads interface plugins from the specified location.
        /// </summary>
        /// <param name="path">Load path</param>
        /// <returns></returns>
        public static ICollection<T> Load(string path)
        {
            var plugins = new List<T>();

            // Convert relative path to absolute (needed by LoadFromAssemblyPath)
            path = Path.GetFullPath(path);

            if (Directory.Exists(path))
            {
                Type pluginType = typeof(T);

                // All interface plugins have "if_" prefix
                var assemblies = Directory.GetFiles(path, "if_*.dll");

                foreach (var assemblyPath in assemblies)
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

                    foreach (var type in assembly.GetTypes())
                    {
                        var typeInfo = type.GetTypeInfo();

                        if (typeInfo.GetInterface(pluginType.FullName) != null)
                        {
                            T plugin = Activator.CreateInstance(type) as T;

                            plugins.Add(plugin);
                        }
                    }
                }
            }

            return plugins;
        }
    }
}
