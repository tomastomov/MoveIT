using MoveIT.Common.Helpers;
using MoveIT.Gateways.Contracts.Models;

namespace MoveIT.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticationResponseModel>> Authenticate();

        Task<Result<AuthenticationResponseModel>> ReAuthenticate(string refreshToken);
    }
}
