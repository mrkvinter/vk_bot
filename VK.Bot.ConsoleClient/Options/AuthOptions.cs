using CommandLine;

namespace VK.Bot.ConsoleClient.Options
{
    public class AuthOptions
    {
        [Option('l', "login")]
        public string Login { get; set; }

        [Option('p', "password")]
        public string Password { get; set; }
    }
}