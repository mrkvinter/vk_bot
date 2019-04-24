using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model;
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
        private readonly ITwitStatCollector twitStatCollector;
        private readonly IVkApi vkApi;
        private readonly IVkAuthorizer vkAuthorizer;

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

            var foundResultUser = vkApi.Users.TryGet(target);
            if (foundResultUser.WasError)
                return FoundResult<string>.Error($"Во время поиска пользователя {target} произошла ошибка.");

            var userInfo = foundResultUser.Value;

            var foundResultStat = TryCollectCharsStat(userInfo);
            if (foundResultStat.WasError)
                return FoundResult<string>.Error(foundResultStat.MessageError);

            var json = JsonConvert.SerializeObject(foundResultStat.Value
                .Select(e => new KeyValuePair<char, double>(e.Key, Math.Round(e.Value, 2)))
                .ToDictionary(e => e.Key, e => e.Value));

            vkApi.Wall.Post(new WallPostParams
            {
                OwnerId = -181436132,
                Message = $"{userInfo.FirstName} {userInfo.LastName}, статистика для последних 5 постов: {json}"
            });

            return FoundResult<string>.Success(json);
        }

        private FoundResult<Dictionary<char, double>> TryCollectCharsStat(User userInfo)
        {
            try
            {
                var stat = twitStatCollector.Collect(userInfo.Id);
                return FoundResult<Dictionary<char, double>>.Success(stat);
            }
            catch (VkApiException e)
            {
                return FoundResult<Dictionary<char, double>>.Error(e.Message);
            }
        }
    }
}