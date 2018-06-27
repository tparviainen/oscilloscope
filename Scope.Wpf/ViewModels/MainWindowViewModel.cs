using PluginContracts;
using PluginLoader;
using Scope.Wpf.Controls;
using Scope.Wpf.Models;
using SCPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Scope.Wpf.ViewModels
{
    class MainWindowViewModel
    {
        public NetworkEndPoint Connection { get; private set; } = new NetworkEndPoint();
        public Instrument Instrument { get; private set; } = new Instrument();
        public Screenshot Screenshot { get; private set; } = new Screenshot();

        public Command CmdConnect { get; private set; }
        public Command CmdFetch { get; private set; }

        private string pluginPath = @".";

        public ObservableCollection<ConnectionInterface> Interfaces { get; private set; } = new ObservableCollection<ConnectionInterface>();

        public MainWindowViewModel()
        {
            Connection.Port = "5555";
            Connection.IP = "192.168.1.160";

            LoadInterfaces(pluginPath);

            CmdConnect = new Command(Connect, null);
            CmdFetch = new Command(Fetch, () => Instrument.Id != null);
        }

        private void LoadInterfaces(string path)
        {
            foreach (var item in Plugins<IPluginV1>.Load(path))
            {
                Interfaces.Add(new ConnectionInterface(item));
            }
        }

        private void Connect()
        {
            var plugin = Plugins<IPluginV1>.Load(pluginPath).FirstOrDefault();

            plugin.IPEndPoint = Connection.IPEndPoint;

            var commands = new Commands();

            var commandInstance = commands.Get("IDN") as IDN;

            var scpiCommand = commandInstance.Command();

            var data = plugin.SendReceiveAsync(scpiCommand).GetAwaiter().GetResult();

            if (commandInstance.Parse(data))
            {
                Instrument.Id = $"{commandInstance.Manufacturer}, {commandInstance.Model}, {commandInstance.SerialNumber}, {commandInstance.SoftwareVersion}";
                CmdFetch.OnCanExecuteChanged();
            }
        }

        private void Fetch()
        {
            var plugin = Plugins<IPluginV1>.Load(pluginPath).FirstOrDefault();

            plugin.IPEndPoint = Connection.IPEndPoint;

            var commands = new Commands();

            var commandInstance = commands.Get("DISPLAY_DATA") as DISPLAY_DATA;

            var scpiCommand = commandInstance.Command(/*"ON", "OFF", "PNG"*/);

            var data = plugin.SendReceiveAsync(scpiCommand).GetAwaiter().GetResult();

            if (commandInstance.Parse(data))
            {
                Screenshot.Update(commandInstance.ImageData());
            }
        }
    }
}
