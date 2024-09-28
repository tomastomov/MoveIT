namespace MoveIT.Gateways.Contracts
{
    public interface IMoveITGateway
    {
        Task<string> LoginAsync(string username);
    }
}
