using System.Collections.Generic;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace VK.Bot
{
    public interface ITwitStatCollector
    {
        Dictionary<char, double> Collect(long userId);
    }

    public class TwitStatCollector : ITwitStatCollector
    {
        private readonly IFrequencyCounter frequencyCounter;
        private readonly IVkApi vkApi;

        public TwitStatCollector(IFrequencyCounter frequencyCounter, IVkApi vkApi)
        {
            this.frequencyCounter = frequencyCounter;
            this.vkApi = vkApi;
        }

        public Dictionary<char, double> Collect(long userId)
        {
            var wallGetObject = vkApi.Wall.Get(new WallGetParams {OwnerId = userId, Count = 5}, true);
            var frequencies = frequencyCounter.GetFrequency(wallGetObject.WallPosts.Select(p => p.Text).ToArray());

            return frequencies;
        }
    }
}
