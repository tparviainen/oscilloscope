using SCPI.Display;
using System.Text;
using Xunit;

namespace SCPI.Tests.Display
{
    public class DISPLAY_GRID_Tests
    {
        [Theory]
        [InlineData("FULL")]
        [InlineData("HALF")]
        [InlineData("NONE")]
        public void CorrectGridValueIsStoredToProperty(string grid)
        {
            // Arrange
            var cmd = new DISPLAY_GRID();

            // Act
            var result = cmd.Parse(Encoding.ASCII.GetBytes(grid + "\n"));

            // Assert
            Assert.True(result);
            Assert.Equal(grid, cmd.Grid);
        }

        [Theory]
        [InlineData("MAX")]
        [InlineData("Half")]
        [InlineData("Nothing")]
        public void GridValueIsNotValid(string grid)
        {
            // Arrange
            var cmd = new DISPLAY_GRID();

            // Act
            var result = cmd.Parse(Encoding.ASCII.GetBytes(grid + "\n"));

            // Assert
            Assert.False(result);
            Assert.Null(cmd.Grid);
        }
    }
}
