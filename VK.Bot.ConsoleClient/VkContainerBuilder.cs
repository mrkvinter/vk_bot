using System.IO;
using Autofac;
using CommandLine;
using VkNet;
using VkNet.Abstractions;
using VK.Bot.ConsoleClient.Commands;

namespace VK.Bot.ConsoleClient
{
    public static class VkContainerBuilder
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<VkApi>().SingleInstance().As<IVkApi>();
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