using SCPI;
using SCPI.Display;
using SCPI.IEEE4882;
using SCPI.System;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace LAN.Tests
{
    public class LANInterface_Tests
    {
        private readonly string IP = "192.168.1.160";
        private readonly int PORT = 5555;

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
            if (ret)
                File.WriteAllBytes($"Screenshot-{DateTime.Now:yyyyMMddTHHmmss}.bmp", command.ImageData());

            // Assert
            Assert.True(ret);
            Assert.NotNull(resp);
        }

        [Fact]
        public async Task QueryTheInstrumentLanguage()
        {
            // Arrange
            var ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var lanIf = new LANInterface() { IPEndPoint = ep };
            var commands = new Commands();
            var command = commands.Get<SYSTEM_LANGUAGE>();

            // Act
            var resp = await lanIf.SendReceiveAsync(command.Command());

            // Assert
            Assert.NotNull(resp);
            Assert.True(resp.Length != 0);
        }
    }
}
