using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SCPI.Tests
{
    public class Commands_Tests
    {
        [Fact]
        public void HasSupportedCommands()
        {
            // Arrange
            var commands = new Commands();

            // Act
            var names = commands.Names();

            // Assert
            Assert.NotEmpty(names);
        }

        [Fact]
        public void LazyInitializationWorks()
        {
            // Arrange
            var commands = new Commands();
            var names = commands.Names();

            // Act
            var cmd1 = commands.Get(names.ElementAt(0));
            var cmd2 = commands.Get(names.ElementAt(0));

            // Assert
            Assert.Equal(cmd1, cmd2);
        }

        [Fact]
        public void QueryNotSupportedCommand()
        {
            // Arrange
            var commands = new Commands();

            // Act
            Assert.Throws<InvalidOperationException>(() => commands.Get("NOT_SUPPORTED"));

            // Assert
        }
    }
}
