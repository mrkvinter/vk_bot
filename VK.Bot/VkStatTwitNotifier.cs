using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;
using VK.Bot.Extensions;
using VK.Bot.Models;

namespace VK.Bot
{
    public interface IVkStatTwitNotifier
    {
        FoundResult<string> Notify(string target, string login, string password);
    }
    public class VkStatTwitNotifier : IVkStatTwitNotifier
    {
        private readonly IVkAuthorizer vkAuthorizer;
        private readonly IVkApi vkApi;
        private readonly ITwitStatCollector twitStatCollector;

        public VkStatTwitNotifier(IVkAuthorizer vkAuthorizer, IVkApi vkApi, ITwitStatCollector twitStatCollector)
        {
            this.vkAuthorizer = vkAuthorizer;
            this.vkApi = vkApi;
            this.twitStatCollector = twitStatCollector;
        }

        public FoundResult<string> Notify(string target, string login, string password)
        {
            var resultAuthorize = vkAuthorizer.TryAuthorize(vkApi, login, password);
            if (resultAuthorize.WasError)
                return FoundResult<string>.Error($"Не удалось авторизоваться. {resultAuthorize.MessageError}");

            var foundResult = vkApi.Users.TryGet(target);

            if (foundResult.WasError)
                return FoundResult<string>.Error($"Во время поиска пользователя {target} произошла ошибка.");

            var userInfo = foundResult.Value;

            //todo: Обработать. У пользователя может быть закрыта стена.
            var stat = twitStatCollector.Collect(userInfo.Id);

            var json = JsonConvert.SerializeObject(stat
                .Select(e => new KeyValuePair<char, double>(e.Key, Math.Round(e.Value, 2)))
                .ToDictionary(e => e.Key, e => e.Value));

            vkApi.Wall.Post(new WallPostParams
            {
                OwnerId = -181436132,
                Message = $"{userInfo.FirstName} {userInfo.LastName}, статистика для последних 5 постов: {json}"
            });

            return FoundResult<string>.Success(json);
        }
    }
}