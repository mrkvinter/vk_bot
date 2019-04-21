using System;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VK.Bot.ConsoleClient.Options;

namespace VK.Bot.ConsoleClient.Extensions
{
    public static class VkApiExtension
    {
        public static bool TryAuthorize(this IVkApi vkApi, AuthOptions authOptions)
        {
            try
            {
                if (!vkApi.IsAuthorized)
                {
                    if (string.IsNullOrEmpty(authOptions.Login) || string.IsNullOrEmpty(authOptions.Password))
                    {
                        Console.WriteLine(
                            "Пользователь не авторизован, пожалуйста, вызовите команду с соответствующими параметрам (логином и паролем)");
                        return false;
                    }

                    vkApi.Authorize(new ApiAuthParams
                    {
                        ApplicationId = 6952669,
                        Login = authOptions.Login,
                        Password = authOptions.Password,
                        Settings = Settings.Wall | Settings.Groups,
                        TwoFactorAuthorization = TwoFactorAuthorization,
                    });
                }
            }
            catch (VkApiException e)
            {
                Console.WriteLine($"Не удалось авторизоваться, возможно введен неверный логин или пароль.");
                return false;
            }

            return true;
        }

        private static string TwoFactorAuthorization()
        {
            Console.WriteLine(
                "Пожалуйста, введите код из личного сообщения от Администрации, чтобы подтвердить, что Вы — владелец страницы: ");
            Console.WriteLine("[Если сообщение не пришло, оставьте поле пустым]");
            return Console.ReadLine();
        }
    }
}