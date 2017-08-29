using PluginContracts;
using System;
using System.Collections.Generic;
using Xunit;

namespace PluginLoader.Tests
{
    public class Plugins_Tests
    {
        [Fact]
        public void PluginsFound()
        {
            // Arrange
            var path = @"..\..\..\..\LAN\bin\Debug\netstandard1.3";

            // Act
            var plugins = Plugins<IPluginV1>.Load(path);

            // Assert
            Assert.NotEmpty(plugins);
        }

        [Fact]
        public void PluginsNotFound()
        {
            // Arrange
            var path = @".";

            // Act
            var plugins = Plugins<IPluginV1>.Load(path);

            // Assert
            Assert.Empty(plugins);
        }
    }
}
