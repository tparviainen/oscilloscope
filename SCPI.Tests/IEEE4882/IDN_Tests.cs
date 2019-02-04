using SCPI.IEEE4882;
using System.Text;
using Xunit;

namespace SCPI.Tests.IEEE4882
{
    public class IDN_Tests
    {
        [Fact]
        public void ValidCommand()
        {
            // Arrange
            var cmd = new IDN();
            var expected = "*IDN?";

            // Act
            var actual = cmd.Command();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParsingCommandSucceeds()
        {
            // Arrange
            var cmd = new IDN();
            var data = "RIGOL TECHNOLOGIES, DS1054Z, DS1ZA000000000, 00.04.04.SP3\n";

            // Act
            var result = cmd.Parse(Encoding.ASCII.GetBytes(data));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ParsingCommandFails()
        {
            // Arrange
            var cmd = new IDN();
            var data = "Not a valid ID string";

            // Act
            var result = cmd.Parse(Encoding.ASCII.GetBytes(data));

            // Assert
            Assert.False(result);
        }
    }
}
