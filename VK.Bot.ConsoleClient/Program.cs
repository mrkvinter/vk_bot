using System;
using System.IO;
using System.Linq;
using Autofac;
using CommandLine;
using VkNet;
using VkNet.Abstractions;
using VK.Bot.ConsoleClient.Commands;

namespace VK.Bot.ConsoleClient
{
    internal class Program
    {
        private static void Main()
        {
            using (var container = VkContainerBuilder.Build())
            {
                var commandsList = container.Resolve<ICommandExecutorList>();

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
                        Console.Error.WriteLine(
                            $"Вовремя выполнения команды {commandName} произошла не предвиденная ошибка.");
                    }
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

    public static class VkContainerBuilder
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<VkApi>().As<IVkApi>();
            builder.RegisterType<FrequencyCounter>().As<IFrequencyCounter>();
            builder.RegisterType<TwitStatCollector>().As<ITwitStatCollector>();
            builder.Register(c => new Parser(e => { e.HelpWriter = TextWriter.Null; })).As<Parser>();
            builder.RegisterCommandExecutorList();

            return builder.Build();
        }

        private static void RegisterCommandExecutorList(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(c =>
            {
                var commandExecutorList = new CommandExecutorList(c.Resolve<Parser>());
                commandExecutorList.Register(new Adder());
                commandExecutorList.Register(new HelpPrinter(commandExecutorList));
                commandExecutorList.Register(new StatCollector(
                    c.Resolve<IVkApi>(),
                    c.Resolve<ITwitStatCollector>()));

                return commandExecutorList;
            }).As<ICommandExecutorList>();
        }
    }
}