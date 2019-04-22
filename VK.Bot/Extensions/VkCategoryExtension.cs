using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VK.Bot.Models;

namespace VK.Bot.Extensions
{
    public static class VkCategoryExtension
    {
        public static FoundResult<User> TryGet(
            this IUsersCategory users,
            string screenName,
            ProfileFields fields = null,
            NameCase nameCase = null,
            bool skipAuthorization = false)
        {
            try
            {
                var result = users.Get(new List<string> {screenName}, fields, nameCase, skipAuthorization)
                    .FirstOrDefault();

                return result == null
                    ? new FoundResult<User>("Пользователь не найден.")
                    : new FoundResult<User>(result);
            }
            catch (InvalidUserIdException e)
            {
                return new FoundResult<User>(e.Message);
            }
            catch (Exception e)
            {
                return new FoundResult<User>($"Неизвестная ошибка. {e.Message}");
            }
        }
    }
}