using CommandLine;
using VK.Bot.ConsoleClient.Commands;

namespace VK.Bot.ConsoleClient.Extensions
{
    public static class CommandExecutorExtension
    {
        public static ParserResult<T> ParseArgs<T>(this ICommand<T> executor, string[] args, Parser parser)
        {
            return parser.ParseArguments<T>(args);
        }
    }
}