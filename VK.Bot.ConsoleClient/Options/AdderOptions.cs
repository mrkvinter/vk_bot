using CommandLine;

namespace VK.Bot.ConsoleClient.Options
{
    public class AdderOptions
    {
        [Value(0, Required = true)]
        public double ValueOne { get; set; }

        [Value(1, Required = true)]
        public double ValueTwo { get; set; }
    }
}