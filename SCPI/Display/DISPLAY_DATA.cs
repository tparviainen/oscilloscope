using System;
using System.Text;

namespace SCPI.Display
{
    public class DISPLAY_DATA : ICommand
    {
        public string Description => "Read the data stream of the image currently displayed on the screen and set the color, invert display, and format of the image acquired.";

        /// <summary>
        /// The width of the data length in the TMC header
        /// </summary>
        private int N => int.Parse(Encoding.ASCII.GetString(data, 1, 1));

        /// <summary>
        /// The length of the data stream in bytes
        /// </summary>
        private int ImageDataLength => int.Parse(Encoding.ASCII.GetString(data, 2, N));

        private byte[] data;

        public string HelpMessage()
        {
            var parameters = nameof(DISPLAY_DATA) + " [<color>, <invert>, <format>]\n";

            parameters += " <color>, {ON|OFF}\n";
            parameters += " <invert>, {{1|ON}|{0|OFF}}\n";
            parameters += " <format>, {BMP24|BMP8|PNG|JPEG|TIFF}\n\n";

            parameters += "Example: " + nameof(DISPLAY_DATA) + " ON, 0, BMP24";

            return parameters;
        }

        public string Command(params string[] parameters)
        {
            var cmd = ":DISPlay:DATA?";

            if (parameters.Length > 0)
            {
                var color = parameters[0];
                cmd = $"{cmd} {color}";
            }

            if (parameters.Length > 1)
            {
                var invert = parameters[1];
                cmd = $"{cmd}, {invert}";
            }

            if (parameters.Length > 2)
            {
                var format = parameters[2];
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
        public string TMCBlockheader() => Encoding.ASCII.GetString(data, 0, N + 2);

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
