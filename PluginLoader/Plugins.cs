using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PluginLoader
{
    public static class Plugins<T> where T : class
    {
        /// <summary>
        /// Loads interface plugins from the specified location.
        /// </summary>
        /// <param name="path">Load path</param>
        /// <param name="searchPattern">Plugin file search pattern, default "if_*.dll"</param>
        /// <returns></returns>
        public static ICollection<T> Load(string path, string searchPattern = "if_*.dll")
        {
            var plugins = new List<T>();

            if (Directory.Exists(path))
            {
                Type pluginType = typeof(T);

                var assemblies = Directory.GetFiles(path, searchPattern);

                foreach (var assemblyPath in assemblies)
                {
                    var assembly = Assembly.LoadFrom(assemblyPath);

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
