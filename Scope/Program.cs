using PluginContracts;
using PluginLoader;
using Scope.Extensions;
using SCPI;
using System;
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
                if (args.Exists("-s"))
                {
                    var commands = new Commands();

                    Console.Write($"Supported commands: {string.Join(", ", commands.Names())}");
                }
                else
                {
                    RunAsync(args).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task RunAsync(string[] args)
        {
            // -i <interface> <args>
            var communicationInterface = args.Parse("-i");

            // -c <command> <args>
            var command = args.Parse("-c");

            if (communicationInterface.Count == 0 || command.Count == 0)
            {
                Usage();
                return;
            }

            // Load the interface assemblies from the application's folder
            var assemblyFolder = Assembly.GetEntryAssembly().GetAssemblyFolder();
            var plugins = Plugins<IPluginV1>.Load(assemblyFolder).ToList();

            // -p <plugin-folder> <plugin-folder> ...
            var additionalPlugins = args.Parse("-p");
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

                var data = await plugin.SendReceiveAsync(scpiCommand);

                if (commandInstance.Parse(data))
                {
                    var outputFileName = args.Parse("-o");
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
