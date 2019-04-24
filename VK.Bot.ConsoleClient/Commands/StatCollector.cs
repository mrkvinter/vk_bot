using System;
using VK.Bot.ConsoleClient.Options;

namespace VK.Bot.ConsoleClient.Commands
{
    internal class StatCollector : ICommandExecutor<StatOptions>
    {
        private readonly IVkStatTwitNotifier statTwitNotifier;

        public StatCollector(IVkStatTwitNotifier statTwitNotifier)
        {
            this.statTwitNotifier = statTwitNotifier;
        }

        public void Execute(StatOptions options)
        {
            var result = statTwitNotifier.Notify(options.User, options.Login, options.Password);

            if (result.WasError)
            {
                Console.WriteLine("Во время выполнения произошла ошибка: ");
                Console.WriteLine(result.MessageError);
            }
            else
            {
                Console.WriteLine("Задача успешно завершена. Статистика: ");
                Console.WriteLine(result.Value);
            }
        }

        public string CommandName { get; } = "stat";
        public string HelpText { get; } = "Получить статистику с последних 5 постов {id} аккаунта в vk";
    }
}