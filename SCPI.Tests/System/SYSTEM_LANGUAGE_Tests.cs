using SCPI.System;
using System.Text;
using Xunit;

namespace SCPI.Tests.System
{
    public class SYSTEM_LANGUAGE_Tests
    {
        [Theory]
        [InlineData("ENGL")]
        [InlineData("GERM")]
        public void LanguageIsSetToProperty(string expected)
        {
            // Arrange
            var cmd = new SYSTEM_LANGUAGE();

            // Act
            var result = cmd.Parse(Encoding.ASCII.GetBytes($"{expected}\n"));

            // Assert
            Assert.True(result);
            Assert.Equal(expected, cmd.Language);
        }
    }
}
