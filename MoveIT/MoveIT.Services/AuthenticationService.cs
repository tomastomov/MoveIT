using MoveIT.Common.Helpers;
using MoveIT.Gateways.Contracts;
using MoveIT.Services.Contracts;

namespace MoveIT.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMoveITGateway _gateway;
        private readonly Action<string> _persistToken;

        public AuthenticationService(IMoveITGateway gateway, Action<string> persistToken)
        {
            _gateway = gateway;
            _persistToken = persistToken;
        }

        public async Task<Result<string>> Authenticate()
        {
            var result = await _gateway.Login("test");

            if (result.Data is not null)
            {
                _persistToken(result.Data);
            }

            return result;
        }
    }
}
