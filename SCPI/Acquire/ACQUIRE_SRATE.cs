using SCPI.Extensions;
using System.Text;

namespace SCPI.Acquire
{
    public class ACQUIRE_SRATE : ICommand
    {
        public string Description => "Query the current sample rate. The default unit is Sa/s.";

        public float SampleRate { get; private set; }

        public string Command(params string[] parameters) => ":ACQuire:SRATe?";

        public string HelpMessage()
        {
            var syntax = nameof(ACQUIRE_SRATE) + "\n";
            var example = "Example: " + nameof(ACQUIRE_SRATE);

            return $"{syntax}\n{example}";
        }

        public bool Parse(byte[] data)
        {
            if (data == null)
            {
                return false;
            }

            if (Encoding.ASCII.GetString(data).ScientificNotationStringToFloat(out float sampleRate))
            {
                SampleRate = sampleRate;

                return true;
            }

            return false;
        }
    }
}
