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

            if (totalCountChars == 0)
                return new Dictionary<char, double>();

            return letters
                .GroupBy(e => e)
                .ToDictionary(e => e.Key, e => (double) e.Count() / totalCountChars);
        }
    }
}