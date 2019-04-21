using System;
using VkNet.Abstractions;
using VkNet.Model;
using VK.Bot.ConsoleClient.Options;

namespace VK.Bot.ConsoleClient.Commands
{
    internal class StatCollector : ICommandExecutor<StatOptions>
    {
        private readonly Func<string> tokenGetter;
        private readonly ITwitStatCollector twitStatCollector;
        private readonly IVkApi vkApi;

        public StatCollector(IVkApi vkApi, ITwitStatCollector twitStatCollector, Func<string> tokenGetter)
        {
            this.vkApi = vkApi;
            this.twitStatCollector = twitStatCollector;
            this.tokenGetter = tokenGetter;
        }

        public void Execute(StatOptions options)
        {
            vkApi.Authorize(new ApiAuthParams
            {
                AccessToken = tokenGetter()
            });

            var stat = twitStatCollector.Collect(options.UserId);

            foreach (var d in stat)
                Console.Write($"{d.Key} : {d.Value*100:0}%\t");
        }

        public string CommandName => "stat";
        public string HelpText => "Получить статистику с последних 5 постов {id} аккаунта в vk";
    }
}