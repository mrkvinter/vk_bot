using System;
using System.Linq;
using Autofac;

namespace VK.Bot.ConsoleClient
{
    internal class Program
    {
        private static void Main()
        {
            using (var container = VkContainerBuilder.Build(TwoFactorAuthorization))
            {
                var commandsList = container.Resolve<ICommandExecutorList>();

                while (true)
                {
                    Console.WriteLine("Пожалуйста, введите команду [help - посмотреть доступные команды]:");
                    var (commandName, commandArgs) = ParseCommandLine(Console.ReadLine());

                    if (commandName == "exit" || commandName == "")
                        break;

                    if (!commandsList.ContainsCommand(commandName))
                    {
                        Console.WriteLine($"Неизвестная команда {commandName}");
                        break;
                    }

                    try
                    {
                        commandsList[commandName](commandArgs);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(
                            $"Вовремя выполнения команды {commandName} произошла непредвиденная ошибка.");
                    }
                }
            }
        }

        private static string TwoFactorAuthorization()
        {
            Console.WriteLine(
                "Пожалуйста, введите код из личного сообщения от Администрации, чтобы подтвердить, что Вы — владелец страницы: ");
            Console.WriteLine("[Если сообщение не пришло, оставьте поле пустым]");
            return Console.ReadLine();
        }

        private static (string, string[]) ParseCommandLine(string line)
        {
            if (line == null)
                return ("", null);

            var splitArgs = line.Trim().ToLower().Split();
            var commandName = splitArgs[0];
            var commandArgs = splitArgs.Skip(1).ToArray();

            return (commandName, commandArgs);
        }
    }
}