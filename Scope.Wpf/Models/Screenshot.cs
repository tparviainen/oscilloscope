using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Scope.Wpf.Models
{
    class Screenshot : INotifyPropertyChanged
    {
        #region Standard pattern for implementing property changed notification
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private ImageSource _Image;

        public ImageSource Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Creates a new bitmap image and updates the Image property to reflect changed content
        /// </summary>
        /// <param name="data"></param>
        public void Update(byte[] data)
        {
            var file = Path.GetTempFileName();

            File.WriteAllBytes(file, data);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(file);
            bitmap.EndInit();

            Image = bitmap;
        }
    }
}
