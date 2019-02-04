using SCPI.Acquire;
using System.Text;
using Xunit;

namespace SCPI.Tests.Acquire
{
    public class ACQUIRE_SRATE_Tests
    {
        [Fact]
        public void ValidCommand()
        {
            // Arrange
            var cmd = new ACQUIRE_SRATE();
            var expected = ":ACQuire:SRATe?";

            // Act
            var actual = cmd.Command();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SampleRateIsConvertedSuccessfully()
        {
            // Arrange
            var cmd = new ACQUIRE_SRATE();
            var data = "1.000000e+09\n";
            var expected = 1000000000f;

            // Act
            var result = cmd.Parse(Encoding.ASCII.GetBytes(data));

            // Assert
            Assert.True(result);
            Assert.Equal(expected, cmd.SampleRate);
        }

        [Fact]
        public void InvalidDataFromInstrument()
        {
            // Arrange
            var cmd = new ACQUIRE_SRATE();

            // Act
            var result = cmd.Parse(null);

            // Assert
            Assert.False(result);
        }
    }
}
