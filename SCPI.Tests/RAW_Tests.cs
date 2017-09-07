using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SCPI.Tests
{
    public class RAW_Tests
    {
        [Fact]
        public void WithoutArguments()
        {
            // Arrange
            var raw = new RAW();
            var expected = "";

            // Act
            var actual = raw.Command();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EmbeddedArguments()
        {
            // Arrange
            var raw = new RAW();
            var expected = ":DISPlay:DATA? ON, OFF, TIFF";

            // Act, one command with arguments embedded
            var actual = raw.Command(":DISPlay:DATA? ON, OFF, TIFF");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithOneExtraArgument()
        {
            // Arrange
            var raw = new RAW();
            var expected = ":DISPlay:DATA? ON";

            // Act, command line style arguments
            var actual = raw.Command(":DISPlay:DATA?", "ON");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithSeveralExtraArguments()
        {
            // Arrange
            var raw = new RAW();
            var expected = ":DISPlay:DATA? ON, OFF, TIFF";

            // Act, command line style arguments
            var actual = raw.Command(":DISPlay:DATA?", "ON", "OFF", "TIFF");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoExtraArguments()
        {
            // Arrange
            var raw = new RAW();
            var expected = ":DISPlay:DATA?";

            // Act
            var actual = raw.Command(":DISPlay:DATA?");

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
