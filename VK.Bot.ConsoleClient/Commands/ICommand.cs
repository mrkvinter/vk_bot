namespace VK.Bot.ConsoleClient.Commands
{
    public interface IHelpable
    {
        string CommandName { get; }
        string HelpText { get; }
    }

    public interface ICommand<T>
    {
        void Execute(T options);
    }

    public interface IHelpableCommand<T> : ICommand<T>, IHelpable { }
}