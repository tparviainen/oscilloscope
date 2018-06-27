using PluginContracts;
using Scope.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scope.Wpf.Models
{
    class ConnectionInterface
    {
        public IPluginV1 Plugin { get; private set; }
        public Command CmdSelect { get; private set; }

        public ConnectionInterface(IPluginV1 plugin)
        {
            Plugin = plugin;
            CmdSelect = new Command(Selected, null);
        }

        private void Selected()
        {

        }
    }
}
