using System;
using System.Collections.Generic;
using System.Text;

namespace SCPI
{
    /// <summary>
    /// Interface for SCPI commands
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The description of the command
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Creates the SCPI command based on the input data
        /// </summary>
        /// <param name="parameters">List of parameters that is used to form the command</param>
        /// <returns>SCPI command</returns>
        string Command(params object[] parameters);

        /// <summary>
        /// Parses and validates the data received from the instrument
        /// </summary>
        /// <param name="data">Received data</param>
        /// <returns>True if parsing succeeded and data is valid, otherwise false</returns>
        bool Parse(byte[] data);
    }
}
