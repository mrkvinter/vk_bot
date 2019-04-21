using System;
using CommandLine;
using CommandLine.Text;

namespace VK.Bot.ConsoleClient.Extensions
{
    public static class ParserResultExtension
    {
        public static ParserResult<T> PrintErrorsWithNotParsed<T>(this ParserResult<T> parserResult)
        {
            return parserResult.WithNotParsed(e =>
            {
                var helpText = new HelpText
                    {Copyright = "", Heading = "", AutoVersion = false, AutoHelp = false, AddDashesToOption = true};
                helpText = HelpText.DefaultParsingErrorsHandler(parserResult, helpText);
                helpText.AddOptions(parserResult);

                Console.WriteLine(helpText);
            });
        }
    }
}