using System;
using log4net;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VK.Bot.Models;

namespace VK.Bot
{
    public interface IVkAuthorizer
    {
        Result TryAuthorize(IVkApi vkApi, string login, string password);
    }

    public class VkAuthorizer : IVkAuthorizer
    {
        private readonly Func<string> twoFactorAuthorization;
        private readonly ILog log;

        public VkAuthorizer(Func<string> twoFactorAuthorization, ILog log)
        {
            this.twoFactorAuthorization = twoFactorAuthorization;
            this.log = log;
        }

        public Result TryAuthorize(IVkApi vkApi, string login, string password)
        {
            try
            {
                if (!vkApi.IsAuthorized)
                {
                    if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                        return Result.Error(
                            "Пользователь не авторизован, пожалуйста, вызовите команду с соответствующими параметрам (логином и паролем)");

                    vkApi.Authorize(new ApiAuthParams
                    {
                        ApplicationId = 6952669,
                        Login = login,
                        Password = password,
                        Settings = Settings.Wall | Settings.Groups,
                        TwoFactorAuthorization = twoFactorAuthorization
                    });
                }
            }
            catch (VkApiException e)
            {
                log.Error("User cannot be authorized.", e);
                return Result.Error("Не удалось авторизоваться, возможно введен неверный логин или пароль.");
            }

            return Result.Success();
        }
    }
}