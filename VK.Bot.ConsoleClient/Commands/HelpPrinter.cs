using System;
using System.Text;
using CommandLine.Text;
using VK.Bot.ConsoleClient.Options;

namespace VK.Bot.ConsoleClient.Commands
{
    internal class HelpPrinter : ICommandExecutor<VoidOptions>
    {
        private readonly ICommandExecutorList commandExecutorList;

        public HelpPrinter(ICommandExecutorList commandExecutorList)
        {
            this.commandExecutorList = commandExecutorList;
        }

        public string CommandName { get; } = "help";
        public string HelpText { get; } = "Показать этот экран справки.";

        public void Execute(VoidOptions _ = default)
        {
            var sentenceBuilder = SentenceBuilder.Create();
            var helpText = new StringBuilder();

            helpText.AppendLine(HeadingInfo.Default);
            helpText.AppendLine(sentenceBuilder.UsageHeadingText());

            foreach (var command in commandExecutorList.Commands)
                helpText.AppendLine($"\t{command.CommandName}\t{command.HelpText}");


            Console.WriteLine(helpText);
        }
    }
}