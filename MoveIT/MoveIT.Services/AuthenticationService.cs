using MoveIT.Common.Helpers;
using MoveIT.Gateways.Contracts;
using MoveIT.Gateways.Contracts.Models;
using MoveIT.Services.Contracts;

namespace MoveIT.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMoveITGateway _gateway;
        private readonly Action<AuthenticationResponseModel> _persistToken;

        public AuthenticationService(IMoveITGateway gateway, Action<AuthenticationResponseModel> persistToken)
        {
            _gateway = gateway;
            _persistToken = persistToken;
        }

        public async Task<Result<AuthenticationResponseModel>> Authenticate()
        {
            var result = await _gateway.Authenticate();

            if (result.Data is not null)
            {
                _persistToken(result.Data);
            }

            return result;
        }

        public async Task<Result<AuthenticationResponseModel>> ReAuthenticate(string refreshToken)
        {
            var result = await _gateway.ReAuthenticate(refreshToken);

            if (result.Data is not null)
            {
                _persistToken(result.Data);
            }

            return result;
        }
    }
}
