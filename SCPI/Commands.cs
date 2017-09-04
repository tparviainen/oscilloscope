﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SCPI
{
    public class Commands
    {
        private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        /// <summary>
        /// Creates a list of supported commands in the SCPI assembly
        /// </summary>
        /// <returns>The names of the supported commands</returns>
        public ICollection<string> Names()
        {
            return SupportedCommands().Select(t => t.Name).ToList();
        }

        /// <summary>
        /// Creates an instance of the requested command (if it does not exist)
        /// and returns it to the caller.
        /// </summary>
        /// <param name="command">Command name</param>
        /// <returns>Instance of the requested command</returns>
        public ICommand Get(string command)
        {
            ICommand cmd = null;

            if (!commands.TryGetValue(command, out cmd))
            {
                // Lazy initialization of the command
                var typeInfo = SupportedCommands().Where(ti => ti.Name.Equals(command)).Single();

                cmd = (ICommand)Activator.CreateInstance(typeInfo.AsType());

                commands.Add(command, cmd);
            }

            return cmd;
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
