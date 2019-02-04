using SCPI.Display;
using Xunit;

namespace SCPI.Tests.Display
{
    public class DISPLAY_DATA_Tests
    {
        [Fact]
        public void DescriptionExists()
        {
            // Arrange
            var cmd = new DISPLAY_DATA();

            // Act
            var desc = cmd.Description;

            // Assert
            Assert.NotNull(desc);
        }

        [Fact]
        public void CommandWithoutParameters()
        {
            // Arrange
            var cmd = new DISPLAY_DATA();
            var expected = ":DISPlay:DATA?";

            // Act
            var actual = cmd.Command();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommandWithColorParameter()
        {
            // Arrange
            var cmd = new DISPLAY_DATA();
            var expected = ":DISPlay:DATA? ON";

            // Act
            var actual = cmd.Command("ON");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommandWithColorInvertParameters()
        {
            // Arrange
            var cmd = new DISPLAY_DATA();
            var expected = ":DISPlay:DATA? ON, OFF";

            // Act
            var actual = cmd.Command("ON", "OFF");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommandWithColorInvertFormatParameters()
        {
            // Arrange
            var cmd = new DISPLAY_DATA();
            var expected = ":DISPlay:DATA? ON, OFF, BMP24";

            // Act
            var actual = cmd.Command("ON", "OFF", "BMP24");

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
