using CommandLine;

namespace VK.Bot.ConsoleClient.Options
{
    internal class StatOptions : AuthOptions
    {
        [Option("user", Required = true, HelpText = "Id или имя пользователя, для которого надо собрать статистику.")]
        public string User { get; set; }
    }
}