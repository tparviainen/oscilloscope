using SCPI.Display;
using System;
using System.Text;
using Xunit;

namespace SCPI.Tests.Display
{
    public class DISPLAY_WBRIGHTNESS_Tests
    {
        [Fact]
        public void ValidQueryCommand()
        {
            // Arrange
            var cmd = new DISPLAY_WBRIGHTNESS();
            var expected = ":DISPlay:WBRightness?";

            // Act
            var actual = cmd.Command();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(100)]
        public void SetValidBrightnessValue(int brightness)
        {
            // Arrange
            var cmd = new DISPLAY_WBRIGHTNESS();
            var expected = $":DISPlay:WBRightness {brightness}";

            // Act
            var actual = cmd.Command(brightness.ToString());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void SetInvalidBrightnessValue(int brightness)
        {
            // Arrange
            var cmd = new DISPLAY_WBRIGHTNESS();

            // Act / Assert
            Assert.Throws<ArgumentException>(() => cmd.Command(brightness.ToString()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(100)]
        public void ResponseIsParsedSuccessfully(int brightness)
        {
            // Arrange
            var cmd = new DISPLAY_WBRIGHTNESS();
            var data = $"{brightness}\n";

            // Act
            var result = cmd.Parse(Encoding.ASCII.GetBytes(data));

            // Assert
            Assert.True(result);
            Assert.Equal(brightness, cmd.Brightness);
        }
    }
}
