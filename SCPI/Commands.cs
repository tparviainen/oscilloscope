using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SCPI
{
    public class Commands
    {
        private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        /// <summary>
        /// Creates a list of supported commands in the SCPI assembly
        /// </summary>
        /// <returns>The names of the supported commands</returns>
        public IEnumerable<string> Names() => SupportedCommands().Select(t => t.Name).OrderBy(s => s);

        /// <summary>
        /// Creates an instance of the requested command (if it does not exist)
        /// and returns it to the caller.
        /// </summary>
        /// <param name="command">Command name</param>
        /// <returns>Instance of the requested command</returns>
        public ICommand Get(string command)
        {
            // Command classes (inherited from ICommand) are always uppercase and thus
            // command must be uppercase as well to match supported commands.
            command = command.ToUpper();

            if (!commands.TryGetValue(command, out ICommand cmd))
            {
                // Lazy initialization of the command
                var typeInfo = SupportedCommands().Where(ti => ti.Name.Equals(command)).Single();

                cmd = (ICommand)Activator.CreateInstance(typeInfo.AsType());

                commands.Add(command, cmd);
            }

            return cmd;
        }

        /// <summary>
        /// Creates an instance of the requested command (if it does not exist)
        /// and returns an actual type of the command to the caller.
        /// </summary>
        /// <param name="command">Command name</param>
        /// <returns>Instance of the requested command</returns>
        public T Get<T>() where T : ICommand
        {
            var command = typeof(T).Name;

            if (!commands.TryGetValue(command, out ICommand cmd))
            {
                // Lazy initialization of the command
                var typeInfo = SupportedCommands().Where(ti => ti.Name.Equals(command)).Single();

                cmd = (ICommand)Activator.CreateInstance(typeInfo.AsType());

                commands.Add(command, cmd);
            }

            return (T)cmd;
        }

        private static IEnumerable<TypeInfo> SupportedCommands()
        {
            var assembly = typeof(ICommand).GetTypeInfo().Assembly;

            // Supported commands are the ones that implement ICommand interface
            var commands = assembly.DefinedTypes.Where(ti => ti.ImplementedInterfaces.Contains(typeof(ICommand)));

            return commands;
        }
    }
}
