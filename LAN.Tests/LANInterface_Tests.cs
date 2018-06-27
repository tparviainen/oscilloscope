using SCPI;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LAN.Tests
{
    public class LANInterface_Tests
    {
        private string IP = "192.168.1.160";
        private int PORT = 5555;

        [Fact]
        public async Task QueryTheIdStringOfTheInstrumentAsync()
        {
            // Arrange
            var ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var lanIf = new LANInterface() { IPEndPoint = ep };
            var commands = new Commands();
            var command = commands.Get<IDN>();

            // Act
            var resp = await lanIf.SendReceiveAsync(command.Command());
            var ret = command.Parse(resp);

            // Assert
            Assert.True(ret);
            Assert.Equal("RIGOL TECHNOLOGIES", command.Manufacturer);
            Assert.NotEmpty(command.Model);
            Assert.NotEmpty(command.SerialNumber);
            Assert.NotEmpty(command.SoftwareVersion);
        }

        [Fact]
        public async Task ReadTheImageCurrentlyDisplayedOnTheScreenAsync()
        {
            // Arrange
            var ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var lanIf = new LANInterface() { IPEndPoint = ep };
            var commands = new Commands();
            var command = commands.Get<DISPLAY_DATA>();

            // Act
            var resp = await lanIf.SendReceiveAsync(command.Command());

            var ret = command.Parse(resp);
            if (ret == true)
            {
                File.WriteAllBytes($"Screenshot-{DateTime.Now:yyyyMMddTHHmmss}.bmp", command.ImageData());
            }

            // Assert
            Assert.True(ret);
            Assert.NotNull(resp);
        }
    }
}
