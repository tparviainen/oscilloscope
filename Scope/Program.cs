using PluginContracts;
using PluginLoader;
using Scope.Extensions;
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
                var commands = new Commands();

                // -s (list of supported commands)
                if (args.Exists("-s"))
                {
                    Console.Write($"Supported commands: {string.Join(", ", commands.Names())}");
                }
                else
                {
                    RunAsync(args, commands).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task RunAsync(string[] args, Commands commands)
        {
            // Command is a required argument
            var command = args.Parse("-c");
            if (command.Count == 0)
            {
                Usage();
                return;
            }

            // Command specific help message can be shown without interface definition!
            if (args.Exists("-h"))
            {
                var commandInstance = commands.Get(command[0]);
                Console.WriteLine(commandInstance.Description);
                Console.WriteLine();
                Console.WriteLine(commandInstance.HelpMessage());
                return;
            }

            // Interface is a required argument
            var communicationInterface = args.Parse("-i");
            if (communicationInterface.Count == 0)
            {
                Usage();
                return;
            }

            var additionalPlugins = args.Parse("-p");
            var plugins = LoadPlugins<IPluginV1>(additionalPlugins);

            // Find the correct plugin to use for communicating with the instrument
            var plugin = plugins.FirstOrDefault(p => p.Name.Equals(communicationInterface[0], StringComparison.CurrentCultureIgnoreCase));
            if (plugin != null)
            {
                var endPoint = communicationInterface[1].Split(':');
                plugin.IPEndPoint = new IPEndPoint(IPAddress.Parse(endPoint[0]), int.Parse(endPoint[1]));

                var commandInstance = commands.Get(command[0]);

                command.RemoveAt(0);

                var scpiCommand = commandInstance.Command(command.ToArray());

                var data = await plugin.SendReceiveAsync(scpiCommand);

                if (commandInstance.Parse(data))
                {
                    var outputFileName = args.Parse("-o").FirstOrDefault();
                    if (outputFileName != null)
                    {
                        File.WriteAllBytes(outputFileName, data);
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
        /// Load all interface assemblies
        /// </summary>
        /// <param name="additionalPlugins">Additional plugins folder</param>
        /// <returns></returns>
        private static List<T> LoadPlugins<T>(List<string> additionalPlugins) where T : class
        {
            // Load the interface assemblies from the application's folder
            var assemblyFolder = Assembly.GetEntryAssembly().GetAssemblyFolder();
            var plugins = Plugins<T>.Load(assemblyFolder).ToList();

            // Load the interface assemblies from the specified folders
            if (additionalPlugins.Count != 0)
            {
                foreach (var pluginFolder in additionalPlugins)
                {
                    plugins.AddRange(Plugins<T>.Load(pluginFolder));
                }
            }

            return plugins;
        }

        /// <summary>
        /// Prints the usage of the application to console
        /// </summary>
        private static void Usage()
        {
            Console.WriteLine("Usage: scope [optional] <required>");
            Console.WriteLine();
            Console.WriteLine("Required arguments:");
            Console.WriteLine(" -i <interface> <arg>");
            Console.WriteLine(" -c <command> [<arg> ...]");
            Console.WriteLine();
            Console.WriteLine("Optional arguments:");
            Console.WriteLine(" -o <output-filename>");
            Console.WriteLine(" -p <plugin-folder> [<plugin-folder> ...]");
            Console.WriteLine(" -s (list of supported commands)");
            Console.WriteLine(" -h (help message about the command)");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine(" scope -c RAW -h");
            Console.WriteLine(" scope -i LAN 192.168.1.160:5555 -c IDN");
            Console.WriteLine(" scope -i LAN 192.168.1.160:5555 -c RAW *IDN?");
            Console.WriteLine(" scope -i LAN 192.168.1.160:5555 -c RAW *IDN? -o received.raw");
            Console.WriteLine(" scope -s");
            Console.WriteLine();
        }
    }
}
