using System;
using System.Collections.Generic;
using System.Text;

namespace PluginContracts
{
    /// <summary>
    /// Common data for all versions of plugin interfaces.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// The name of the interface
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the interface
        /// </summary>
        string Description { get; }
    }
}
