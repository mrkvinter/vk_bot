using System.Collections.Generic;
using System.Linq;

namespace VK.Bot
{
    public interface IFrequencyCounter
    {
        Dictionary<char, double> GetFrequency(string[] texts);
    }

    public class FrequencyCounter : IFrequencyCounter
    {
        public Dictionary<char, double> GetFrequency(string[] texts)
        {
            var totalText = string.Join("", texts);

            var letters = totalText
                .ToLower()
                .Where(char.IsLetter)
                .ToArray();

            var totalCountChars = letters.Length;
            var stat = letters
                .GroupBy(e => e)
                .ToDictionary(e => e.Key, e => (double) e.Count() / totalCountChars);

            return stat;
        }
    }
}