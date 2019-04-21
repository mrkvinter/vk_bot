using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using VK.Bot.ConsoleClient.Extensions;
using VK.Bot.ConsoleClient.Options;

namespace VK.Bot.ConsoleClient.Commands
{
    internal class StatCollector : ICommandExecutor<StatOptions>
    {
        private readonly ITwitStatCollector twitStatCollector;
        private readonly IVkApi vkApi;

        public StatCollector(IVkApi vkApi, ITwitStatCollector twitStatCollector)
        {
            this.vkApi = vkApi;
            this.twitStatCollector = twitStatCollector;
        }

        public void Execute(StatOptions options)
        {
            if (!vkApi.TryAuthorize(options)) return;

            var userInfo = vkApi.Users.Get(new List<string> {options.User}).FirstOrDefault();

            if (userInfo == null)
            {
                Console.WriteLine($"Пользователь {options.User} не найден.");
                return;
            }

            var stat = twitStatCollector.Collect(userInfo.Id);


            var json = JsonConvert.SerializeObject(stat.Select(e => new KeyValuePair<char, double>(e.Key, Math.Round(e.Value, 2))).ToDictionary(e => e.Key, e => e.Value));

            vkApi.Wall.Post(new WallPostParams
            {
                OwnerId = -181436132,
                Message = $"{userInfo.FirstName} {userInfo.LastName}, статистика для последних 5 постов: {json}"
            });

            Console.WriteLine(json);
        }


        public string CommandName => "stat";
        public string HelpText => "Получить статистику с последних 5 постов {id} аккаунта в vk";
    }
}