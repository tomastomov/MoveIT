namespace MoveIT.Gateways.Contracts
{
    public class Result<T>
    {
        public T? Data { get; set; }

        public string? ErrorMessage { get; set; }
    }

    public class Result : Result<object>
    {

    }
}
