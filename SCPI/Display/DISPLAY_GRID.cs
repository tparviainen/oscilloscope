using System;
using System.Text;

namespace SCPI.Display
{
    public class DISPLAY_GRID : ICommand
    {
        public string Description => "Set or query the grid type of screen display.";

        public string Grid { get; private set; }

        private readonly string[] gridRange = new string[] { "FULL", "HALF", "NONE" };

        public string Command(params string[] parameters)
        {
            var cmd = ":DISPlay:GRID";

            if (parameters.Length > 0)
            {
                var grid = parameters[0];
                cmd = $"{cmd} {grid}";
            }
            else
            {
                cmd += "?";
            }

            return cmd;
        }

        public string HelpMessage()
        {
            var syntax = nameof(DISPLAY_GRID) + "\n" +
                         nameof(DISPLAY_GRID) + " <grid>";
            var parameters = " <grid> = {"+ string.Join("|", gridRange) +"}\n";
            var example = "Example: " + nameof(DISPLAY_GRID) + "?";

            return $"{syntax}\n{parameters}\n{example}";
        }

        public bool Parse(byte[] data)
        {
            if (data != null)
            {
                Grid = Encoding.ASCII.GetString(data).Trim();
                if (Array.Exists(gridRange, g => g.Equals(Grid)))
                {
                    return true;
                }
            }

            Grid = null;

            return false;
        }
    }
}
