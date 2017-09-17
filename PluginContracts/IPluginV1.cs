using System;
using System.Net;
using System.Threading.Tasks;

namespace PluginContracts
{
    public interface IPluginV1 : IPlugin
    {
        /// <summary>
        /// IP address and port number of the connection
        /// </summary>
        IPEndPoint IPEndPoint { get; set; }

        /// <summary>
        /// Sends the command to instrument and waits a response from the connected instrument
        /// </summary>
        /// <param name="command">Command to send</param>
        /// <returns>Received data</returns>
        Task<byte[]> SendReceiveAsync(string command);
    }
}
