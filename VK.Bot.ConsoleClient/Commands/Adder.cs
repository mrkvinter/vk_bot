using System;
using VK.Bot.ConsoleClient.Options;

namespace VK.Bot.ConsoleClient.Commands
{
    class Adder : ICommandExecutor<AdderOptions>
    {
        public void Execute(AdderOptions options)
        {
            var sum = options.ValueOne + options.ValueTwo;

            Console.WriteLine($"{options.ValueOne} + {options.ValueTwo} = {sum}");
        }

        public string CommandName { get; } = "add";
        public string HelpText { get; } = "Сложить два числа, и вывести на экран";
    }
}
