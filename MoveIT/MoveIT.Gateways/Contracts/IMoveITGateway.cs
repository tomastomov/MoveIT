namespace MoveIT.Gateways.Contracts
{
    public interface IMoveITGateway
    {
        Task<string> Login(string username);

        Task UploadFileToDirectory(byte[] file, string fileName, string directory);
    }
}
