using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SCPI.Extensions;

namespace SCPI.Display
{
    public class DISPLAY_WBRIGHTNESS : ICommand
    {
        public string Description => "Set or query the waveform brightness.";

        public int Brightness { get; private set; }

        public string Command(params string[] parameters)
        {
            var cmd = ":DISPlay:WBRightness";

            if (parameters.Length > 0)
            {
                var brightness = int.Parse(parameters[0]);
                if (brightness.IsWithin(0, 100))
                {
                    cmd = $"{cmd} {brightness}";
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                cmd += "?";
            }

            return cmd;
        }

        public string HelpMessage()
        {
            var syntax = nameof(DISPLAY_WBRIGHTNESS) + "\n" +
                         nameof(DISPLAY_WBRIGHTNESS) + " <brightness>";
            var parameters = " <brightness> = 0 to 100\n";
            var example = "Example: " + nameof(DISPLAY_WBRIGHTNESS) + " 60";

            return $"{syntax}\n{parameters}\n{example}";
        }

        public bool Parse(byte[] data)
        {
            if (data != null)
            {
                if (int.TryParse(Encoding.ASCII.GetString(data), out int brightness))
                {
                    Brightness = brightness;

                    return true;
                }
            }

            Brightness = 0;

            return false;
        }
    }
}
