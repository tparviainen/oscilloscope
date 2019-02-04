using System.Text;

namespace SCPI.System
{
    public class SYSTEM_LANGUAGE : ICommand
    {
        public string Description => "Set or query the system language";

        public string Language { get; private set; }

        public string Command(params string[] parameters)
        {
            var cmd = ":SYSTem:LANGuage";

            if (parameters.Length > 0)
            {
                var language = parameters[0];
                cmd = $"{cmd} {language}";
            }
            else
            {
                cmd += "?";
            }

            return cmd;
        }

        public string HelpMessage()
        {
            var syntax = nameof(SYSTEM_LANGUAGE) + "\n" +
                         nameof(SYSTEM_LANGUAGE) + " <lang>";

            var parameters = " <lang> = SCH, TCH, ENGL, PORT, GERM, POL, KOR, JAPA, FREN or RUSS\n";

            var example = "Example: " + nameof(SYSTEM_LANGUAGE) + " ENGL";

            return $"{syntax}\n{parameters}\n{example}";
        }

        public bool Parse(byte[] data)
        {
            if (data == null)
            {
                return false;
            }

            Language = Encoding.ASCII.GetString(data).Trim();

            return true;
        }
    }
}
