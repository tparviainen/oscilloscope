using System;
using System.Text;

namespace SCPI
{
    public class DISPLAY_DATA : ICommand
    {
        public string Description => "Read the data stream of the image currently displayed on the screen and set the color, invert display, and format of the image acquired.";

        /// <summary>
        /// The width of the data length in the TMC header
        /// </summary>
        private int N
        {
            get
            {
                return int.Parse(Encoding.ASCII.GetString(data, 1, 1));
            }
        }

        /// <summary>
        /// The length of the data stream in bytes
        /// </summary>
        private int ImageDataLength
        {
            get
            {
                return int.Parse(Encoding.ASCII.GetString(data, 2, N));
            }
        }

        private byte[] data;

        public string Command(params object[] parameters)
        {
            var cmd = ":DISPlay:DATA?";

            if (parameters.Length > 0)
            {
                var color = parameters[0].ToString();
                cmd = $"{cmd} {color}";
            }

            if (parameters.Length > 1)
            {
                var invert = parameters[1].ToString();
                cmd = $"{cmd}, {invert}";
            }

            if (parameters.Length > 2)
            {
                var format = parameters[2].ToString();
                cmd = $"{cmd}, {format}";
            }

            return cmd;
        }

        public bool Parse(byte[] data)
        {
            this.data = data;

            // Start denoter of the data stream
            if (data[0] == '#')
            {
                if (N <= 9)
                {
                    // 2 + N = TMC blockheader
                    // TMC blockheader + image data length + '\n'
                    var expectedSize = 2 + N + ImageDataLength + 1;

                    if (data.Length == expectedSize)
                    {
                        return true;
                    }
                }
            }

            // Not a valid data
            this.data = null;

            return false;
        }

        /// <summary>
        /// Returns a TMC blockheader from the data stream
        /// </summary>
        /// <returns>TMC blockheader</returns>
        public string TMCBlockheader()
        {
            return Encoding.ASCII.GetString(data, 0, N + 2);
        }

        /// <summary>
        /// Creates a new buffer that contains only the image data from the received data
        /// </summary>
        /// <returns>Image data</returns>
        public byte[] ImageData()
        {
            if (data == null)
            {
                throw new NullReferenceException();
            }

            var imageData = new byte[ImageDataLength];

            Array.Copy(data, 2 + N, imageData, 0, ImageDataLength);

            return imageData;
        }
    }
}
