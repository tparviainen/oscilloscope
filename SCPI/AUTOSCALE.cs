using System;
using System.Collections.Generic;
using System.Text;

namespace SCPI
{
    public class AUTOSCALE : ICommand
    {
        public string Description => "The oscilloscope will automatically adjust the vertical scale, horizontal timebase, and trigger mode according to the input signal to realize optimum waveform display";

        public string Command(params string[] parameters) => ":AUToscale";

        public string HelpMessage() => nameof(AUTOSCALE);

        public bool Parse(byte[] data) => true;
    }
}
