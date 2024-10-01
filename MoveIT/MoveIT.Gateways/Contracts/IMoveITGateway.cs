using MoveIT.Common.Helpers;

namespace MoveIT.Gateways.Contracts
{
    public interface IMoveITGateway
    {
        Task<Result<string>> Login(string username);

        Task<Result> UploadFileToDirectory(byte[] file, string fileName, int directoryId);
    }
}
