using System;
using System.Text;
using CommandLine.Text;
using VK.Bot.ConsoleClient.Options;

namespace VK.Bot.ConsoleClient.Commands
{
    internal class HelpPrinter : ICommandExecutor<VoidOptions>
    {
        private readonly CommandExecutorList commandExecutorList;
        public string CommandName => "help";
        public string HelpText => "Показать этот экран справки.";

        public HelpPrinter(CommandExecutorList commandExecutorList)
        {
            this.commandExecutorList = commandExecutorList;
        }

        public void Execute(VoidOptions _ = default)
        {
            var sentenceBuilder = SentenceBuilder.Create();
            var helpText = new StringBuilder();

            helpText.AppendLine(HeadingInfo.Default);
            helpText.AppendLine(sentenceBuilder.UsageHeadingText());

            foreach (var command in commandExecutorList.Commands)
            {
                helpText.AppendLine($"\t{command.CommandName}\t{command.HelpText}");
            }


            Console.WriteLine(helpText);
        }
    }
}