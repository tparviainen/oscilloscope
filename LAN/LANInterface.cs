using PluginContracts;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LAN
{
    public class LANInterface : IPluginV1
    {
        public string Name { get; } = "LAN";

        public string Description { get; } = "LAN communication interface for oscilloscopes such as Rigol DS1054Z";

        public IPEndPoint IPEndPoint { get; set; }

        public int ReadTimeout { get; set; } = 500;

        public async Task<byte[]> SendReceiveAsync(string command)
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(IPEndPoint.Address, IPEndPoint.Port);

                using (var stream = client.GetStream())
                {
                    stream.ReadTimeout = ReadTimeout;

                    var data = Encoding.UTF8.GetBytes(command + "\n");
                    await stream.WriteAsync(data, 0, data.Length);

                    using (var ms = new MemoryStream())
                    {
                        try
                        {
                            var reader = new BinaryReader(stream);

                            do
                            {
                                var value = reader.ReadByte();
                                ms.WriteByte(value);

                                while (client.Available != 0)
                                {
                                    var values = reader.ReadBytes(client.Available);
                                    ms.Write(values, 0, values.Length);
                                }
                            } while (true);
                        }
                        catch (Exception ex) when (ex.InnerException.GetType() == typeof(SocketException))
                        {
                            // ReadByte() method will eventually timeout ...
                        }

                        return ms.ToArray();
                    }
                }
            }
        }
    }
}
