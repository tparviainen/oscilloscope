using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Scope.Wpf.Models
{
    class NetworkEndPoint : INotifyPropertyChanged
    {
        #region Standard pattern for implementing property changed notification
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private string _Port;

        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
                OnPropertyChanged();
            }
        }

        private string _IP;

        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Network endpoint as an IP address and a port number.
        /// </summary>
        public IPEndPoint IPEndPoint
        {
            get
            {
                return new IPEndPoint(IPAddress.Parse(IP), int.Parse(Port));
            }
        }
    }
}
