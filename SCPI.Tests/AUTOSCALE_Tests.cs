using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SCPI.Tests
{
    public class AUTOSCALE_Tests
    {
        [Fact]
        public void ValidCommand()
        {
            // Arrange
            var cmd = new AUTOSCALE();
            var expected = ":AUToscale";

            // Act
            var actual = cmd.Command();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
