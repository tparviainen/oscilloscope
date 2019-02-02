namespace SCPI
{
    public class RAW : ICommand
    {
        public string Description => "Send a raw SCPI command to instrument";

        public string Command(params string[] parameters)
        {
            if (parameters.Length != 0)
            {
                if (parameters.Length > 1)
                {
                    return $"{parameters[0]} {string.Join(", ", parameters, 1, parameters.Length - 1)}";
                }

                return parameters[0];
            }

            return string.Empty;
        }

        public string HelpMessage()
        {
            var message = nameof(RAW) + " SCPI_COMMAND <arg1> <arg2> ... <argn>\n";

            message += " <arg1>, extra argument to command\n";
            message += " <arg2>, extra argument to command\n\n";

            message += "Example: " + nameof(RAW) + " :DISPlay:DATA? ON 0 TIFF\n";
            message += "Example: " + nameof(RAW) + " *IDN?";

            return message;
        }

        public bool Parse(byte[] data) => true;
    }
}
