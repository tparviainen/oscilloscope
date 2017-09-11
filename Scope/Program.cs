using PluginContracts;
using PluginLoader;
using SCPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scope
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Scope v1.0\n");

            try
            {
                // -s (list of supported commands)
                if (Array.Exists(args, p => p.Equals("-s", StringComparison.CurrentCultureIgnoreCase)))
                {
                    var commands = new Commands();

                    Console.Write($"Supported commands: {string.Join(", ", commands.Names())}");
                }
                else
                {
                    Run(args).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task Run(string[] args)
        {
            // -i <interface> <args>
            var communicationInterface = ParseArguments("-i", args);

            // -c <command> <args>
            var command = ParseArguments("-c", args);

            if (communicationInterface.Count == 0 || command.Count == 0)
            {
                Usage();
                return;
            }

            // Load the interface assemblies from the application's folder
            var assemblyFolder = AssemblyHelper.GetAssemblyFolder(Assembly.GetEntryAssembly());
            var plugins = Plugins<IPluginV1>.Load(assemblyFolder).ToList();

            // -p <plugin-folder> <plugin-folder> ...
            var additionalPlugins = ParseArguments("-p", args);
            if (additionalPlugins.Count != 0)
            {
                foreach (var pluginFolder in additionalPlugins)
                {
                    plugins.AddRange(Plugins<IPluginV1>.Load(pluginFolder));
                }
            }

            // Find the correct plugin to use for communicating with the instrument
            var plugin = plugins.FirstOrDefault(p => p.Name.Equals(communicationInterface[0], StringComparison.CurrentCultureIgnoreCase));
            if (plugin != null)
            {
                var endPoint = communicationInterface[1].Split(':');
                plugin.IPEndPoint = new IPEndPoint(IPAddress.Parse(endPoint[0]), int.Parse(endPoint[1]));

                var commands = new Commands();

                var commandInstance = commands.Get(command[0]);

                command.RemoveAt(0);

                var scpiCommand = commandInstance.Command(command.ToArray());

                var data = await plugin.SendReceive(scpiCommand);

                if (commandInstance.Parse(data))
                {
                    var outputFileName = ParseArguments("-o", args);
                    if (outputFileName.Count != 0)
                    {
                        File.WriteAllBytes(outputFileName.First(), data);
                    }
                    else
                    {
                        Console.WriteLine(Encoding.ASCII.GetString(data));
                    }
                }
                else
                {
                    // This happens when supported SCPI command is unable to parse received
                    // data, for RAW commands this will never happen.
                    Console.WriteLine("Invalid reply from the instrument!");
                }
            }
            else
            {
                Console.WriteLine($"Cannot find requested interface plugin, {communicationInterface[0]}");
            }
        }

        /// <summary>
        /// Parses argument values from the array based on the argument name.
        /// </summary>
        /// <param name="name">The name of the argument to parse (-a, -b, -c, ...)</param>
        /// <param name="args">The array of all arguments and values</param>
        /// <returns>Arguments, or null if name not found</returns>
        private static List<string> ParseArguments(string name, string[] args)
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
        /// Prints the usage of the application to console
        /// </summary>
        private static void Usage()
        {
            Console.WriteLine("Usage: scope [optional] <required>");
            Console.WriteLine();
            Console.WriteLine("Required arguments:");
            Console.WriteLine(" -i <interface> <args>");
            Console.WriteLine(" -c <command> <args>");
            Console.WriteLine();
            Console.WriteLine("Optional arguments:");
            Console.WriteLine(" -o <output-filename>");
            Console.WriteLine(" -p <plugin-folder> <plugin-folder> ...");
            Console.WriteLine(" -s (list of supported commands)");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine(" scope -i LAN 192.168.1.160:5555 -c IDN");
            Console.WriteLine(" scope -i LAN 192.168.1.160:5555 -c RAW *IDN?");
            Console.WriteLine(" scope -i LAN 192.168.1.160:5555 -c RAW *IDN? -o received.raw");
            Console.WriteLine(" scope -s");
            Console.WriteLine();
        }
    }
}
