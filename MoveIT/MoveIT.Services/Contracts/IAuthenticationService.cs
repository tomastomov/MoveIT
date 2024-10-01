using MoveIT.Common.Helpers;

namespace MoveIT.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<string>> Authenticate();
    }
}
