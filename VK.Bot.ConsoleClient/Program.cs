using System;
using System.IO;
using System.Linq;
using CommandLine;
using VkNet;
using VK.Bot.ConsoleClient.Commands;

namespace VK.Bot.ConsoleClient
{
    internal class Program
    {
        private static void Main()
        {
            var vkApi = new VkApi();

            var twitStatCollector = new TwitStatCollector(new FrequencyCounter(), vkApi);

            var parser = new Parser(e => { e.HelpWriter = TextWriter.Null; });

            var commandsList = new CommandExecutorList(parser);

            commandsList.Register(new Adder());
            commandsList.Register(new HelpPrinter(commandsList));
            commandsList.Register(new StatCollector(
                vkApi,
                twitStatCollector));

            while (true)
            {
                Console.WriteLine("Пожалуйста, введите комманду [help - посмотреть доступные команды]:");
                var (commandName, commandArgs) = ParseCommandLine(Console.ReadLine());

                if (commandName == "exit" || commandName == "")
                    break;

                try
                {
                    commandsList[commandName](commandArgs);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Вовремя выполнения команды {commandName} произошла не предвиденная ошибка.");
                }
            }
        }

        private static (string, string[]) ParseCommandLine(string line)
        {
            if (line == null)
                return ("", null);

            var splitArgs = line.Trim().ToLower().Split();
            var commandName = splitArgs[0];
            var commandArgs = splitArgs.Skip(1).ToArray();

            return (commandName, commandArgs);
        }
    }
}