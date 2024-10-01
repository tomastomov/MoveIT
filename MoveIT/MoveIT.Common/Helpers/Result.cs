namespace MoveIT.Common.Helpers
{
    public class Result<T>
    {
        public T? Data { get; private set; }

        public string? ErrorMessage { get; private set; }

        public static Result<T> ToError(string errorMessage)
            => new Result<T>
            {
                ErrorMessage = errorMessage,
            };

        public static Result ToVoidError(string errorMessage)
            => new Result
            {
                ErrorMessage = errorMessage,
            };

        public static Result<T> ToResult(T data)
            => new Result<T>
            {
                Data = data,
            };

        public static Result ToEmptyResult()
            => new Result { };
    }

    public class Result : Result<object>
    {

    }
}
