namespace VK.Bot.Models
{
    public class FoundResult<T>
    {
        public FoundResult(T value)
        {
            WasError = false;
            Value = value;
        }

        public FoundResult(string messageError)
        {
            WasError = true;
            MessageError = messageError;
        }

        public T Value { get; }

        public bool WasError { get; }

        public string MessageError { get; }
    }
}