using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace VK.Bot.ConsoleClient.Options
{
    class StatOptions
    {
        [Option("id", Required = true, HelpText = "Id пользователя, для которого надо собрать статистику.")]
        public int UserId { get; set; }
    }
}
