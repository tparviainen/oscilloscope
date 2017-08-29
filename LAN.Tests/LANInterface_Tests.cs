using System;
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
        public async Task QueryTheIdStringOfTheInstrument()
        {
            // Arrange
            var ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var lanIf = new LANInterface() { IPEndPoint = ep };
            var cmd = "*IDN?";

            // Act
            var resp = Encoding.ASCII.GetString(await lanIf.SendReceive(cmd));

            // Assert
            Assert.Contains("RIGOL TECHNOLOGIES", resp);
        }

        [Fact]
        public async Task ReadTheImageCurrentlyDisplayedOnTheScreen()
        {
            // Arrange
            var ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var lanIf = new LANInterface() { IPEndPoint = ep };
            var cmd = ":DISPlay:DATA?";

            // Act
            var resp = await lanIf.SendReceive(cmd);

            // Assert
            Assert.NotNull(resp);
        }
    }
}
