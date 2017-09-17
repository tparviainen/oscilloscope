using PluginContracts;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace LAN
{
    public class LANInterface : IPluginV1
    {
        public string Name { get; } = "LAN";

        public string Description { get; } = "LAN communication interface for oscilloscopes such as Rigol DS1054Z";

        public IPEndPoint IPEndPoint { get; set; }

        public async Task<byte[]> SendReceiveAsync(string command)
        {
            using (var socket = new Socket(IPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(IPEndPoint);

                // Start with the initial size of the socket receive buffer
                using (var ms = new MemoryStream(socket.ReceiveBufferSize))
                {
                    socket.Send(Encoding.ASCII.GetBytes(command + "\n"));

                    var data = new byte[1024];

                    int received = 0;

                    do
                    {
                        // Receive will block if no data available
                        received = socket.Receive(data, data.Length, 0);

                        // Zero bytes received means that socket has been closed by the remote host
                        if (received != 0)
                        {
                            await ms.WriteAsync(data, 0, received);
                        }

                        // Read until terminator '\n' (0x0A) found from buffer
                    } while (received != 0 && data[received - 1] != 0x0A);

                    return ms.ToArray();
                }
            }
        }
    }
}
