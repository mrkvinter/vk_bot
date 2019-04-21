namespace VK.Bot.ConsoleClient.Commands
{
    public interface ICommand
    {
        string CommandName { get; }
        string HelpText { get; }
    }

    public interface ICommandExecutor<T> : ICommand
    {
        void Execute(T options);
    }
}