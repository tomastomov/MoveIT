using MoveIT.Common.Helpers;
using MoveIT.Gateways.Contracts.Models;

namespace MoveIT.Gateways.Contracts
{
    public interface IMoveITGateway
    {
        Task<Result<AuthenticationResponseModel>> Authenticate();

        Task<Result<AuthenticationResponseModel>> ReAuthenticate(string refreshToken);

        Task<Result> UploadFileToDirectory(byte[] file, string fileName, int directoryId);
    }
}
