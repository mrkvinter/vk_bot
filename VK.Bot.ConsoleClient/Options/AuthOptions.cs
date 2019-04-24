using CommandLine;

namespace VK.Bot.ConsoleClient.Options
{
    public class AuthOptions
    {
        [Option('l', "login", HelpText = "Логин для входа. [При успешной авторизации, не обязательно вводить]")]
        public string Login { get; set; }

        [Option('p', "password", HelpText = "Пароль для входа. [При успешной авторизации, не обязательно вводить]")]
        public string Password { get; set; }
    }
}