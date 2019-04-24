using System;

namespace VK.Bot.Models
{
    public class Result
    {
        public bool WasError { get; protected set; }

        public string MessageError { get; protected set; }

        public Exception Exception { get; protected set; }

        public static Result Error(string message, Exception exception = default)
        {
            return new Result {WasError = true, MessageError = message, Exception = exception};
        }

        public static Result Success()
        {
            return new Result {WasError = false};
        }
    }

    public class FoundResult<T> : Result
    {
        public T Value { get; private set; }

        public new static FoundResult<T> Error(string message, Exception exception = default)
        {
            return new FoundResult<T> {WasError = true, MessageError = message, Exception = exception};
        }

        public static FoundResult<T> Success(T value)
        {
            return new FoundResult<T> {WasError = false, Value = value};
        }
    }
}