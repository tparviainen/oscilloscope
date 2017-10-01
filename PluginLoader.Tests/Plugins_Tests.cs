using PluginContracts;
using System;
using System.Collections.Generic;
using Xunit;

namespace PluginLoader.Tests
{
    public class Plugins_Tests
    {
        [Fact]
        public void PluginsFoundFromLibsFolder()
        {
            // Arrange
            var path = @"..\..\..\..\Libs";

            // Act
            var plugins = Plugins<IPluginV1>.Load(path);

            // Assert
            Assert.NotEmpty(plugins);
        }

        [Fact]
        public void PluginsNotFoundFromCurrentFolder()
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
