namespace MoveIT.Gateways.Contracts
{
    public interface IMoveITGateway
    {
        Task<string> Login(string username);
    }
}
