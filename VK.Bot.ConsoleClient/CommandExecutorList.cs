using System;
using System.Collections.Generic;
using CommandLine;
using VK.Bot.ConsoleClient.Commands;
using VK.Bot.ConsoleClient.Extensions;

namespace VK.Bot.ConsoleClient
{
    public interface ICommandExecutorList
    {
        List<IHelpable> Commands { get; }
        Action<string[]> this[string commandName] { get; }
        void Register<T>(IHelpableCommand<T> commandExecutor);
        bool ContainsCommand(string command);
    }

    public class CommandExecutorList : ICommandExecutorList
    {
        private readonly Dictionary<string, Action<string[]>> commandExecutors =
            new Dictionary<string, Action<string[]>>();

        private readonly Parser parser;

        public CommandExecutorList(Parser parser)
        {
            this.parser = parser;
        }

        public List<IHelpable> Commands { get; } = new List<IHelpable>();

        public Action<string[]> this[string commandName] => commandExecutors[commandName];

        public void Register<T>(IHelpableCommand<T> commandExecutor)
        {
            Commands.Add(commandExecutor);
            commandExecutors.Add(
                commandExecutor.CommandName,
                args =>
                {
                    commandExecutor
                        .ParseArgs(args, parser)
                        .WithParsed(e => commandExecutor.Execute(e))
                        .PrintErrorsWithNotParsed();
                });
        }

        public bool ContainsCommand(string command)
        {
            return commandExecutors.ContainsKey(command);
        }
    }
}